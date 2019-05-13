using Log2Net.Models;
using Log2Net.Util.DBUtil.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;


namespace Log2Net.Util
{

    /// <summary>
    /// lambda表达式转为where条件sql
    /// </summary>
    internal class LambdaToSqlHelper<T>
    {
        #region Expression 转成 where
        /// <summary>
        /// Expression 转成 Where String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="databaseType">数据类型（用于字段是否加引号）</param>
        /// <returns></returns>
        public static string GetWhereFromLambda(Expression<Func<T, bool>> predicate, DataBaseType databaseType)
        {
            bool withQuotationMarks = GetWithQuotationMarks(databaseType);

            ExpressBool.ConditionBuilder conditionBuilder = new ExpressBool.ConditionBuilder();
            conditionBuilder.SetIfWithQuotationMarks(withQuotationMarks); //字段是否加引号（PostGreSql,Oracle）
            conditionBuilder.SetDataBaseType(databaseType);
            conditionBuilder.Build(predicate);

            for (int i = 0; i < conditionBuilder.Arguments.Length; i++)
            {
                object ce = conditionBuilder.Arguments[i];
                if (ce == null)
                {
                    conditionBuilder.Arguments[i] = DBNull.Value;
                }
                else if (ce is string || ce is char)
                {
                    if (ce.ToString().ToLower().Trim().IndexOf(@"in(") == 0 ||
                        ce.ToString().ToLower().Trim().IndexOf(@"not in(") == 0 ||
                         ce.ToString().ToLower().Trim().IndexOf(@" like '") == 0 ||
                        ce.ToString().ToLower().Trim().IndexOf(@"not like") == 0)
                    {
                        conditionBuilder.Arguments[i] = string.Format(" {0} ", ce.ToString());
                    }
                    else
                    {
                        //****************************************
                        conditionBuilder.Arguments[i] = string.Format("'{0}'", ce.ToString());
                    }
                }
                else if (ce is DateTime)
                {
                    conditionBuilder.Arguments[i] = string.Format("'{0}'", ce.ToString());
                }
                else if (ce is int || ce is long || ce is short || ce is decimal || ce is double || ce is float || ce is bool || ce is byte || ce is sbyte)
                {
                    conditionBuilder.Arguments[i] = ce.ToString();
                }
                else if (ce is ValueType)
                {
                    conditionBuilder.Arguments[i] = ce.ToString();
                }
                else
                {

                    conditionBuilder.Arguments[i] = string.Format("'{0}'", ce.ToString());
                }

            }
            string strWhere = string.Format(conditionBuilder.Condition, conditionBuilder.Arguments);
            return strWhere;
        }


        public static LamSQL GetSqlFromLambda(Expression<Func<IQueryable<T>, IQueryable<T>>> exp)
        {
            ExpressQueryable<T>.AiExpConditions aiExpConditions = new ExpressQueryable<T>.AiExpConditions();
            aiExpConditions.SetCondition(exp);
            string where = aiExpConditions.GetWhere(exp);
            string orderby = aiExpConditions.GetOrderBy(exp);
            return new LamSQL() { WhereSql = where, OrderbySql = orderby };
        }



        /// <summary>
        /// 获取是否字段加双引号
        /// </summary>
        /// <param name="databaseType"></param>
        /// <returns></returns>
        static bool GetWithQuotationMarks(DataBaseType databaseType)
        {
            bool result = false;
            switch (databaseType)
            {

                case DataBaseType.PostgreSQL:
                case DataBaseType.Oracle:

                    result = true;
                    break;
            }

            return result;


        }


        #endregion
    }

    /// <summary>
    /// 从Expression<Func<T, bool>> predicate 中得到where sql
    /// </summary>
    internal class ExpressBool
    {
        public abstract class ExpressionVisitor
        {
            protected ExpressionVisitor() { }

            protected virtual Expression Visit(Expression exp)
            {
                if (exp == null)
                    return exp;
                switch (exp.NodeType)
                {
                    case ExpressionType.Negate:
                    case ExpressionType.NegateChecked:
                    case ExpressionType.Not:
                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked:
                    case ExpressionType.ArrayLength:
                    case ExpressionType.Quote:
                    case ExpressionType.TypeAs:
                        return this.VisitUnary((UnaryExpression)exp);
                    case ExpressionType.Add:
                    case ExpressionType.AddChecked:
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractChecked:
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyChecked:
                    case ExpressionType.Divide:
                    case ExpressionType.Modulo:
                    case ExpressionType.And:
                    case ExpressionType.AndAlso:
                    case ExpressionType.Or:
                    case ExpressionType.OrElse:
                    case ExpressionType.LessThan:
                    case ExpressionType.LessThanOrEqual:
                    case ExpressionType.GreaterThan:
                    case ExpressionType.GreaterThanOrEqual:
                    case ExpressionType.Equal:
                    case ExpressionType.NotEqual:
                    case ExpressionType.Coalesce:
                    case ExpressionType.ArrayIndex:
                    case ExpressionType.RightShift:
                    case ExpressionType.LeftShift:
                    case ExpressionType.ExclusiveOr:
                        return this.VisitBinary((BinaryExpression)exp);
                    case ExpressionType.TypeIs:
                        return this.VisitTypeIs((TypeBinaryExpression)exp);
                    case ExpressionType.Conditional:
                        return this.VisitConditional((ConditionalExpression)exp);
                    case ExpressionType.Constant:
                        return this.VisitConstant((ConstantExpression)exp);
                    case ExpressionType.Parameter:
                        return this.VisitParameter((ParameterExpression)exp);
                    case ExpressionType.MemberAccess:
                        return this.VisitMemberAccess((MemberExpression)exp);
                    case ExpressionType.Call:
                        return this.VisitMethodCall((MethodCallExpression)exp);
                    case ExpressionType.Lambda:
                        return this.VisitLambda((LambdaExpression)exp);
                    case ExpressionType.New:
                        return this.VisitNew((NewExpression)exp);
                    case ExpressionType.NewArrayInit:
                    case ExpressionType.NewArrayBounds:
                        return this.VisitNewArray((NewArrayExpression)exp);
                    case ExpressionType.Invoke:
                        return this.VisitInvocation((InvocationExpression)exp);
                    case ExpressionType.MemberInit:
                        return this.VisitMemberInit((MemberInitExpression)exp);
                    case ExpressionType.ListInit:
                        return this.VisitListInit((ListInitExpression)exp);
                    default:
                        throw new Exception(string.Format("Unhandled expression type: '{0}'", exp.NodeType));
                }
            }

            protected virtual MemberBinding VisitBinding(MemberBinding binding)
            {
                switch (binding.BindingType)
                {
                    case MemberBindingType.Assignment:
                        return this.VisitMemberAssignment((MemberAssignment)binding);
                    case MemberBindingType.MemberBinding:
                        return this.VisitMemberMemberBinding((MemberMemberBinding)binding);
                    case MemberBindingType.ListBinding:
                        return this.VisitMemberListBinding((MemberListBinding)binding);
                    default:
                        throw new Exception(string.Format("Unhandled binding type '{0}'", binding.BindingType));
                }
            }

            protected virtual ElementInit VisitElementInitializer(ElementInit initializer)
            {
                ReadOnlyCollection<Expression> arguments = this.VisitExpressionList(initializer.Arguments);
                if (arguments != initializer.Arguments)
                {
                    return Expression.ElementInit(initializer.AddMethod, arguments);
                }
                return initializer;
            }

            protected virtual Expression VisitUnary(UnaryExpression u)
            {
                Expression operand = this.Visit(u.Operand);
                if (operand != u.Operand)
                {
                    return Expression.MakeUnary(u.NodeType, operand, u.Type, u.Method);
                }
                return u;
            }

            protected virtual Expression VisitBinary(BinaryExpression b)
            {
                Expression left = this.Visit(b.Left);
                Expression right = this.Visit(b.Right);
                Expression conversion = this.Visit(b.Conversion);
                if (left != b.Left || right != b.Right || conversion != b.Conversion)
                {
                    if (b.NodeType == ExpressionType.Coalesce && b.Conversion != null)
                        return Expression.Coalesce(left, right, conversion as LambdaExpression);
                    else
                        return Expression.MakeBinary(b.NodeType, left, right, b.IsLiftedToNull, b.Method);
                }
                return b;
            }

            protected virtual Expression VisitTypeIs(TypeBinaryExpression b)
            {
                Expression expr = this.Visit(b.Expression);
                if (expr != b.Expression)
                {
                    return Expression.TypeIs(expr, b.TypeOperand);
                }
                return b;
            }

            protected virtual Expression VisitConstant(ConstantExpression c)
            {
                return c;
            }

            protected virtual Expression VisitConditional(ConditionalExpression c)
            {
                Expression test = this.Visit(c.Test);
                Expression ifTrue = this.Visit(c.IfTrue);
                Expression ifFalse = this.Visit(c.IfFalse);
                if (test != c.Test || ifTrue != c.IfTrue || ifFalse != c.IfFalse)
                {
                    return Expression.Condition(test, ifTrue, ifFalse);
                }
                return c;
            }

            protected virtual Expression VisitParameter(ParameterExpression p)
            {
                return p;
            }

            protected virtual Expression VisitMemberAccess(MemberExpression m)
            {
                Expression exp = this.Visit(m.Expression);
                if (exp != m.Expression)
                {
                    return Expression.MakeMemberAccess(exp, m.Member);
                }
                return m;
            }

            protected virtual Expression VisitMethodCall(MethodCallExpression m)
            {

                //MethodCallExpression mce = m;
                //if (mce.Method.Name == "Like")
                //    return string.Format("({0} like {1})", ExpressionRouter(mce.Arguments[0]), ExpressionRouter(mce.Arguments[1]));
                //else if (mce.Method.Name == "NotLike")
                //    return string.Format("({0} Not like {1})", ExpressionRouter(mce.Arguments[0]), ExpressionRouter(mce.Arguments[1]));
                //else if (mce.Method.Name == "In")
                //    return string.Format("{0} In ({1})", ExpressionRouter(mce.Arguments[0]), ExpressionRouter(mce.Arguments[1]));
                //else if (mce.Method.Name == "NotIn")
                //    return string.Format("{0} Not In ({1})", ExpressionRouter(mce.Arguments[0]), ExpressionRouter(mce.Arguments[1]));
                //MethodCallExpression mce = m;


                Expression obj = this.Visit(m.Object);
                IEnumerable<Expression> args = this.VisitExpressionList(m.Arguments);
                if (obj != m.Object || args != m.Arguments)
                {
                    return Expression.Call(obj, m.Method, args);
                }
                return m;
            }

            protected virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
            {
                List<Expression> list = null;
                for (int i = 0, n = original.Count; i < n; i++)
                {
                    Expression p = this.Visit(original[i]);
                    if (list != null)
                    {
                        list.Add(p);
                    }
                    else if (p != original[i])
                    {
                        list = new List<Expression>(n);
                        for (int j = 0; j < i; j++)
                        {
                            list.Add(original[j]);
                        }
                        list.Add(p);
                    }
                }
                if (list != null)
                {
                    return list.AsReadOnly();
                }
                return original;
            }

            protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
            {
                Expression e = this.Visit(assignment.Expression);
                if (e != assignment.Expression)
                {
                    return Expression.Bind(assignment.Member, e);
                }
                return assignment;
            }

            protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
            {
                IEnumerable<MemberBinding> bindings = this.VisitBindingList(binding.Bindings);
                if (bindings != binding.Bindings)
                {
                    return Expression.MemberBind(binding.Member, bindings);
                }
                return binding;
            }

            protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding binding)
            {
                IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(binding.Initializers);
                if (initializers != binding.Initializers)
                {
                    return Expression.ListBind(binding.Member, initializers);
                }
                return binding;
            }

            protected virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
            {
                List<MemberBinding> list = null;
                for (int i = 0, n = original.Count; i < n; i++)
                {
                    MemberBinding b = this.VisitBinding(original[i]);
                    if (list != null)
                    {
                        list.Add(b);
                    }
                    else if (b != original[i])
                    {
                        list = new List<MemberBinding>(n);
                        for (int j = 0; j < i; j++)
                        {
                            list.Add(original[j]);
                        }
                        list.Add(b);
                    }
                }
                if (list != null)
                    return list;
                return original;
            }

