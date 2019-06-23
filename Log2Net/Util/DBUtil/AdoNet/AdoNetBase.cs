using Log2Net.Models;
using Log2Net.Util.DBUtil.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Log2Net.Util.DBUtil.AdoNet
{

    internal interface IAdoNetBase<T> where T : class
    {
        ExeResEdm Add(string tableName, T model, params string[] skipCols);
        ExeResEdm GetListByPage(string tableName, PageSerach<T> para);

    }



    internal abstract class AdoNetBase<T> : IAdoNetBase<T> where T : class
    {
        #region 属性字段   
        protected abstract DBBaseAttr DBBaseAttr { get; }
        protected string connstr { get; set; }
        #endregion 属性字段

        //构造函数   
        internal AdoNetBase(string strConnStr)
        {
            connstr = strConnStr;
        }

        #region 接口的实现 
        public ExeResEdm Add(string tableName, T model, params string[] skipCols)
        {
            Dictionary<string, object> dic = DtModelConvert<T>.GetPropertity(model);
            object[] values = dic.Values.ToArray();
            // string idVal = dic.Values.ToArray()[0].ToString();
            //SqlParameter[] pms = GetOleDbParameters(dic.Keys.ToList(), dic.Values.ToList());//参数过多，不会影响程序执行的正确性。
            for (int i = 0; i < skipCols.Length; i++)//自动增长的列要忽略
            {
                dic.Remove(skipCols[i]);
            }

            for (int i = dic.Values.Count - 1; i >= 0; i--)//值为空的不参与
            {
                if (dic.Values.ToList()[i] == null)
                {
                    dic.Remove(dic.Keys.ToList()[i]);
                }
            }
            ComDBFun ComDBFun = new ComDBFun(DBBaseAttr);
            string textParas = ComDBFun.GetSQLText(dic.Keys.ToList());
            string sql = "insert into " + tableName + textParas;
            //  SqlParameter[] pms = ComDBFun.GetMSOleDbParameters(dic.Keys.ToList(), dic.Values.ToList());

            DbParameter[] //pms = ComDBFun.GetOrclOleDbParameters(dic.Keys.ToList(), dic.Values.ToList());
            pms = GetDbParametersFromDic(dic);
            var n = ExecuteNonQuery(sql, pms);
            return n;

        }

        public ExeResEdm GetListByPage(string tableName, PageSerach<T> para)
        {
            var orderByStr = LambdaToSqlHelper<T>.GetSqlFromLambda(para.OrderBy).OrderbySql;
            string whereSql = !string.IsNullOrEmpty(para.StrWhere) ? para.StrWhere : LambdaToSqlHelper<T>.GetWhereFromLambda(para.Filter, DataBaseType.NoSelect);
            SearchParam searchParam = new SearchParam() { Orderby = orderByStr, PageIndex = para.PageIndex, PageSize = para.PageSize, TableName = tableName, StrWhere = whereSql, };
            ExeResEdm res = GetDTByPage(searchParam);
            if (res.ErrCode == 0)
            {
                List<T> list = DtModelConvert<T>.DatatableToList((res.ExeModel as DataTable));
                res.ExeModel = list.AsQueryable();
                return res;
            }
            else
            {
                return res;
            }
        }
        #endregion 接口的实现 

        #region 可继承的方法 
        //SQL Server 和 oracle 可以使用此方法，MySQL不行
        protected virtual ExeResEdm GetDataByPage(string tableName, string strWhere, string orderby, int pageIndex, int pageSize, out int totalCnt)
        {
            totalCnt = 0;
            StringBuilder strSql = new StringBuilder();
            string columns = "";//为空，则获取全部列
            if (string.IsNullOrEmpty(orderby) || string.IsNullOrEmpty(orderby.Trim()))
            {
                return new ExeResEdm() { ErrCode = 1, ErrMsg = "orderby 参数不能为空", Module = "GetDataByPage" };
            }

            if (string.IsNullOrEmpty(columns))
            {
                columns = "*";
            }
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageSize * pageIndex;

            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");

            strSql.Append("order by T." + orderby);
            strSql.Append(")AS RowIx, T.* ,COUNT(*) OVER() AS dbtotal from " + tableName + " T ");
            if (!string.IsNullOrEmpty(strWhere) && !string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");

            strSql.AppendFormat(" WHERE TT.RowIx between " + DBBaseAttr.ParaPreChar + "startIx and " + DBBaseAttr.ParaPreChar + "endIx "); //ComDBFun.paraChar

            Dictionary<string, object> pageDic = new Dictionary<string, object>() {
                { "startIx",pageSize *(pageIndex-1 ) +1 },
                {"endIx", pageSize * pageIndex}
            };

            DbParameter[] pms = GetDbParametersFromDic(pageDic);
            try
            {
                string text = strSql.ToString();

                ExeResEdm res = ExecuteDataTable(strSql.ToString(), pms);
                if (res.ErrCode == 0 && res.ExeNum > 0)
                {
                    totalCnt = Convert.ToInt32((res.ExeModel as DataTable).Rows[0]["dbtotal"].ToString());
                }
                return res;
            }

            catch (Exception ex)
            {
                return new ExeResEdm() { ErrCode = 1, ExBody = ex, Module = "GetDataByPage" };
            }

        }

        public DbParameter[] GetDbParametersFromDic(Dictionary<string, object> dic)
        {
            List<DbParameter> list = new List<DbParameter>();

            if (dic == null || dic.Count <= 0)
            {
                return list.ToArray();
            }
            List<string> colNames = dic.Keys.ToList(); List<object> colValues = dic.Values.ToList();
            for (int i = 0; i < dic.Count; i++)
            {
                DbParameter cur = GetOneDbParameter(DBBaseAttr.ParaPreChar + ComDBFun.RemoveSpecialChar(colNames[i]), GetValue(colValues[i]));
                list.Add(cur);
            }
            return list.ToArray();

        }

        #endregion 可继承的方法

        #region 私有方法   
        protected object GetValue(object value)
        {
            if (value.GetType().Name == "".GetType().Name)
            {
                return value.ToString().Trim();
            }
            if (value.GetType().BaseType.Name == "Enum")
            {
                var enumVal = Enum.Parse(value.GetType(), value.ToString());
                return (int)enumVal;
            }
            return value;
        }

        ExeResEdm GetDTByPage(SearchParam searchParam)
        {
            string strWhere = searchParam.StrWhere;
            string orderby = searchParam.Orderby;

            int pageSize = searchParam.PageSize;
            int pageIndex = searchParam.PageIndex;
            int totalCnt = 0;
            if (string.IsNullOrEmpty(orderby) || string.IsNullOrEmpty(orderby.Trim()))
            {
                return null;
            }

            ExeResEdm dtRes = GetDataByPage(searchParam.TableName, strWhere, orderby, pageIndex, pageSize, out totalCnt);
            searchParam.TotalCount = totalCnt;
            return dtRes;
        }

        ExeResEdm ExecuteNonQuery(string cmdText, params DbParameter[] parameters)
        {
            ExeResEdm dBResEdm = SqlCMD(cmdText, cmd => cmd.ExecuteNonQuery(), parameters);
            if (dBResEdm.ErrCode == 0)
            {
                dBResEdm.ExeNum = Convert.ToInt32(dBResEdm.ExeModel);
            }
            return dBResEdm;
        }

        ExeResEdm ExecuteDataTable(string cmdText, params DbParameter[] parameters)
        {
            DataTable dt = new DataTable();
            parameters = ParameterPrepare(parameters);
            var res = SqlCMD_DT(cmdText, adt => adt.Fill(dt), parameters);
            res.ExeModel = dt;
            return res;
        }
        #endregion 私有方法

        #region 抽象方法   
        protected abstract ExeResEdm SqlCMD_DT(string cmdText, Func<DbDataAdapter, int> fun, params DbParameter[] parameters);
        protected abstract ExeResEdm SqlCMD(string sql, Func<DbCommand, object> fun, params DbParameter[] pms);
        protected abstract string GetColumnsNameSql(string strTbName, string strField = "*");
        protected abstract DbParameter GetOneDbParameter(string name, object value);
        protected abstract DbParameter[] ParameterPrepare(DbParameter[] parameters);
        #endregion 抽象方法
    }



}
