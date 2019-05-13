using Log2Net.Util.DBUtil.Models;

#if NET
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Log2Net.Util.DBUtil.EF2DB
{


    internal class EFBaseDal<T> : DBAccess<T> where T : class
    {
        protected DbContext Context;
        protected DbSet<T> DbSet;
        public EFBaseDal(DbContext context)
        {
            this.Context = context;
            this.DbSet = context.Set<T>();
        }

        internal override ExeResEdm /* IQueryable<T> */ GetAll(PageSerach<T> para)
        {
            if (para == null)
            {
                return new ExeResEdm() { ErrCode = 1, Module = "GetAll-EFBaseDal", ErrMsg = "参数不能为空" };
            }
            IQueryable<T> query = this.DbSet;
            var filter = para.Filter;
            var orderBy = para.OrderBy;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (para.IncludeProps != null && para.IncludeProps.Count > 0)
            {
                foreach (var item in para.IncludeProps)
                {
                    //要先判断是否有此导航属性
                    query = query.Include(item);
                }
            }

            if (orderBy != null)
            {
                query = orderBy.Compile()(query);
            }

            int total = query.Count();
            return new ExeResEdm() { ExeModel = query };



        }


        public virtual T GetByID(object id)
        {
            return DbSet.Find(id);
        }

        internal override ExeResEdm Add(AddDBPara<T> dBPara)
        {
            ExeResEdm dBResEdm = new ExeResEdm() { };
            try
            {
                dBResEdm = Insert(dBPara.Model);
                return dBResEdm;
            }

            catch (Exception ex)
            {
                dBResEdm.Module = "Add 方法";
                dBResEdm.ExBody = ex;
                dBResEdm.ErrCode = 1;
                return dBResEdm;
            }
        }


        public virtual ExeResEdm Insert(T entity)
        {
            ExeResEdm dBResEdm = new ExeResEdm() { };
            try
            {
                var t = DbSet.Add(entity);
                this.Context.SaveChanges();
                dBResEdm.ExeModel = t;
                dBResEdm.ExeNum = 1;
                return dBResEdm;
            }
            catch (Exception ex)
            {
                dBResEdm.Module = "Add 方法";
                dBResEdm.ExBody = ex;
                dBResEdm.ErrCode = 1;
                return dBResEdm;
            }


        }

        public virtual T Update(T entity, params Expression<Func<T, object>>[] properties)
        {
            if (properties != null && properties.Length > 0)
            {
                try { Context.Entry(entity).State = EntityState.Unchanged; } catch { }               // 等价于 Context.Entry(entity).State = EntityState.Unchanged;
                List<string> fieldList = GetPropertyNames(properties);
                if (fieldList != null && fieldList.Count > 0)
                {
                    foreach (var item in fieldList)
                    {
                        Context.Entry(entity).Property(item).IsModified = true;
                    }
                }
            }
            else
            {
                try { Context.Entry(entity).State = EntityState.Unchanged; } catch { }  //  DbSet.Attach(entity);
                Context.Entry(entity).State = EntityState.Modified;
            }
            this.Context.SaveChanges();
            return entity;
        }

        public virtual void Delete(object id)
        {
            try
            {
                T entityToDelete = DbSet.Find(id);
                Delete(entityToDelete);
            }
#pragma warning disable CS0168 // 声明了变量“ex”，但从未使用过
            catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量“ex”，但从未使用过
            {
                return;
            }
        }

        public virtual void Delete(T entityToDelete)
        {
            if (entityToDelete == null)
            {
                return;
            }
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }


        //获取属性名称的集合，参数形如a=>a.Name 或 a=>"Name;Code"   
        List<string> GetPropertyNames(Expression<Func<T, object>>[] properties)
        {
            List<string> fieldList = new List<string>();
            if (properties == null || properties.Length <= 0)
            {
                return fieldList;
            }
            foreach (var field in properties)
            {
                var para = field.Parameters[0] + ".";
                string fieldStr = field.ToString();

                if (fieldStr.Contains("Convert("))
                {
                    string fieldNew = GetPropertySymbol(field);
                    int index = fieldStr.IndexOf(para);
                    fieldStr = fieldStr.Substring(index + para.Length);
                    var fieldName = fieldStr.Trim('"').Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    fieldList.Add(fieldName);
                }
                else
                {
                    var fieldNames = field.Body.ToString().Trim('"').Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    fieldList.AddRange(fieldNames);
                }
            }
            return fieldList;
        }


        string GetPropertySymbol<O, TResult>(Expression<Func<O, TResult>> expression)
        {
            return String.Join(".", GetMembersOnPath(expression.Body as MemberExpression).Select(m => m.Member.Name).Reverse());
        }

        IEnumerable<MemberExpression> GetMembersOnPath(MemberExpression expression)
        {
            while (expression != null)
            {
                yield return expression;
                expression = expression.Expression as MemberExpression;
            }
        }



    }
}