            protected virtual IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original)
            {
                List<ElementInit> list = null;
                for (int i = 0, n = original.Count; i < n; i++)
                {
                    ElementInit init = this.VisitElementInitializer(original[i]);
                    if (list != null)
                    {
                        list.Add(init);
                    }
                    else if (init != original[i])
                    {
                        list = new List<ElementInit>(n);
                        for (int j = 0; j < i; j++)
                        {
                            list.Add(original[j]);
                        }
                        list.Add(init);
                    }
                }
                if (list != null)
                    return list;
                return original;
            }

            protected virtual Expression VisitLambda(LambdaExpression lambda)
            {
                Expression body = this.Visit(lambda.Body);
                if (body != lambda.Body)
                {
                    return Expression.Lambda(lambda.Type, body, lambda.Parameters);
                }
                return lambda;
            }

            protected virtual NewExpression VisitNew(NewExpression nex)
            {
                IEnumerable<Expression> args = this.VisitExpressionList(nex.Arguments);
                if (args != nex.Arguments)
                {
                    if (nex.Members != null)
                        return Expression.New(nex.Constructor, args, nex.Members);
                    else
                        return Expression.New(nex.Constructor, args);
                }
                return nex;
            }

            protected virtual Expression VisitMemberInit(MemberInitExpression init)
            {
                NewExpression n = this.VisitNew(init.NewExpression);
                IEnumerable<MemberBinding> bindings = this.VisitBindingList(init.Bindings);
                if (n != init.NewExpression || bindings != init.Bindings)
                {
                    return Expression.MemberInit(n, bindings);
                }
                return init;
            }

            protected virtual Expression VisitListInit(ListInitExpression init)
            {
                NewExpression n = this.VisitNew(init.NewExpression);
                IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(init.Initializers);
                if (n != init.NewExpression || initializers != init.Initializers)
                {
                    return Expression.ListInit(n, initializers);
                }
                return init;
            }

            protected virtual Expression VisitNewArray(NewArrayExpression na)
            {
                IEnumerable<Expression> exprs = this.VisitExpressionList(na.Expressions);
                if (exprs != na.Expressions)
                {
                    if (na.NodeType == ExpressionType.NewArrayInit)
                    {
                        return Expression.NewArrayInit(na.Type.GetElementType(), exprs);
                    }
                    else
                    {
                        return Expression.NewArrayBounds(na.Type.GetElementType(), exprs);
                    }
                }
                return na;
            }

            protected virtual Expression VisitInvocation(InvocationExpression iv)
            {
                IEnumerable<Expression> args = this.VisitExpressionList(iv.Arguments);
                Expression expr = this.Visit(iv.Expression);
                if (args != iv.Arguments || expr != iv.Expression)
                {
                    return Expression.Invoke(expr, args);
                }
                return iv;
            }
        }
        public class PartialEvaluator : ExpressionVisitor
        {
            private Func<Expression, bool> m_fnCanBeEvaluated;
            private HashSet<Expression> m_candidates;

            public PartialEvaluator()
                : this(CanBeEvaluatedLocally)
            { }

            public PartialEvaluator(Func<Expression, bool> fnCanBeEvaluated)
            {
                this.m_fnCanBeEvaluated = fnCanBeEvaluated;
            }

            public Expression Eval(Expression exp)
            {
                this.m_candidates = new Nominator(this.m_fnCanBeEvaluated).Nominate(exp);

                return this.Visit(exp);
            }

            protected override Expression Visit(Expression exp)
            {
                if (exp == null)
                {
                    return null;
                }

                if (this.m_candidates.Contains(exp))
                {
                    return this.Evaluate(exp);
                }

                return base.Visit(exp);
            }

            private Expression Evaluate(Expression e)
            {
                if (e.NodeType == ExpressionType.Constant)
                {
                    return e;
                }

                LambdaExpression lambda = Expression.Lambda(e);
                Delegate fn = lambda.Compile();
                return Expression.Constant(fn.DynamicInvoke(null), e.Type);
            }

            private static bool CanBeEvaluatedLocally(Expression exp)
            {
                return exp.NodeType != ExpressionType.Parameter;
            }

            #region Nominator

            /// <summary>
            /// Performs bottom-up analysis to determine which nodes can possibly
            /// be part of an evaluated sub-tree.
            /// </summary>
            private class Nominator : ExpressionVisitor
            {
                private Func<Expression, bool> m_fnCanBeEvaluated;
                private HashSet<Expression> m_candidates;
                private bool m_cannotBeEvaluated;

                internal Nominator(Func<Expression, bool> fnCanBeEvaluated)
                {
                    this.m_fnCanBeEvaluated = fnCanBeEvaluated;
                }

                internal HashSet<Expression> Nominate(Expression expression)
                {
                    this.m_candidates = new HashSet<Expression>();
                    this.Visit(expression);
                    return this.m_candidates;
                }

                protected override Expression Visit(Expression expression)
                {
                    if (expression != null)
                    {
                        bool saveCannotBeEvaluated = this.m_cannotBeEvaluated;
                        this.m_cannotBeEvaluated = false;

                        base.Visit(expression);

                        if (!this.m_cannotBeEvaluated)
                        {
                            if (this.m_fnCanBeEvaluated(expression))
                            {
                                this.m_candidates.Add(expression);
                            }
                            else
                            {
                                this.m_cannotBeEvaluated = true;
                            }
                        }

                        this.m_cannotBeEvaluated |= saveCannotBeEvaluated;
                    }

                    return expression;
                }
            }

            #endregion
        }
        internal class ConditionBuilder : ExpressionVisitor
        {
            /// <summary>
            /// 数据库类型
            /// </summary>
            private DataBaseType m_dataBaseType = DataBaseType.NoSelect;
            /// <summary>
            /// 字段是否加引号
            /// </summary>
            private bool m_ifWithQuotationMarks = false;

            private List<object> m_arguments;
            private Stack<string> m_conditionParts;

            public string Condition { get; private set; }

            public object[] Arguments { get; private set; }




            #region 加双引号
            /// <summary>
            /// 加双引号
            /// </summary>
            /// <param name="str">字串</param>
            /// <returns></returns>
            public static string AddQuotationMarks(string str)
            {
                if (str != null)
                {
                    return "\"" + str.Trim() + "\"";
                }
                else
                {
                    return str;
                }
            }

            #endregion

            #region 设置是否加双引号
            public void SetIfWithQuotationMarks(bool ifwith)
            {
                m_ifWithQuotationMarks = ifwith;
            }
            #endregion

            #region 设置是数据库类型
            public void SetDataBaseType(DataBaseType databaseType)
            {
                if (databaseType != DataBaseType.NoSelect)
                {
                    m_dataBaseType = databaseType;
                }



            }
            #endregion
            public void Build(Expression expression)
            {
                PartialEvaluator evaluator = new PartialEvaluator();
                Expression evaluatedExpression = evaluator.Eval(expression);

                this.m_arguments = new List<object>();
                this.m_conditionParts = new Stack<string>();

                this.Visit(evaluatedExpression);

                this.Arguments = this.m_arguments.ToArray();
                this.Condition = this.m_conditionParts.Count > 0 ? this.m_conditionParts.Pop() : null;
            }

            protected override Expression VisitBinary(BinaryExpression b)
            {
                if (b == null) return b;

                string opr;
                switch (b.NodeType)
                {
                    case ExpressionType.Equal:
                        opr = "=";
                        break;
                    case ExpressionType.NotEqual:
                        opr = "<>";
                        break;
                    case ExpressionType.GreaterThan:
                        opr = ">";
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        opr = ">=";
                        break;
                    case ExpressionType.LessThan:
                        opr = "<";
                        break;
                    case ExpressionType.LessThanOrEqual:
                        opr = "<=";
                        break;
                    case ExpressionType.AndAlso:
                        opr = "AND";
                        break;
                    case ExpressionType.OrElse:
                        opr = "OR";
                        break;
                    case ExpressionType.Add:
                        opr = "+";
                        break;
                    case ExpressionType.Subtract:
                        opr = "-";
                        break;
                    case ExpressionType.Multiply:
                        opr = "*";
                        break;
                    case ExpressionType.Divide:
                        opr = "/";
                        break;
                    default:
                        throw new NotSupportedException(b.NodeType + "is not supported.");
                }

                this.Visit(b.Left);
                this.Visit(b.Right);

                string right = this.m_conditionParts.Pop();
                string left = this.m_conditionParts.Pop();

                string condition = String.Format("({0} {1} {2})", left, opr, right);
                this.m_conditionParts.Push(condition);

                return b;
            }

            protected override Expression VisitConstant(ConstantExpression c)
            {
                if (c == null) return c;

                this.m_arguments.Add(c.Value);
                this.m_conditionParts.Push(String.Format("{{{0}}}", this.m_arguments.Count - 1));

                return c;
            }

            protected override Expression VisitMemberAccess(MemberExpression m)
            {
                if (m == null) return m;

                PropertyInfo propertyInfo = m.Member as PropertyInfo;
                if (propertyInfo == null) return m;

                //   this.m_conditionParts.Push(String.Format("(Where.{0})", propertyInfo.Name));
                //是否添加引号
                if (m_ifWithQuotationMarks)
                {
                    this.m_conditionParts.Push(String.Format(" {0} ", AddQuotationMarks(propertyInfo.Name)));
                }
                else
                {
                    // this.m_conditionParts.Push(String.Format("[{0}]", propertyInfo.Name));
                    this.m_conditionParts.Push(String.Format(" {0} ", propertyInfo.Name));
                }

                return m;
            }

            #region 其他
            static string BinarExpressionProvider(Expression left, Expression right, ExpressionType type)
            {
                string sb = "(";
                //先处理左边
                sb += ExpressionRouter(left);

                sb += ExpressionTypeCast(type);

                //再处理右边
                string tmpStr = ExpressionRouter(right);

                if (tmpStr == "null")
                {
                    if (sb.EndsWith(" ="))
                        sb = sb.Substring(0, sb.Length - 1) + " is null";
                    else if (sb.EndsWith("<>"))
                        sb = sb.Substring(0, sb.Length - 1) + " is not null";
                }
                else
                    sb += tmpStr;
                return sb += ")";
            }

            static string ExpressionRouter(Expression exp)
            {
                string sb = string.Empty;
                if (exp is BinaryExpression)
                {

                    BinaryExpression be = ((BinaryExpression)exp);
                    return BinarExpressionProvider(be.Left, be.Right, be.NodeType);
                }
                else if (exp is MemberExpression)
                {

                    MemberExpression me = ((MemberExpression)exp);
                    return me.Member.Name;
                }
                else if (exp is NewArrayExpression)
                {
                    NewArrayExpression ae = ((NewArrayExpression)exp);
                    StringBuilder tmpstr = new StringBuilder();
                    foreach (Expression ex in ae.Expressions)
                    {
                        tmpstr.Append(ExpressionRouter(ex));
                        tmpstr.Append(",");
                    }
                    return tmpstr.ToString(0, tmpstr.Length - 1);
                }
                else if (exp is MethodCallExpression)
                {
                    MethodCallExpression mce = (MethodCallExpression)exp;
                    if (mce.Method.Name == "Like")
                        return string.Format("({0} like {1})", ExpressionRouter(mce.Arguments[0]), ExpressionRouter(mce.Arguments[1]));
                    else if (mce.Method.Name == "NotLike")
                        return string.Format("({0} Not like {1})", ExpressionRouter(mce.Arguments[0]), ExpressionRouter(mce.Arguments[1]));
                    else if (mce.Method.Name == "In")
                        return string.Format("{0} In ({1})", ExpressionRouter(mce.Arguments[0]), ExpressionRouter(mce.Arguments[1]));
                    else if (mce.Method.Name == "NotIn")
                        return string.Format("{0} Not In ({1})", ExpressionRouter(mce.Arguments[0]), ExpressionRouter(mce.Arguments[1]));
                    else if (mce.Method.Name == "StartWith")
                        return string.Format("{0} like '{1}%'", ExpressionRouter(mce.Arguments[0]), ExpressionRouter(mce.Arguments[1]));
                }
                else if (exp is ConstantExpression)
                {
                    ConstantExpression ce = ((ConstantExpression)exp);
                    if (ce.Value == null)
                        return "null";
                    else if (ce.Value is ValueType)
                        return ce.Value.ToString();
                    else if (ce.Value is string || ce.Value is DateTime || ce.Value is char)
                    {

                        return string.Format("'{0}'", ce.Value.ToString());
                    }
                }
                else if (exp is UnaryExpression)
                {
                    UnaryExpression ue = ((UnaryExpression)exp);
                    return ExpressionRouter(ue.Operand);
                }
                return null;
            }

            static string ExpressionTypeCast(ExpressionType type)
            {
                switch (type)
                {
                    case ExpressionType.And:
                    case ExpressionType.AndAlso:
                        return " AND ";
                    case ExpressionType.Equal:
                        return " =";
                    case ExpressionType.GreaterThan:
                        return " >";
                    case ExpressionType.GreaterThanOrEqual:
                        return ">=";
                    case ExpressionType.LessThan:
                        return "<";
                    case ExpressionType.LessThanOrEqual:
                        return "<=";
                    case ExpressionType.NotEqual:
                        return "<>";
                    case ExpressionType.Or:
                    case ExpressionType.OrElse:
                        return " Or ";
                    case ExpressionType.Add:
                    case ExpressionType.AddChecked:
                        return "+";
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractChecked:
                        return "-";
                    case ExpressionType.Divide:
                        return "/";
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyChecked:
                        return "*";
                    default:
                        return null;
                }
            }
            #endregion


            /// <summary>
            /// ConditionBuilder 并不支持生成Like操作，如 字符串的 StartsWith，Contains，EndsWith 并不能生成这样的SQL： Like ‘xxx%’, Like ‘%xxx%’ , Like ‘%xxx’ . 只要override VisitMethodCall 这个方法即可实现上述功能。
            /// </summary>
            /// <param name="m"></param>
            /// <returns></returns>
            protected override Expression VisitMethodCall(MethodCallExpression m)
            {
                string connectorWords = GetLikeConnectorWords(m_dataBaseType); //获取like链接符

                if (m == null) return m;
                string format;
                switch (m.Method.Name)
                {
                    case "StartsWith":
                        format = "({0} LIKE ''" + connectorWords + "{1}" + connectorWords + "'%')";
                        break;
                    case "Contains":
                        format = "({0} LIKE '%'" + connectorWords + "{1}" + connectorWords + "'%')";
                        break;
                    case "EndsWith":
                        format = "({0} LIKE '%'" + connectorWords + "{1}" + connectorWords + "'')";
                        break;

                    case "Equals":
                        // not in 或者  in 或 not like
                        format = "({0} {1} )";
                        break;

                    default:
                        throw new NotSupportedException(m.NodeType + " is not supported!");
                }
                this.Visit(m.Object);
                this.Visit(m.Arguments[0]);
                string right = this.m_conditionParts.Pop();
                string left = this.m_conditionParts.Pop();

                this.m_conditionParts.Push(String.Format(format, left, right));
                return m;
            }

            /// <summary>
            /// 获得like语句链接符
            /// </summary>
            /// <param name="databaseType"></param>
            /// <returns></returns>
            public static string GetLikeConnectorWords(DataBaseType databaseType)
            {
                string result = "+";
                switch (databaseType)
                {
                    case DataBaseType.PostgreSQL:
                    case DataBaseType.Oracle:
                    case DataBaseType.MySql:
                    case DataBaseType.SQLite:
                        result = "||";
                        break;
                }

                return result;
            }

        }
    }



    /// <summary>
    /// 从Expression<Func<IQueryable<T>, IQueryable<T>>> exp 中得到where sql和order by sql
    /// </summary>
    /// <typeparam name="T">查询类</typeparam>
    internal sealed class ExpressQueryable<T>
    {

        #region 外部访问方法

        #endregion




        public class AiExpConditions
        {

            #region 混合语句增加操作

            /// <summary>
            /// 获取 Where 条件语句
            /// </summary>
            /// <param name="AddCinditionKey">是否加Where词</param>
            /// <returns>Where条件语句</returns>
            public string GetWhere(Expression<Func<IQueryable<T>, IQueryable<T>>> exp, bool AddCinditionKey = false)
            {
                if (string.IsNullOrWhiteSpace(_aiWhereStr)) return string.Empty;

                if (AddCinditionKey)
                {
                    return " Where " + _aiWhereStr;
                }
                else
                {
                    return _aiWhereStr;
                }
            }

            /// <summary>
            /// 获取 OrderBy 条件语句
            /// </summary>
            /// <param name="AddCinditionKey">是否加Order By词</param>
            /// <returns>OrderBy 条件语句</returns>
            public string GetOrderBy(Expression<Func<IQueryable<T>, IQueryable<T>>> exp, bool AddCinditionKey = false)
            {
                if (string.IsNullOrWhiteSpace(_aiOrderByStr)) return string.Empty;

                if (AddCinditionKey)
                {
                    return " Order By " + _aiOrderByStr;
                }
                else
                {
                    return _aiOrderByStr;
                }

            }


            /// <summary>
            /// 添加一个条件语句（Where/OrderBy）
            /// </summary>
            /// <param name="exp">条件表达示</param>
            public void SetCondition(Expression<Func<IQueryable<T>, IQueryable<T>>> exp)
            {
                SetConditionStr(exp, AiExpUnion.And);
            }

            /// <summary>
            /// 当给定条件满足时,添加一个条件语句（Where/OrderBy）
            /// </summary>
            /// <param name="aiCondition">当给定条件满足时</param>
            /// <param name="exp">条件表达示</param>
            public void SetCondition(bool aiCondition, Expression<Func<IQueryable<T>, IQueryable<T>>> exp)
            {
                if (aiCondition)
                {
                    SetConditionStr(exp, AiExpUnion.And);
                }
            }

            /// <summary>
            /// 当给定lambda表达式条件满足时,添加一个条件语句（Where/OrderBy）
            /// </summary>
            /// <param name="aiCondition">给定lambda表达式条件</param>
            /// <param name="exp">条件表达示</param>
            public void SetCondition(Func<bool> aiCondition, Expression<Func<IQueryable<T>, IQueryable<T>>> exp)
            {
                SetCondition(aiCondition(), exp);
            }

            /// <summary>
            /// 如果条件满足时,将添加前一个条件语句（Where/OrderBy），否则添加后一个
            /// </summary>
            /// <param name="aiCondition">条件</param>
            /// <param name="aiExpWhenTrue">条件为真时</param>
            /// <param name="aiExpWhenFalse">条件为假时</param>
            public void SetCondition(bool aiCondition, Expression<Func<IQueryable<T>, IQueryable<T>>> aiExpWhenTrue, Expression<Func<IQueryable<T>, IQueryable<T>>> aiExpWhenFalse)
            {
                if (aiCondition)
                {
                    SetConditionStr(aiExpWhenTrue, AiExpUnion.And);
                }
                else
                {
                    SetConditionStr(aiExpWhenFalse, AiExpUnion.And);
                }
            }

            /// <summary>
            /// 如果条件满足时,将添加前一个条件语句（Where/OrderBy），否则添加后一个
            /// </summary>
            /// <param name="aiCondition">条件</param>
            /// <param name="aiExpWhenTrue">条件为真时</param>
            /// <param name="aiExpWhenFalse">条件为假时</param>
            public void SetCondition(Func<bool> aiCondition, Expression<Func<IQueryable<T>, IQueryable<T>>> aiExpWhenTrue, Expression<Func<IQueryable<T>, IQueryable<T>>> aiExpWhenFalse)
            {
                SetCondition(aiCondition(), aiExpWhenTrue, aiExpWhenFalse);
            }

            #endregion

            #region 以 And 相联接 Where条件语句
            /// <summary>
            /// 添加一个Where条件语句，如果语句存在，则以 And 相联接
            /// </summary>
            /// <param name="exp">Where条件表达示</param>
            public void AddAndWhere(Expression<Func<T, bool>> exp)
            {
                SetOneConditionStr(exp, AiExpUnion.And);
            }

            /// <summary>
            /// 当给定条件满足时,添加一个Where条件语句，如果语句存在，则以 And 相联接
            /// </summary>
            /// <param name="aiCondition">给定条件</param>
            /// <param name="exp">Where条件表达示</param>
            public void AddAndWhere(bool aiCondition, Expression<Func<T, bool>> exp)
            {
                if (aiCondition)
                {
                    SetOneConditionStr(exp, AiExpUnion.And);
                }

            }

            /// <summary>
            /// 当给定lambda表达式条件满足时,添加一个Where条件语句，如果语句存在，则以 And 相联接
            /// </summary>
            /// <param name="aiCondition">给定lambda表达式条件</param>
            /// <param name="exp"></param>
            public void AddAndWhere(Func<bool> aiCondition, Expression<Func<T, bool>> exp)
            {
                AddAndWhere(aiCondition(), exp);
            }

            /// <summary>
            /// 如果条件满足时,将添加前一个条件语句（Where），否则添加后一个,以 And 相联接
            /// </summary>
            /// <param name="aiCondition">条件</param>
            /// <param name="aiExpWhenTrue">条件为真时</param>
            /// <param name="aiExpWhenFalse">条件为假时</param>
            public void AddAndWhere(bool aiCondition, Expression<Func<T, bool>> aiExpWhenTrue, Expression<Func<T, bool>> aiExpWhenFalse)
            {
                if (aiCondition)
                {
                    SetOneConditionStr(aiExpWhenTrue, AiExpUnion.And);
                }
                else
                {
                    SetOneConditionStr(aiExpWhenFalse, AiExpUnion.And);
                }

            }

            /// <summary>
            /// 如果条件满足时,将添加前一个条件语句（Where），否则添加后一个,以 And 相联接
            /// </summary>
            /// <param name="aiCondition">Lambda条件</param>
            /// <param name="aiExpWhenTrue">条件为真时</param>
            /// <param name="aiExpWhenFalse">条件为假时</param>
            public void AddAndWhere(Func<bool> aiCondition, Expression<Func<T, bool>> aiExpWhenTrue, Expression<Func<T, bool>> aiExpWhenFalse)
            {
                AddAndWhere(aiCondition(), aiExpWhenTrue, aiExpWhenFalse);
            }

            #endregion

            #region 以 Or 相联接 Where条件语句
            /// <summary>
            /// 添加一个Where条件语句，如果语句存在，则以 Or 相联接
            /// </summary>
            /// <param name="exp">Where条件表达示</param>
            public void AddOrWhere(Expression<Func<T, bool>> exp)
            {

                SetOneConditionStr(exp, AiExpUnion.Or);
            }

            /// <summary>
            /// 当给定条件满足时,添加一个Where条件语句，如果语句存在，则以 Or 相联接
            /// </summary>
            /// <param name="aiCondition">给定条件</param>
            /// <param name="exp">Where条件表达示</param>
            public void AddOrWhere(bool aiCondition, Expression<Func<T, bool>> exp)
            {
                if (aiCondition)
                {
                    SetOneConditionStr(exp, AiExpUnion.Or);
                }

            }

            /// <summary>
            /// 当给定lambda表达式条件满足时,添加一个Where条件语句，如果语句存在，则以 Or 相联接
            /// </summary>
            /// <param name="aiCondition">给定lambda表达式条件</param>
            /// <param name="exp"></param>
            public void AddOrWhere(Func<bool> aiCondition, Expression<Func<T, bool>> exp)
            {
                AddOrWhere(aiCondition(), exp);
            }

            /// <summary>
            /// 如果条件满足时,将添加前一个条件语句（Where），否则添加后一个,以 Or 相联接
            /// </summary>
            /// <param name="aiCondition">条件</param>
            /// <param name="aiExpWhenTrue">条件为真时</param>
            /// <param name="aiExpWhenFalse">条件为假时</param>
            public void AddOrWhere(bool aiCondition, Expression<Func<T, bool>> aiExpWhenTrue, Expression<Func<T, bool>> aiExpWhenFalse)
            {
                if (aiCondition)
                {
                    SetOneConditionStr(aiExpWhenTrue, AiExpUnion.Or);
                }
                else
                {
                    SetOneConditionStr(aiExpWhenFalse, AiExpUnion.Or);
                }

            }

            /// <summary>
            /// 如果条件满足时,将添加前一个条件语句（Where），否则添加后一个,以 Or 相联接
            /// </summary>
            /// <param name="aiCondition">Lambda条件</param>
            /// <param name="aiExpWhenTrue">条件为真时</param>
            /// <param name="aiExpWhenFalse">条件为假时</param>
            public void AddOrWhere(Func<bool> aiCondition, Expression<Func<T, bool>> aiExpWhenTrue, Expression<Func<T, bool>> aiExpWhenFalse)
            {
                AddOrWhere(aiCondition(), aiExpWhenTrue, aiExpWhenFalse);
            }

            #endregion

            #region  OrderBy语句

            /// <summary>
            /// 添加一个OrderBy语句
            /// </summary>
            /// <typeparam name="D">OrderBy的字段数据类型</typeparam>
            /// <param name="exp">OrderBy条件表达示</param>
            public void AddOrderBy<D>(Expression<Func<T, D>> exp)
            {
                SetOneConditionStr(exp, AiExpUnion.OrderBy);
            }

            /// <summary>
            /// 如果条件满足时,添加一个OrderBy语句
            /// </summary>
            /// <typeparam name="D">OrderBy的字段数据类型</typeparam>
            /// <param name="aiCondition">条件</param>
            /// <param name="exp">OrderBy条件表达示</param>
            public void AddOrderBy<D>(bool aiCondition, Expression<Func<T, D>> exp)
            {
                if (aiCondition)
                {
                    SetOneConditionStr(exp, AiExpUnion.OrderBy);
                }

            }

            /// <summary>
            /// 如果条件满足时,添加一个OrderBy语句
            /// </summary>
            /// <typeparam name="D">OrderBy的数据字段类型</typeparam>
            /// <param name="aiCondition">Lambda条件</param>
            /// <param name="exp">OrderBy条件表达示</param>
            public void AddOrderBy<D>(Func<bool> aiCondition, Expression<Func<T, D>> exp)
            {
                AddOrderBy<D>(aiCondition(), exp);
            }

            /// <summary>
            /// 如果条件满足时,将添加前一个OrderBy语句，否则添加后一个
            /// </summary>
            /// <typeparam name="D">OrderBy的数据字段类型</typeparam>
            /// <param name="aiCondition">条件</param>
            /// <param name="aiExpWhenTrue">条件为真时</param>
            /// <param name="aiExpWhenFalse">条件为假时</param>
            public void AddOrderBy<D>(bool aiCondition, Expression<Func<T, D>> aiExpWhenTrue, Expression<Func<T, D>> aiExpWhenFalse)
            {
                if (aiCondition)
                {
                    SetOneConditionStr(aiExpWhenTrue, AiExpUnion.OrderBy);
                }
                else
                {
                    SetOneConditionStr(aiExpWhenFalse, AiExpUnion.OrderBy);
                }

            }

            /// <summary>
            /// 如果条件满足时,将添加前一个OrderBy语句，否则添加后一个
            /// </summary>
            /// <typeparam name="D">OrderBy的数据字段类型</typeparam>
            /// <param name="aiCondition">Lambda条件</param>
            /// <param name="aiExpWhenTrue">条件为真时</param>
            /// <param name="aiExpWhenFalse">条件为假时</param>
            public void AddOrderBy<D>(Func<bool> aiCondition, Expression<Func<T, D>> aiExpWhenTrue, Expression<Func<T, D>> aiExpWhenFalse)
            {
                AddOrderBy<D>(aiCondition(), aiExpWhenTrue, aiExpWhenFalse);
            }


            #endregion

            #region 内部操作

            private string _aiWhereStr = string.Empty;

            private string _aiOrderByStr = string.Empty;

            private void SetConditionStr(Expression exp, AiExpUnion aiUion = AiExpUnion.And)
            {
                SetWhere(exp);//Where条件句


                SetOrderBy(exp);//Order by 语句
            }

            private void SetOneConditionStr(Expression exp, AiExpUnion bizUion = AiExpUnion.And)
            {

                if ((bizUion == AiExpUnion.And) || (bizUion == AiExpUnion.Or))
                {
                    SetWhere(exp);//Where条件句
                }
                else if (bizUion == AiExpUnion.OrderBy)
                {
                    SetOrderBy(exp);//Order by 语句
                }
            }

            private void SetOrderBy(Expression exp)
            {
                var itemstr = AiExpressionWriterSql.BizWhereWriteToString(exp, AiExpSqlType.aiOrder);
                if (string.IsNullOrWhiteSpace(_aiOrderByStr))
                {
                    _aiOrderByStr = itemstr;
                }
                else
                {

                    _aiOrderByStr = _aiOrderByStr + "," + itemstr;

                }
            }

            private void SetWhere(Expression exp, AiExpUnion bizUion = AiExpUnion.And)
            {
                var itemstr = AiExpressionWriterSql.BizWhereWriteToString(exp, AiExpSqlType.aiWhere);
                if (string.IsNullOrWhiteSpace(_aiWhereStr))
                {
                    _aiWhereStr = itemstr;
                }
                else
                {
                    if (bizUion == AiExpUnion.Or)
                    {
                        _aiWhereStr = _aiWhereStr + " Or " + itemstr;
                    }
                    else
                    {
                        _aiWhereStr = _aiWhereStr + " And " + itemstr;
                    }
                }
            }
            #endregion

            internal enum AiExpUnion : byte
            {
                And,
                Or,
                OrderBy
            }
        }


        internal abstract class AiExpressionVisitor
        {
            protected AiExpressionVisitor() { }

            protected virtual Expression Visit(Expression exp)
            {
                if (exp == null)
                    return exp;
                switch (exp.NodeType)
                {
                    case ExpressionType.Negate:
                    case ExpressionType.NegateChecked:
                    case ExpressionType.Not:
                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked:
                    case ExpressionType.ArrayLength:
                    case ExpressionType.Quote:
                    case ExpressionType.TypeAs:
                        return this.VisitUnary((UnaryExpression)exp);
                    case ExpressionType.Add:
                    case ExpressionType.AddChecked:
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractChecked:
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyChecked:
                    case ExpressionType.Divide:
                    case ExpressionType.Modulo:
                    case ExpressionType.And:
                    case ExpressionType.AndAlso:
                    case ExpressionType.Or:
                    case ExpressionType.OrElse:
                    case ExpressionType.LessThan:
                    case ExpressionType.LessThanOrEqual:
                    case ExpressionType.GreaterThan:
                    case ExpressionType.GreaterThanOrEqual:
                    case ExpressionType.Equal:
                    case ExpressionType.NotEqual:
                    case ExpressionType.Coalesce:
                    case ExpressionType.ArrayIndex:
                    case ExpressionType.RightShift:
                    case ExpressionType.LeftShift:
                    case ExpressionType.ExclusiveOr:
                        return this.VisitBinary((BinaryExpression)exp);
                    case ExpressionType.TypeIs:
                        return this.VisitTypeIs((TypeBinaryExpression)exp);
                    case ExpressionType.Conditional:
                        return this.VisitConditional((ConditionalExpression)exp);
                    case ExpressionType.Constant:
                        return this.VisitConstant((ConstantExpression)exp);
                    case ExpressionType.Parameter:
                        return this.VisitParameter((ParameterExpression)exp);
                    case ExpressionType.MemberAccess:
                        return this.VisitMemberAccess((MemberExpression)exp);
                    case ExpressionType.Call:
                        return this.VisitMethodCall((MethodCallExpression)exp);
                    case ExpressionType.Lambda:
                        return this.VisitLambda((LambdaExpression)exp);
                    case ExpressionType.New:
                        return this.VisitNew((NewExpression)exp);
                    case ExpressionType.NewArrayInit:
                    case ExpressionType.NewArrayBounds:
                        return this.VisitNewArray((NewArrayExpression)exp);
                    case ExpressionType.Invoke:
                        return this.VisitInvocation((InvocationExpression)exp);
                    case ExpressionType.MemberInit:
                        return this.VisitMemberInit((MemberInitExpression)exp);
                    case ExpressionType.ListInit:
                        return this.VisitListInit((ListInitExpression)exp);
                    default:
                        throw new Exception(string.Format("Unhandled expression type: '{0}'", exp.NodeType));
                }
            }
            protected virtual Expression VisitUnknown(Expression expression)
            {
                throw new Exception(string.Format("Unhandled expression type: '{0}'", expression.NodeType));
            }

            protected virtual MemberBinding VisitBinding(MemberBinding binding)
            {
                switch (binding.BindingType)
                {
                    case MemberBindingType.Assignment:
                        return this.VisitMemberAssignment((MemberAssignment)binding);
                    case MemberBindingType.MemberBinding:
                        return this.VisitMemberMemberBinding((MemberMemberBinding)binding);
                    case MemberBindingType.ListBinding:
                        return this.VisitMemberListBinding((MemberListBinding)binding);
                    default:
                        throw new Exception(string.Format("Unhandled binding type '{0}'", binding.BindingType));
                }
            }

            protected virtual ElementInit VisitElementInitializer(ElementInit initializer)
            {
                ReadOnlyCollection<Expression> arguments = this.VisitExpressionList(initializer.Arguments);
                if (arguments != initializer.Arguments)
                {
                    return Expression.ElementInit(initializer.AddMethod, arguments);
                }
                return initializer;
            }

            protected virtual Expression VisitUnary(UnaryExpression u)
            {
                Expression operand = this.Visit(u.Operand);
                if (operand != u.Operand)
                {
                    return Expression.MakeUnary(u.NodeType, operand, u.Type, u.Method);
                }
                return u;
            }

            protected virtual Expression VisitBinary(BinaryExpression b)
            {
                Expression left = this.Visit(b.Left);
                Expression right = this.Visit(b.Right);
                Expression conversion = this.Visit(b.Conversion);
                if (left != b.Left || right != b.Right || conversion != b.Conversion)
                {
                    if (b.NodeType == ExpressionType.Coalesce && b.Conversion != null)
                        return Expression.Coalesce(left, right, conversion as LambdaExpression);
                    else
                        return Expression.MakeBinary(b.NodeType, left, right, b.IsLiftedToNull, b.Method);
                }
                return b;
            }

            protected virtual Expression VisitTypeIs(TypeBinaryExpression b)
            {
                Expression expr = this.Visit(b.Expression);
                if (expr != b.Expression)
                {
                    return Expression.TypeIs(expr, b.TypeOperand);
                }
                return b;
            }

            protected virtual Expression VisitConstant(ConstantExpression c)
            {
                return c;
            }

            protected virtual Expression VisitConditional(ConditionalExpression c)
            {
                Expression test = this.Visit(c.Test);
                Expression ifTrue = this.Visit(c.IfTrue);
                Expression ifFalse = this.Visit(c.IfFalse);
                if (test != c.Test || ifTrue != c.IfTrue || ifFalse != c.IfFalse)
                {
                    return Expression.Condition(test, ifTrue, ifFalse);
                }
                return c;
            }

            protected virtual Expression VisitParameter(ParameterExpression p)
            {
                return p;
            }

            protected virtual Expression VisitMemberAccess(MemberExpression m)
            {
                Expression exp = this.Visit(m.Expression);
                if (exp != m.Expression)
                {
                    return Expression.MakeMemberAccess(exp, m.Member);
                }
                return m;
            }

            protected virtual Expression VisitMethodCall(MethodCallExpression m)
            {
                Expression obj = this.Visit(m.Object);
                IEnumerable<Expression> args = this.VisitExpressionList(m.Arguments);
                if (obj != m.Object || args != m.Arguments)
                {
                    return Expression.Call(obj, m.Method, args);
                }
                return m;
            }

            protected virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
            {
                List<Expression> list = null;
                for (int i = 0, n = original.Count; i < n; i++)
                {
                    Expression p = this.Visit(original[i]);
                    if (list != null)
                    {
                        list.Add(p);
                    }
                    else if (p != original[i])
                    {
                        list = new List<Expression>(n);
                        for (int j = 0; j < i; j++)
                        {
                            list.Add(original[j]);
                        }
                        list.Add(p);
                    }
                }
                if (list != null)
                {
                    return list.AsReadOnly();
                }
                return original;
            }

            protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
            {
                Expression e = this.Visit(assignment.Expression);
                if (e != assignment.Expression)
                {
                    return Expression.Bind(assignment.Member, e);
                }
                return assignment;
            }

            protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
            {
                IEnumerable<MemberBinding> bindings = this.VisitBindingList(binding.Bindings);
                if (bindings != binding.Bindings)
                {
                    return Expression.MemberBind(binding.Member, bindings);
                }
                return binding;
            }

            protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding binding)
            {
                IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(binding.Initializers);
                if (initializers != binding.Initializers)
                {
                    return Expression.ListBind(binding.Member, initializers);
                }
                return binding;
            }

            protected virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
            {
                List<MemberBinding> list = null;
                for (int i = 0, n = original.Count; i < n; i++)
                {
                    MemberBinding b = this.VisitBinding(original[i]);
                    if (list != null)
                    {
                        list.Add(b);
                    }
                    else if (b != original[i])
                    {
                        list = new List<MemberBinding>(n);
                        for (int j = 0; j < i; j++)
                        {
                            list.Add(original[j]);
                        }
                        list.Add(b);
                    }
                }
                if (list != null)
                    return list;
                return original;
            }

            protected virtual IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original)
            {
                List<ElementInit> list = null;
                for (int i = 0, n = original.Count; i < n; i++)
                {
                    ElementInit init = this.VisitElementInitializer(original[i]);
                    if (list != null)
                    {
                        list.Add(init);
                    }
                    else if (init != original[i])
                    {
                        list = new List<ElementInit>(n);
                        for (int j = 0; j < i; j++)
                        {
                            list.Add(original[j]);
                        }
                        list.Add(init);
                    }
                }
                if (list != null)
                    return list;
                return original;
            }

            protected virtual Expression VisitLambda(LambdaExpression lambda)
            {
                Expression body = this.Visit(lambda.Body);
                if (body != lambda.Body)
                {
                    return Expression.Lambda(lambda.Type, body, lambda.Parameters);
                }
                return lambda;
            }

            protected virtual NewExpression VisitNew(NewExpression nex)
            {
                IEnumerable<Expression> args = this.VisitExpressionList(nex.Arguments);
                if (args != nex.Arguments)
                {
                    if (nex.Members != null)
                        return Expression.New(nex.Constructor, args, nex.Members);
                    else
                        return Expression.New(nex.Constructor, args);
                }
                return nex;
            }

            protected virtual Expression VisitMemberInit(MemberInitExpression init)
            {
                NewExpression n = this.VisitNew(init.NewExpression);
                IEnumerable<MemberBinding> bindings = this.VisitBindingList(init.Bindings);
                if (n != init.NewExpression || bindings != init.Bindings)
                {
                    return Expression.MemberInit(n, bindings);
                }
                return init;
            }

            protected virtual Expression VisitListInit(ListInitExpression init)
            {
                NewExpression n = this.VisitNew(init.NewExpression);
                IEnumerable<ElementInit> initializers = this.VisitElementInitializerList(init.Initializers);
                if (n != init.NewExpression || initializers != init.Initializers)
                {
                    return Expression.ListInit(n, initializers);
                }
                return init;
            }

            protected virtual Expression VisitNewArray(NewArrayExpression na)
            {
                IEnumerable<Expression> exprs = this.VisitExpressionList(na.Expressions);
                if (exprs != na.Expressions)
                {
                    if (na.NodeType == ExpressionType.NewArrayInit)
                    {
                        return Expression.NewArrayInit(na.Type.GetElementType(), exprs);
                    }
                    else
                    {
                        return Expression.NewArrayBounds(na.Type.GetElementType(), exprs);
                    }
                }
                return na;
            }

            protected virtual Expression VisitInvocation(InvocationExpression iv)
            {
                IEnumerable<Expression> args = this.VisitExpressionList(iv.Arguments);
                Expression expr = this.Visit(iv.Expression);
                if (args != iv.Arguments || expr != iv.Expression)
                {
                    return Expression.Invoke(expr, args);
                }
                return iv;
            }
        }




        internal enum AiExpSqlType : byte
        {
            aiWhere,
            aiOrder
        }
        /// <summary>
        /// 输出一个基于C#分析器的表达示树
        /// </summary>
        internal class AiExpressionWriterSql : AiExpressionVisitor
        {
            TextWriter writer;
            int indent = 2;
            int depth = 0;
            string aiWhereResult = string.Empty;
            string aiOrdeRsult = string.Empty;
            int aiOrderTime = 0;

            public int AiOrderTime
            {
                get { return aiOrderTime; }
                set
                {

                    aiOrderTime = value;
                }
            }
            int bizWhereTime = 0;

            public int AiWhereTime
            {
                get { return bizWhereTime; }
                set
                {

                    bizWhereTime = value;
                }
            }
            AiExpSqlType bizRead = AiExpSqlType.aiWhere;

            protected AiExpressionWriterSql(TextWriter writer)
            {
                this.writer = writer;
            }

            private static void Write(TextWriter writer, Expression expression)
            {
                new AiExpressionWriterSql(writer).Visit(expression);
            }

            private static string Write(TextWriter writer, Expression expression, AiExpSqlType bizSql)
            {
                expression = AiPartialEvaluator.Eval(expression);
                var bizR = new AiExpressionWriterSql(writer);
                bizR.bizRead = bizSql;
                bizR.Visit(expression);
                string result = string.Empty;
                switch (bizSql)
                {
                    case AiExpSqlType.aiOrder:
                        result = Regex.Replace(bizR.aiOrdeRsult, @",\s?$", "");
                        return result;

                    case AiExpSqlType.aiWhere:
                        result = Regex.Replace(bizR.aiWhereResult, @"and\s?$", "");
                        return result; ;
                    default: return string.Empty;
                }

            }

            private static string WriteToString(Expression expression)
            {
                StringWriter sw = new StringWriter();
                Write(sw, expression);

                return sw.ToString();
            }

            public static string BizWhereWriteToString(Expression expression, AiExpSqlType bizSql)
            {
                StringWriter sw = new StringWriter();
                return Write(sw, expression, bizSql);
            }

            protected enum Indentation
            {
                Same,
                Inner,
                Outer
            }

            protected int IndentationWidth
            {
                get { return this.indent; }
                set { this.indent = value; }
            }

            protected void WriteLine(Indentation style)
            {
                this.writer.WriteLine();
                //  this.Indent(style);

            }

            private static readonly char[] splitters = new char[] { '\n', '\r' };

            protected void Write(string text)
            {
                switch (bizRead)
                {
                    case AiExpSqlType.aiOrder: aiOrdeRsult = aiOrdeRsult + text; break;
                    case AiExpSqlType.aiWhere: aiWhereResult = aiWhereResult + text; break;
                }

                this.writer.Write(text);
            }

            protected void Indent(Indentation style)
            {
                if (style == Indentation.Inner)
                {
                    // this.depth++;
                }
                else if (style == Indentation.Outer)
                {
                    // this.depth--;
                    System.Diagnostics.Debug.Assert(this.depth >= 0);
                }
            }

            protected virtual string GetOperator(ExpressionType type)
            {
                switch (type)
                {
                    case ExpressionType.Not:
                        return "!";
                    case ExpressionType.Add:
                    case ExpressionType.AddChecked:
                        return "+";
                    case ExpressionType.Negate:
                    case ExpressionType.NegateChecked:
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractChecked:
                        return "-";
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyChecked:
                        return "*";
                    case ExpressionType.Divide:
                        return "/";
                    case ExpressionType.Modulo:
                        return "%";
                    case ExpressionType.And:
                        return "&";
                    case ExpressionType.AndAlso:
                        return "and";
                    case ExpressionType.Or:
                        return "Or";
                    case ExpressionType.OrElse:
                        return "Or";
                    case ExpressionType.LessThan:
                        return "<";
                    case ExpressionType.LessThanOrEqual:
                        return "<=";
                    case ExpressionType.GreaterThan:
                        return ">";
                    case ExpressionType.GreaterThanOrEqual:
                        return ">=";
                    case ExpressionType.Equal:
                        return "=";
                    case ExpressionType.NotEqual:
                        return "!=";
                    case ExpressionType.Coalesce:
                        return "??";
                    case ExpressionType.RightShift:
                        return ">>";
                    case ExpressionType.LeftShift:
                        return "<<";
                    case ExpressionType.ExclusiveOr:
                        return "^";
                    default:
                        return null;
                }
            }

            protected override Expression VisitBinary(BinaryExpression b)
            {
                this.Write("(");
                if (b.NodeType == ExpressionType.Power)
                {
                    this.Write("POWER(");
                    this.VisitValue(b.Left);
                    this.Write(", ");
                    this.VisitValue(b.Right);
                    this.Write(")");
                    //  return b;
                }
                else if (b.NodeType == ExpressionType.Coalesce)
                {
                    this.Write("COALESCE(");
                    this.VisitValue(b.Left);
                    this.Write(", ");
                    Expression right = b.Right;
                    while (right.NodeType == ExpressionType.Coalesce)
                    {
                        BinaryExpression rb = (BinaryExpression)right;
                        this.VisitValue(rb.Left);
                        this.Write(", ");
                        right = rb.Right;
                    }
                    this.VisitValue(right);
                    this.Write(")");
                    // return b;
                }
                else if (b.NodeType == ExpressionType.LeftShift)
                {
                    this.Write("(");
                    this.VisitValue(b.Left);
                    this.Write(" * POWER(2, ");
                    this.VisitValue(b.Right);
                    this.Write("))");
                    // return b;
                }
                else if (b.NodeType == ExpressionType.RightShift)
                {
                    this.Write("(");
                    this.VisitValue(b.Left);
                    this.Write(" / POWER(2, ");
                    this.VisitValue(b.Right);
                    this.Write("))");
                    //return b;
                }

                else
                {
                    //if (b.Left is MemberExpression)
                    //{
                    //    if (((MemberExpression)b.Left).Type == typeof(bool) && (bizRead == BizExpSqlType.bizWhere))
                    //    {
                    //        this.Write("(" + ((MemberExpression)b.Left).Member.Name + " = 1)");
                    //    }
                    //    else
                    //    {
                    //        this.Visit(b.Left);
                    //    }
                    //}
                    //else
                    //{
                    this.Visit(b.Left);
                    //}

                    this.Write(" ");
                    this.Write(GetOperator(b.NodeType));
                    this.Write(" ");
                    this.Visit(b.Right);
                }
                this.Write(")");
                return b;
            }

            protected override Expression VisitUnary(UnaryExpression u)
            {
                switch (u.NodeType)
                {
                    case ExpressionType.Convert:
                    case ExpressionType.ConvertChecked:
                        //this.Write("((");
                        //this.Write(this.GetTypeName(u.Type));
                        // this.Write(")");
                        this.Visit(u.Operand);
                        //this.Write(")");
                        break;
                    case ExpressionType.ArrayLength:
                        this.Visit(u.Operand);
                        this.Write(".Length");
                        break;
                    case ExpressionType.Quote:
                        this.Visit(u.Operand);
                        break;
                    case ExpressionType.TypeAs:
                        this.Visit(u.Operand);
                        this.Write(" as ");
                        this.Write(this.GetTypeName(u.Type));
                        break;
                    case ExpressionType.UnaryPlus:
                        this.Visit(u.Operand);
                        break;

                    default:
                        this.Write(this.GetOperator(u.NodeType));
                        this.Visit(u.Operand);
                        break;
                }
                return u;
            }

            protected virtual string GetTypeName(Type type)
            {
                string name = type.Name;
                name = name.Replace('+', '.');
                int iGeneneric = name.IndexOf('`');
                if (iGeneneric > 0)
                {
                    name = name.Substring(0, iGeneneric);
                }
                if (type.IsGenericType || type.IsGenericTypeDefinition)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(name);
                    sb.Append("<");
                    var args = type.GetGenericArguments();
                    for (int i = 0, n = args.Length; i < n; i++)
                    {
                        if (i > 0)
                        {
                            sb.Append(",");
                        }
                        if (type.IsGenericType)
                        {
                            sb.Append(this.GetTypeName(args[i]));
                        }
                    }
                    sb.Append(">");
                    name = sb.ToString();
                }
                return name;
            }

            protected override Expression VisitConditional(ConditionalExpression c)
            {
                this.Visit(c.Test);
                this.WriteLine(Indentation.Inner);
                this.Write("? ");
                this.Visit(c.IfTrue);
                this.WriteLine(Indentation.Same);
                this.Write(": ");
                this.Visit(c.IfFalse);
                this.Indent(Indentation.Outer);
                return c;
            }

            protected override IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
            {
                for (int i = 0, n = original.Count; i < n; i++)
                {
                    this.VisitBinding(original[i]);
                    if (i < n - 1)
                    {
                        this.Write(",");
                        this.WriteLine(Indentation.Same);
                    }
                }
                return original;
            }

            private static readonly char[] special = new char[] { '\n', '\n', '\\' };

            protected override Expression VisitConstant(ConstantExpression c)
            {
                if (c.Value == null)
                {
                    this.Write("null");
                }
                else if (c.Type == typeof(DateTime) || c.Type == typeof(Guid))
                {
                    this.Write("'");//new DateTime(\" 
                    this.Write(c.Value.ToString());
                    this.Write("'");//\"
                }
                else
                {
                    switch (Type.GetTypeCode(c.Value.GetType()))
                    {
                        case TypeCode.Boolean:
                            this.Write(((bool)c.Value) ? "1" : "0");
                            break;
                        case TypeCode.Single:
                        case TypeCode.Double:
                            string str = c.Value.ToString();
                            if (!str.Contains('.'))
                            {
                                str += ".0";
                            }
                            this.Write(str);
                            break;
                        case TypeCode.DateTime:
                            this.Write("'");//new DateTime(\"
                            this.Write(c.Value.ToString());
                            this.Write("'");//\"
                            break;
                        case TypeCode.String:
                            this.Write("'");
                            this.Write(c.Value.ToString().Replace("'", "\""));
                            this.Write("'");
                            break;
                        default:
                            this.Write(c.Value.ToString());
                            break;
                    }
                }
                return c;
            }

            protected override ElementInit VisitElementInitializer(ElementInit initializer)
            {
                if (initializer.Arguments.Count > 1)
                {
                    this.Write("{");
                    for (int i = 0, n = initializer.Arguments.Count; i < n; i++)
                    {
                        this.Visit(initializer.Arguments[i]);
                        if (i < n - 1)
                        {
                            this.Write(", ");
                        }
                    }
                    this.Write("}");
                }
                else
                {
                    this.Visit(initializer.Arguments[0]);
                }
                return initializer;
            }

            protected override IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original)
            {
                for (int i = 0, n = original.Count; i < n; i++)
                {
                    this.VisitElementInitializer(original[i]);
                    if (i < n - 1)
                    {
                        this.Write(",");
                        this.WriteLine(Indentation.Same);
                    }
                }
                return original;
            }

            protected override ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
            {
                for (int i = 0, n = original.Count; i < n; i++)
                {
                    this.Visit(original[i]);
                    //if (i < n - 1)
                    //{
                    //    this.Write(",");
                    //    this.WriteLine(Indentation.Same);
                    //}
                }
                return original;
            }

            protected override Expression VisitInvocation(InvocationExpression iv)
            {
                this.Write("Invoke(");
                this.WriteLine(Indentation.Inner);
                this.VisitExpressionList(iv.Arguments);
                this.Write(", ");
                this.WriteLine(Indentation.Same);
                this.Visit(iv.Expression);
                this.WriteLine(Indentation.Same);
                this.Write(")");
                this.Indent(Indentation.Outer);
                return iv;
            }

            protected override Expression VisitLambda(LambdaExpression lambda)
            {
                if (lambda.Body.NodeType == ExpressionType.MemberAccess)
                {
                    if (((MemberExpression)lambda.Body).Type == typeof(bool) && (bizRead == AiExpSqlType.aiWhere))
                    {
                        this.Write(((MemberExpression)lambda.Body).Member.Name + " = 1");
                    }
                    else
                    {
                        this.Visit(lambda.Body);
                        return lambda;
                    }

                }
                this.Visit(lambda.Body);
                return lambda;
            }

            protected override Expression VisitListInit(ListInitExpression init)
            {
                this.Visit(init.NewExpression);
                this.Write(" {");
                this.WriteLine(Indentation.Inner);
                this.VisitElementInitializerList(init.Initializers);
                this.WriteLine(Indentation.Outer);
                this.Write("}");
                return init;
            }

            protected override Expression VisitMemberAccess(MemberExpression m)
            {
                this.Write(m.Member.Name);
                // string t = m.Update.GetType().Name;
                if (m.Member.DeclaringType == typeof(string))
                {
                    switch (m.Member.Name)
                    {
                        case "Length":
                            this.Write("LEN(");
                            this.Visit(m.Expression);
                            this.Write(")");
                            return m;
                    }
                }
                else if (m.Member.DeclaringType == typeof(DateTime) || m.Member.DeclaringType == typeof(DateTimeOffset))
                {
                    switch (m.Member.Name)
                    {
                        case "Day":
                            this.Write("DAY(");
                            this.Visit(m.Expression);
                            this.Write(")");
                            return m;
                        case "Month":
                            this.Write("MONTH(");
                            this.Visit(m.Expression);
                            this.Write(")");
                            return m;
                        case "Year":
                            this.Write("YEAR(");
                            this.Visit(m.Expression);
                            this.Write(")");
                            return m;
                        case "Hour":
                            this.Write("DATEPART(hour, ");
                            this.Visit(m.Expression);
                            this.Write(")");
                            return m;
                        case "Minute":
                            this.Write("DATEPART(minute, ");
                            this.Visit(m.Expression);
                            this.Write(")");
                            return m;
                        case "Second":
                            this.Write("DATEPART(second, ");
                            this.Visit(m.Expression);
                            this.Write(")");
                            return m;
                        case "Millisecond":
                            this.Write("DATEPART(millisecond, ");
                            this.Visit(m.Expression);
                            this.Write(")");
                            return m;
                        case "DayOfWeek":
                            this.Write("(DATEPART(weekday, ");
                            this.Visit(m.Expression);
                            this.Write(") - 1)");
                            return m;
                        case "DayOfYear":
                            this.Write("(DATEPART(dayofyear, ");
                            this.Visit(m.Expression);
                            this.Write(") - 1)");
                            return m;


                    }
                }
                return base.VisitMemberAccess(m);
            }

            protected override MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
            {
                this.Write(assignment.Member.Name);
                this.Write(" = ");
                this.Visit(assignment.Expression);
                return assignment;
            }

            protected override Expression VisitMemberInit(MemberInitExpression init)
            {
                this.Visit(init.NewExpression);
                this.Write(" {");
                this.WriteLine(Indentation.Inner);
                this.VisitBindingList(init.Bindings);
                this.WriteLine(Indentation.Outer);
                this.Write("}");
                return init;
            }

            protected override MemberListBinding VisitMemberListBinding(MemberListBinding binding)
            {
                this.Write(binding.Member.Name);
                this.Write(" = {");
                this.WriteLine(Indentation.Inner);
                this.VisitElementInitializerList(binding.Initializers);
                this.WriteLine(Indentation.Outer);
                this.Write("}");
                return binding;
            }

            protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
            {
                this.Write(binding.Member.Name);
                this.Write(" = {");
                this.WriteLine(Indentation.Inner);
                this.VisitBindingList(binding.Bindings);
                this.WriteLine(Indentation.Outer);
                this.Write("}");
                return binding;
            }

            protected override Expression VisitMethodCall(MethodCallExpression m)
            {
                //if (m.Object != null)
                //{
                //    this.Visit(m.Object);
                //}
                //else
                //{
                //     this.Write(this.GetTypeName(m.Method.DeclaringType));
                //}
                string bizname = m.Method.Name.ToLower();

                //if (this.GetTypeName(m.Method.DeclaringType).ToLower() != "queryable")
                //{
                //    this.Write(".");
                //}
                //    this.Write(m.Method.Name);

                switch (bizname)
                {
                    case "where":
                        if (bizRead == AiExpSqlType.aiWhere)
                        {
                            AiWhereTime = AiWhereTime + 1;
                        }
                        else
                        {
                            this.Visit(m.Arguments[0]);
                            return m;
                        }
                        break;
                    case "orderby":
                    case "orderbydescending":
                    case "thenbydescending":
                    case "thenby":
                        if (bizRead == AiExpSqlType.aiOrder)
                        {
                            AiOrderTime = AiOrderTime + 1;
                        }
                        else
                        {
                            this.Visit(m.Arguments[0]);
                            return m;
                        }
                        break;
                }

                //bool bizisw = bizname == "where" || bizname == "orderby" || bizname == "thenby";
                //if (bizisw)
                //{
                //    this.Write(m.Method.Name);
                //}

                //this.VisitExpressionList(m.Arguments);

                if (m.Arguments.Count > 1)
                {

                }
                //    this.WriteLine(Indentation.Outer);
                //if (bizisw)
                //{
                //    this.Write(")");
                //}
                //return m;
                #region

                if (m.Method.DeclaringType == typeof(string))
                {
                    switch (m.Method.Name)
                    {
                        case "StartsWith":
                            this.Write("(");
                            this.Visit(m.Object);
                            this.Write(" LIKE ");
                            this.Visit(m.Arguments[0]);
                            this.Write(" + '%')");
                            return m;
                        case "EndsWith":
                            this.Write("(");
                            this.Visit(m.Object);
                            this.Write(" LIKE '%' + ");
                            this.Visit(m.Arguments[0]);
                            this.Write(")");
                            return m;
                        case "Contains":
                            this.Write("(");
                            this.Visit(m.Object);
                            this.Write(" LIKE '%' + ");
                            this.Visit(m.Arguments[0]);
                            this.Write(" + '%')");
                            return m;
                        case "Concat":
                            IList<Expression> args = m.Arguments;
                            if (args.Count == 1 && args[0].NodeType == ExpressionType.NewArrayInit)
                            {
                                args = ((NewArrayExpression)args[0]).Expressions;
                            }
                            for (int i = 0, n = args.Count; i < n; i++)
                            {
                                if (i > 0) this.Write(" + ");
                                this.Visit(args[i]);
                            }
                            return m;
                        case "IsNullOrEmpty":
                            this.Write("(");
                            this.Visit(m.Arguments[0]);
                            this.Write(" IS NULL OR ");
                            this.Visit(m.Arguments[0]);
                            this.Write(" = '')");
                            return m;
                        case "ToUpper":
                            this.Write("UPPER(");
                            this.Visit(m.Object);
                            this.Write(")");
                            return m;
                        case "ToLower":
                            this.Write("LOWER(");
                            this.Visit(m.Object);
                            this.Write(")");
                            return m;
                        case "Replace":
                            this.Write("REPLACE(");
                            this.Visit(m.Object);
                            this.Write(", ");
                            this.Visit(m.Arguments[0]);
                            this.Write(", ");
                            this.Visit(m.Arguments[1]);
                            this.Write(")");
                            return m;
                        case "Substring":
                            this.Write("SUBSTRING(");
                            this.Visit(m.Object);
                            this.Write(", ");
                            this.Visit(m.Arguments[0]);
                            this.Write(" + 1, ");
                            if (m.Arguments.Count == 2)
                            {
                                this.Visit(m.Arguments[1]);
                            }
                            else
                            {
                                this.Write("8000");
                            }
                            this.Write(")");
                            return m;
                        case "Remove":
                            this.Write("STUFF(");
                            this.Visit(m.Object);
                            this.Write(", ");
                            this.Visit(m.Arguments[0]);
                            this.Write(" + 1, ");
                            if (m.Arguments.Count == 2)
                            {
                                this.Visit(m.Arguments[1]);
                            }
                            else
                            {
                                this.Write("8000");
                            }
                            this.Write(", '')");
                            return m;
                        case "IndexOf":
                            this.Write("(CHARINDEX(");
                            this.Visit(m.Arguments[0]);
                            this.Write(", ");
                            this.Visit(m.Object);
                            if (m.Arguments.Count == 2 && m.Arguments[1].Type == typeof(int))
                            {
                                this.Write(", ");
                                this.Visit(m.Arguments[1]);
                                this.Write(" + 1");
                            }
                            this.Write(") - 1)");
                            return m;
                        case "Trim":
                            this.Write("RTRIM(LTRIM(");
                            this.Visit(m.Object);
                            this.Write("))");
                            return m;
                    }
                }
                else if (m.Method.DeclaringType == typeof(DateTime))
                {
                    switch (m.Method.Name)
                    {
                        case "op_Subtract":
                            if (m.Arguments[1].Type == typeof(DateTime))
                            {
                                this.Write("DATEDIFF(");
                                this.Visit(m.Arguments[0]);
                                this.Write(", ");
                                this.Visit(m.Arguments[1]);
                                this.Write(")");
                                return m;
                            }
                            break;
                        case "AddYears":
                            this.Write("DATEADD(YYYY,");
                            this.Visit(m.Arguments[0]);
                            this.Write(",");
                            this.Visit(m.Object);
                            this.Write(")");
                            return m;
                        case "AddMonths":
                            this.Write("DATEADD(MM,");
                            this.Visit(m.Arguments[0]);
                            this.Write(",");
                            this.Visit(m.Object);
                            this.Write(")");
                            return m;
                        case "AddDays":
                            this.Write("DATEADD(DAY,");
                            this.Visit(m.Arguments[0]);
                            this.Write(",");
                            this.Visit(m.Object);
                            this.Write(")");
                            return m;
                        case "AddHours":
                            this.Write("DATEADD(HH,");
                            this.Visit(m.Arguments[0]);
                            this.Write(",");
                            this.Visit(m.Object);
                            this.Write(")");
                            return m;
                        case "AddMinutes":
                            this.Write("DATEADD(MI,");
                            this.Visit(m.Arguments[0]);
                            this.Write(",");
                            this.Visit(m.Object);
                            this.Write(")");
                            return m;
                        case "AddSeconds":
                            this.Write("DATEADD(SS,");
                            this.Visit(m.Arguments[0]);
                            this.Write(",");
                            this.Visit(m.Object);
                            this.Write(")");
                            return m;
                        case "AddMilliseconds":
                            this.Write("DATEADD(MS,");
                            this.Visit(m.Arguments[0]);
                            this.Write(",");
                            this.Visit(m.Object);
                            this.Write(")");
                            return m;
                    }
                }
                else if (m.Method.DeclaringType == typeof(Decimal))
                {
                    switch (m.Method.Name)
                    {
                        case "Add":
                        case "Subtract":
                        case "Multiply":
                        case "Divide":
                        case "Remainder":
                            this.Write("(");
                            this.VisitValue(m.Arguments[0]);
                            this.Write(" ");
                            this.Write(GetOperator(m.Method.Name));
                            this.Write(" ");
                            this.VisitValue(m.Arguments[1]);
                            this.Write(")");
                            return m;
                        case "Negate":
                            this.Write("-");
                            this.Visit(m.Arguments[0]);
                            this.Write("");
                            return m;
                        case "Ceiling":
                        case "Floor":
                            this.Write(m.Method.Name.ToUpper());
                            this.Write("(");
                            this.Visit(m.Arguments[0]);
                            this.Write(")");
                            return m;
                        case "Round":
                            if (m.Arguments.Count == 1)
                            {
                                this.Write("ROUND(");
                                this.Visit(m.Arguments[0]);
                                this.Write(", 0)");
                                return m;
                            }
                            else if (m.Arguments.Count == 2 && m.Arguments[1].Type == typeof(int))
                            {
                                this.Write("ROUND(");
                                this.Visit(m.Arguments[0]);
                                this.Write(", ");
                                this.Visit(m.Arguments[1]);
                                this.Write(")");
                                return m;
                            }
                            break;
                        case "Truncate":
                            this.Write("ROUND(");
                            this.Visit(m.Arguments[0]);
                            this.Write(", 0, 1)");
                            return m;
                    }
                }
                else if (m.Method.DeclaringType == typeof(Math))
                {
                    switch (m.Method.Name)
                    {
                        case "Abs":
                        case "Acos":
                        case "Asin":
                        case "Atan":
                        case "Cos":
                        case "Exp":
                        case "Log10":
                        case "Sin":
                        case "Tan":
                        case "Sqrt":
                        case "Sign":
                        case "Ceiling":
                        case "Floor":
                            this.Write(m.Method.Name.ToUpper());
                            this.Write("(");
                            this.Visit(m.Arguments[0]);
                            this.Write(")");
                            return m;
                        case "Atan2":
                            this.Write("ATN2(");
                            this.Visit(m.Arguments[0]);
                            this.Write(", ");
                            this.Visit(m.Arguments[1]);
                            this.Write(")");
                            return m;
                        case "Log":
                            if (m.Arguments.Count == 1)
                            {
                                goto case "Log10";
                            }
                            break;
                        case "Pow":
                            this.Write("POWER(");
                            this.Visit(m.Arguments[0]);
                            this.Write(", ");
                            this.Visit(m.Arguments[1]);
                            this.Write(")");
                            return m;
                        case "Round":
                            if (m.Arguments.Count == 1)
                            {
                                this.Write("ROUND(");
                                this.Visit(m.Arguments[0]);
                                this.Write(", 0)");
                                return m;
                            }
                            else if (m.Arguments.Count == 2 && m.Arguments[1].Type == typeof(int))
                            {
                                this.Write("ROUND(");
                                this.Visit(m.Arguments[0]);
                                this.Write(", ");
                                this.Visit(m.Arguments[1]);
                                this.Write(")");
                                return m;
                            }
                            break;
                        case "Truncate":
                            this.Write("ROUND(");
                            this.Visit(m.Arguments[0]);
                            this.Write(", 0, 1)");
                            return m;
                    }
                }
                if (m.Method.Name == "ToString")
                {
                    if (m.Object.Type != typeof(string))
                    {
                        this.Write("CONVERT(NVARCHAR, ");
                        this.Visit(m.Object);
                        this.Write(")");
                    }
                    else
                    {
                        this.Visit(m.Object);
                    }
                    return m;
                }
                else if (!m.Method.IsStatic && m.Method.Name == "CompareTo" && m.Method.ReturnType == typeof(int) && m.Arguments.Count == 1)
                {
                    this.Write("(CASE WHEN ");
                    this.Visit(m.Object);
                    this.Write(" = ");
                    this.Visit(m.Arguments[0]);
                    this.Write(" THEN 0 WHEN ");
                    this.Visit(m.Object);
                    this.Write(" < ");
                    this.Visit(m.Arguments[0]);
                    this.Write(" THEN -1 ELSE 1 END)");
                    return m;
                }
                else if (m.Method.IsStatic && m.Method.Name == "Compare" && m.Method.ReturnType == typeof(int) && m.Arguments.Count == 2)
                {
                    this.Write("(CASE WHEN ");
                    this.Visit(m.Arguments[0]);
                    this.Write(" = ");
                    this.Visit(m.Arguments[1]);
                    this.Write(" THEN 0 WHEN ");
                    this.Visit(m.Arguments[0]);
                    this.Write(" < ");
                    this.Visit(m.Arguments[1]);
                    this.Write(" THEN -1 ELSE 1 END)");
                    return m;
                }
                #endregion

                if (m.Arguments.Count > 1)
                {
                    this.WriteLine(Indentation.Outer);
                }
                base.VisitMethodCall(m);
                if (m.Arguments.Count > 1)
                {
                    switch (bizname)
                    {
                        case "orderbydescending":
                        case "thenbydescending": aiOrdeRsult = aiOrdeRsult + " Desc"; break;

                    }
                    if (bizRead == AiExpSqlType.aiOrder)
                    {
                        aiOrdeRsult = aiOrdeRsult + ",";
                    }
                    else
                    {

                        aiWhereResult = aiWhereResult + " and ";


                    }
                    this.WriteLine(Indentation.Outer);
                }

                return m;
            }
            protected Expression VisitValue(Expression expr)
            {
                if (IsPredicate(expr))
                {
                    this.Write("CASE WHEN (");
                    this.Visit(expr);
                    this.Write(") THEN 1 ELSE 0 END");
                    return expr;
                }
                return expr;
            }
            protected override NewExpression VisitNew(NewExpression nex)
            {
                //this.Write("new ");
                //this.Write(this.GetTypeName(nex.Constructor.DeclaringType));
                //this.Write("(");
                //if (nex.Arguments.Count > 1)
                //    this.WriteLine(Indentation.Inner);
                //this.VisitExpressionList(nex.Arguments);
                //if (nex.Arguments.Count > 1)
                //    this.WriteLine(Indentation.Outer);
                //this.Write(")");
                //return nex;
                if (nex.Constructor.DeclaringType == typeof(DateTime))
                {
                    if (nex.Arguments.Count == 3)
                    {
                        this.Write("Convert(DateTime, ");
                        this.Write("Convert(nvarchar, ");
                        this.Visit(nex.Arguments[0]);
                        this.Write(") + '/' + ");
                        this.Write("Convert(nvarchar, ");
                        this.Visit(nex.Arguments[1]);
                        this.Write(") + '/' + ");
                        this.Write("Convert(nvarchar, ");
                        this.Visit(nex.Arguments[2]);
                        this.Write("))");
                        return nex;
                    }
                    else if (nex.Arguments.Count == 6)
                    {
                        this.Write("Convert(DateTime, ");
                        this.Write("Convert(nvarchar, ");
                        this.Visit(nex.Arguments[0]);
                        this.Write(") + '/' + ");
                        this.Write("Convert(nvarchar, ");
                        this.Visit(nex.Arguments[1]);
                        this.Write(") + '/' + ");
                        this.Write("Convert(nvarchar, ");
                        this.Visit(nex.Arguments[2]);
                        this.Write(") + ' ' + ");
                        this.Write("Convert(nvarchar, ");
                        this.Visit(nex.Arguments[3]);
                        this.Write(") + ':' + ");
                        this.Write("Convert(nvarchar, ");
                        this.Visit(nex.Arguments[4]);
                        this.Write(") + ':' + ");
                        this.Write("Convert(nvarchar, ");
                        this.Visit(nex.Arguments[5]);
                        this.Write("))");
                        return nex;
                    }
                }
                return base.VisitNew(nex);
            }

            protected override Expression VisitNewArray(NewArrayExpression na)
            {
                this.Write("new ");
                this.Write(this.GetTypeName(AiTypeHelper.GetElementType(na.Type)));
                this.Write("[] {");
                if (na.Expressions.Count > 1)
                    this.WriteLine(Indentation.Inner);
                this.VisitExpressionList(na.Expressions);
                if (na.Expressions.Count > 1)
                    this.WriteLine(Indentation.Outer);
                this.Write("}");
                return na;
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                // this.Write(p.Name);
                return p;
            }

            protected override Expression VisitTypeIs(TypeBinaryExpression b)
            {
                this.Visit(b.Expression);
                this.Write(" is ");
                this.Write(this.GetTypeName(b.TypeOperand));
                return b;
            }

            protected override Expression VisitUnknown(Expression expression)
            {
                this.Write(expression.ToString());
                return expression;
            }

            protected virtual bool IsBoolean(Type type)
            {
                return type == typeof(bool) || type == typeof(bool?);
            }

            protected virtual bool IsPredicate(Expression expr)
            {
                switch (expr.NodeType)
                {
                    case ExpressionType.And:
                    case ExpressionType.AndAlso:
                    case ExpressionType.Or:
                    case ExpressionType.OrElse:
                        return IsBoolean(((BinaryExpression)expr).Type);
                    case ExpressionType.Not:
                        return IsBoolean(((UnaryExpression)expr).Type);
                    case ExpressionType.Equal:
                    case ExpressionType.NotEqual:
                    case ExpressionType.LessThan:
                    case ExpressionType.LessThanOrEqual:
                    case ExpressionType.GreaterThan:
                    case ExpressionType.GreaterThanOrEqual:
                    case (ExpressionType)DbExpressionType.IsNull:
                    case (ExpressionType)DbExpressionType.Between:
                    case (ExpressionType)DbExpressionType.Exists:
                    case (ExpressionType)DbExpressionType.In:
                        return true;
                    case ExpressionType.Call:
                        return IsBoolean(((MethodCallExpression)expr).Type);
                    default:
                        return false;
                }
            }


            internal enum DbExpressionType
            {
                Table = 1000, // make sure these don't overlap with ExpressionType
                ClientJoin,
                Column,
                Select,
                Projection,
                Entity,
                Join,
                Aggregate,
                Scalar,
                Exists,
                In,
                Grouping,
                AggregateSubquery,
                IsNull,
                Between,
                RowCount,
                NamedValue,
                OuterJoined,
                Insert,
                Update,
                Delete,
                Batch,
                Function,
                Block,
                If,
                Declaration,
                Variable
            }


            protected virtual string GetOperator(string methodName)
            {
                switch (methodName)
                {
                    case "Add": return "+";
                    case "Subtract": return "-";
                    case "Multiply": return "*";
                    case "Divide": return "/";
                    case "Negate": return "-";
                    case "Remainder": return "%";
                    default: return null;
                }
            }

            protected virtual string GetOperator(UnaryExpression u)
            {
                switch (u.NodeType)
                {
                    case ExpressionType.Negate:
                    case ExpressionType.NegateChecked:
                        return "-";
                    case ExpressionType.UnaryPlus:
                        return "+";
                    case ExpressionType.Not:
                        return IsBoolean(u.Operand.Type) ? "NOT" : "~";
                    default:
                        return "";
                }
            }

            protected virtual string GetOperator(BinaryExpression b)
            {
                switch (b.NodeType)
                {
                    case ExpressionType.And:
                    case ExpressionType.AndAlso:
                        return (IsBoolean(b.Left.Type)) ? "AND" : "&";
                    case ExpressionType.Or:
                    case ExpressionType.OrElse:
                        return (IsBoolean(b.Left.Type) ? "OR" : "|");
                    case ExpressionType.Equal:
                        return "=";
                    case ExpressionType.NotEqual:
                        return "<>";
                    case ExpressionType.LessThan:
                        return "<";
                    case ExpressionType.LessThanOrEqual:
                        return "<=";
                    case ExpressionType.GreaterThan:
                        return ">";
                    case ExpressionType.GreaterThanOrEqual:
                        return ">=";
                    case ExpressionType.Add:
                    case ExpressionType.AddChecked:
                        return "+";
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractChecked:
                        return "-";
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyChecked:
                        return "*";
                    case ExpressionType.Divide:
                        return "/";
                    case ExpressionType.Modulo:
                        return "%";
                    case ExpressionType.ExclusiveOr:
                        return "^";
                    case ExpressionType.LeftShift:
                        return "<<";
                    case ExpressionType.RightShift:
                        return ">>";
                    default:
                        return "";
                }
            }
        }



        /// <summary>
        ///重写一个表达示树,并将其中引用变量转换成常量
        ///去除所附加的类信息
        /// </summary>
        internal static class AiPartialEvaluator
        {
            /// <summary>
            /// Performs evaluation & replacement of independent sub-trees
            /// </summary>
            /// <param name="expression">The root of the expression tree.</param>
            /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
            public static Expression Eval(Expression expression)
            {
                return Eval(expression, null);
            }

            /// <summary>
            /// Performs evaluation & replacement of independent sub-trees
            /// </summary>
            /// <param name="expression">The root of the expression tree.</param>
            /// <param name="fnCanBeEvaluated">A function that decides whether a given expression node can be part of the local function.</param>
            /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
            public static Expression Eval(Expression expression, Func<Expression, bool> fnCanBeEvaluated)
            {
                if (fnCanBeEvaluated == null)
                    fnCanBeEvaluated = AiPartialEvaluator.CanBeEvaluatedLocally;
                return SubtreeEvaluator.Eval(Nominator.Nominate(fnCanBeEvaluated, expression), expression);
            }

            private static bool CanBeEvaluatedLocally(Expression expression)
            {
                return expression.NodeType != ExpressionType.Parameter;
            }

            /// <summary>
            /// Evaluates & replaces sub-trees when first candidate is reached (top-down)
            /// </summary>
            class SubtreeEvaluator : AiExpressionVisitor
            {
                HashSet<Expression> candidates;

                private SubtreeEvaluator(HashSet<Expression> candidates)
                {
                    this.candidates = candidates;
                }

                internal static Expression Eval(HashSet<Expression> candidates, Expression exp)
                {
                    return new SubtreeEvaluator(candidates).Visit(exp);
                }

                protected override Expression Visit(Expression exp)
                {
                    if (exp == null)
                    {
                        return null;
                    }

                    if (this.candidates.Contains(exp))
                    {
                        return this.Evaluate(exp);
                    }

                    return base.Visit(exp);
                }

                private Expression Evaluate(Expression e)
                {
                    Type type = e.Type;

                    // check for nullable converts & strip them
                    if (e.NodeType == ExpressionType.Convert)
                    {
                        var u = (UnaryExpression)e;
                        if (AiTypeHelper.GetNonNullableType(u.Operand.Type) == AiTypeHelper.GetNonNullableType(type))
                        {
                            e = ((UnaryExpression)e).Operand;
                        }
                    }

                    // if we now just have a constant, return it
                    if (e.NodeType == ExpressionType.Constant)
                    {
                        var ce = (ConstantExpression)e;

                        // if we've lost our nullable typeness add it back
                        if (e.Type != type && AiTypeHelper.GetNonNullableType(e.Type) == AiTypeHelper.GetNonNullableType(type))
                        {
                            e = ce = Expression.Constant(ce.Value, type);
                        }

                        return e;
                    }

                    var me = e as MemberExpression;
                    if (me != null)
                    {
                        // member accesses off of constant's are common, and yet since these partial evals
                        // are never re-used, using reflection to access the member is faster than compiling  
                        // and invoking a lambda
                        var ce = me.Expression as ConstantExpression;
                        if (ce != null)
                        {
                            return Expression.Constant(me.Member.GetValue(ce.Value), type);
                        }
                    }

                    if (type.IsValueType)
                    {
                        e = Expression.Convert(e, typeof(object));
                    }

                    Expression<Func<object>> lambda = Expression.Lambda<Func<object>>(e);
#if NOREFEMIT
                Func<object> fn = ExpressionEvaluator.CreateDelegate(lambda);
#else
                    Func<object> fn = lambda.Compile();
#endif
                    return Expression.Constant(fn(), type);
                }
            }

            /// <summary>
            /// Performs bottom-up analysis to determine which nodes can possibly
            /// be part of an evaluated sub-tree.
            /// </summary>
            class Nominator : AiExpressionVisitor
            {
                Func<Expression, bool> fnCanBeEvaluated;
                HashSet<Expression> candidates;
                bool cannotBeEvaluated;

                private Nominator(Func<Expression, bool> fnCanBeEvaluated)
                {
                    this.candidates = new HashSet<Expression>();
                    this.fnCanBeEvaluated = fnCanBeEvaluated;
                }

                internal static HashSet<Expression> Nominate(Func<Expression, bool> fnCanBeEvaluated, Expression expression)
                {
                    Nominator nominator = new Nominator(fnCanBeEvaluated);
                    nominator.Visit(expression);
                    return nominator.candidates;
                }

                protected override Expression VisitConstant(ConstantExpression c)
                {
                    return base.VisitConstant(c);
                }

                protected override Expression Visit(Expression expression)
                {
                    if (expression != null)
                    {
                        bool saveCannotBeEvaluated = this.cannotBeEvaluated;
                        this.cannotBeEvaluated = false;
                        base.Visit(expression);
                        if (!this.cannotBeEvaluated)
                        {
                            if (this.fnCanBeEvaluated(expression))
                            {
                                this.candidates.Add(expression);
                            }
                            else
                            {
                                this.cannotBeEvaluated = true;
                            }
                        }
                        this.cannotBeEvaluated |= saveCannotBeEvaluated;
                    }
                    return expression;
                }
            }
        }






        /// <summary>
        /// 类型关系帮助方法
        /// </summary>
        internal static class AiTypeHelper
        {
            public static Type FindIEnumerable(Type seqType)
            {
                if (seqType == null || seqType == typeof(string))
                    return null;
                if (seqType.IsArray)
                    return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());
                if (seqType.IsGenericType)
                {
                    foreach (Type arg in seqType.GetGenericArguments())
                    {
                        Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
                        if (ienum.IsAssignableFrom(seqType))
                        {
                            return ienum;
                        }
                    }
                }
                Type[] ifaces = seqType.GetInterfaces();
                if (ifaces != null && ifaces.Length > 0)
                {
                    foreach (Type iface in ifaces)
                    {
                        Type ienum = FindIEnumerable(iface);
                        if (ienum != null) return ienum;
                    }
                }
                if (seqType.BaseType != null && seqType.BaseType != typeof(object))
                {
                    return FindIEnumerable(seqType.BaseType);
                }
                return null;
            }

            public static Type GetSequenceType(Type elementType)
            {
                return typeof(IEnumerable<>).MakeGenericType(elementType);
            }

            public static Type GetElementType(Type seqType)
            {
                Type ienum = FindIEnumerable(seqType);
                if (ienum == null) return seqType;
                return ienum.GetGenericArguments()[0];
            }

            public static bool IsNullableType(Type type)
            {
                return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
            }

            public static bool IsNullAssignable(Type type)
            {
                return !type.IsValueType || IsNullableType(type);
            }

            public static Type GetNonNullableType(Type type)
            {
                if (IsNullableType(type))
                {
                    return type.GetGenericArguments()[0];
                }
                return type;
            }

            public static Type GetNullAssignableType(Type type)
            {
                if (!IsNullAssignable(type))
                {
                    return typeof(Nullable<>).MakeGenericType(type);
                }
                return type;
            }

            public static ConstantExpression GetNullConstant(Type type)
            {
                return Expression.Constant(null, GetNullAssignableType(type));
            }

            public static Type GetMemberType(MemberInfo mi)
            {
                FieldInfo fi = mi as FieldInfo;
                if (fi != null) return fi.FieldType;
                PropertyInfo pi = mi as PropertyInfo;
                if (pi != null) return pi.PropertyType;
                EventInfo ei = mi as EventInfo;
                if (ei != null) return ei.EventHandlerType;
                MethodInfo meth = mi as MethodInfo;  // property getters really
                if (meth != null) return meth.ReturnType;
                return null;
            }

            public static object GetDefault(Type type)
            {
                bool isNullable = !type.IsValueType || AiTypeHelper.IsNullableType(type);
                if (!isNullable)
                    return Activator.CreateInstance(type);
                return null;
            }

            public static bool IsReadOnly(MemberInfo member)
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        return (((FieldInfo)member).Attributes & FieldAttributes.InitOnly) != 0;
                    case MemberTypes.Property:
                        PropertyInfo pi = (PropertyInfo)member;
                        return !pi.CanWrite || pi.GetSetMethod() == null;
                    default:
                        return true;
                }
            }

            public static bool IsInteger(Type type)
            {
                Type nnType = GetNonNullableType(type);
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.SByte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.Byte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        return true;
                    default:
                        return false;
                }
            }
        }


    }


    /// <summary>
    /// 成员反射操作
    /// </summary>
    internal static class AiReflectionExtensions
    {
        public static object GetValue(this MemberInfo member, object instance)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Property:
                    return ((PropertyInfo)member).GetValue(instance, null);
                case MemberTypes.Field:
                    return ((FieldInfo)member).GetValue(instance);
                default:
                    throw new InvalidOperationException();
            }
        }

        public static void SetValue(this MemberInfo member, object instance, object value)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Property:
                    var pi = (PropertyInfo)member;
                    pi.SetValue(instance, value, null);
                    break;
                case MemberTypes.Field:
                    var fi = (FieldInfo)member;
                    fi.SetValue(instance, value);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }



    internal class LamSQL
    {
        public string WhereSql { get; set; }
        public string OrderbySql { get; set; }
    }






}
