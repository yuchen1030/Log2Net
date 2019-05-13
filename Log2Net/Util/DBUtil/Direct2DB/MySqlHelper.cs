using Log2Net.Models;
using Log2Net.Util.DBUtil.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Log2Net.Util.DBUtil.Direct2DB
{
    /// <summary>
    /// sql server 数据库访问类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class MySqlHelper<T> : Direct2DBBase<T>, IDirect2DBBase<T> where T : class
    {
        readonly DBBaseAttr dbBaseAttr = new DBBaseAttr() { DataBaseType = DataBaseType.MySql, LeftPre = "", ParaPreChar = "?", RightSuf = "" };

        protected override DBBaseAttr DBBaseAttr { get { return dbBaseAttr; } }

        public MySqlHelper(string strConnStr) : base(strConnStr)
        {
            connstr = strConnStr;
        }




        public override ExeResEdm SqlCMD_DT(string cmdText, Func<DbDataAdapter, int> fun, params DbParameter[] parameters)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            ExeResEdm dBResEdm = new ExeResEdm();
            try
            {
                parameters = ParameterPrepare(parameters);
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = cmdText;
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange((parameters));
                    }
                    var da = new MySqlDataAdapter(cmd);
                    var res = fun(da);
                    dBResEdm.ExeNum = res;
                }
            }
            catch (Exception ex)
            {
                dBResEdm.Module = "SqlCMD_DT 方法";
                dBResEdm.ExBody = ex;
                dBResEdm.ErrCode = 1;
                return dBResEdm;
            }
            return dBResEdm;
        }



        ExeResEdm ExecuteDataSet(string cmdText, params DbParameter[] parameters)
        {
            DataSet ds = new DataSet();
          //  DataTable dt = new DataTable();
            parameters = ParameterPrepare(parameters);
            var res = SqlCMD_DT(cmdText, adt => adt.Fill(ds), parameters);
            res.ExeModel = ds;
            return res;


            //try
            //{
            //    using (MySqlConnection conn = new MySqlConnection(connstr))
            //    {
            //        conn.Open();
            //        MySqlCommand cmd = conn.CreateCommand();
            //        cmd.CommandText = cmdText;
            //        cmd.Parameters.AddRange(parameters);
            //        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            //        adapter.Fill(ds);
            //    }
            //}
            //catch (Exception e)
            //{
            //    ds = null;
            //    throw new Exception("Err-DataHelper:" + e.Message);
            //}
            //return ds;
        }



        //select SQL_CALC_FOUND_ROWS * from log_systemmonitor where OnlineCnt >1 order by time DESC limit 0,5;
        //SELECT FOUND_ROWS();
        //SELECT SQL_CALC_FOUND_ROWS @rowno:=@rowno+1 as rowno,r.* from log_systemmonitor r,(select @rowno:= 0) t where OnlineCnt >1 order by time DESC;
        //SELECT FOUND_ROWS();
        public override ExeResEdm GetDataByPage(string tableName, string strWhere, string orderby, int pageIndex, int pageSize, out int totalCnt)
        {
            totalCnt = 0;
            StringBuilder strSql = new StringBuilder();
            string columns = "";//为空，则获取全部列
            if (string.IsNullOrEmpty(orderby) || string.IsNullOrEmpty(orderby.Trim()))
            {
                return null;
            }

            if (string.IsNullOrEmpty(columns))
            {
                columns = "*";
            }
            columns = "SQL_CALC_FOUND_ROWS " + columns;

            strSql.Append("SELECT " + columns + " FROM " + tableName);
            if (!string.IsNullOrEmpty(strWhere) && !string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" order by " + orderby + " limit " + DBBaseAttr.ParaPreChar + "startIx," + DBBaseAttr.ParaPreChar + "endIx;");
            strSql.Append("SELECT FOUND_ROWS();");

            Dictionary<string, object> pageDic = new Dictionary<string, object>() {
                { "startIx",pageSize *(pageIndex-1 )  },
                {"endIx", pageSize }
            };

            DbParameter[] pms = GetDbParametersFromDic(pageDic);
            try
            {

                string text = strSql.ToString();
                ExeResEdm ds = ExecuteDataSet(strSql.ToString(), pms);
                if (ds != null && ds.ErrCode == 0 && (ds.ExeModel as DataSet).Tables.Count > 1)
                {
                    totalCnt = Convert.ToInt32((ds.ExeModel as DataSet).Tables[1].Rows[0][0].ToString());
                    return new ExeResEdm() {  ExeModel = (ds.ExeModel as DataSet).Tables[0] };
                }
                else
                {
                    return ds;
                }         
            }
            catch (Exception ex)
            {
                return new ExeResEdm() {  ErrCode =1, ExBody = ex,Module = "GetDataByPage-MySql" };
            }
        }




        protected override ExeResEdm SqlCMD(string sql, Func<DbCommand, object> fun, params DbParameter[] pms)
        {
            ExeResEdm dBResEdm = new ExeResEdm();
            try
            {
                pms = ParameterPrepare(pms);
                using (MySqlConnection con = new MySqlConnection(connstr))
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        con.Open();

                        if (pms != null && pms.Length > 0)
                        {
                            cmd.Parameters.AddRange((pms));
                        }
                        var res = fun(cmd);
                        dBResEdm.ExeModel = res;
                        return dBResEdm;
                    }
                }
            }
            catch (Exception ex)
            {
                dBResEdm.Module = "SqlCMD方法";
                dBResEdm.ExBody = ex;
                dBResEdm.ErrCode = 1;
                return dBResEdm;
            }
        }



        public override bool UpdateDtToDB(DataTable dtInfos, string strComFields = "*")
        {
            bool bolFb = false;
            string strTableName = dtInfos.TableName;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = GetColumnsNameSql(strTableName, strComFields);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.UpdateCommand = new MySqlCommandBuilder(adapter).GetUpdateCommand();
                    adapter.Update(dtInfos.GetChanges());
                    dtInfos.AcceptChanges();
                }
                bolFb = true;
            }
            catch (Exception e)
            {
                bolFb = false;
                throw new Exception("Err-DataHelper-UpdateDtToDB:" + e.Message);
            }

            return bolFb;
        }

        public override bool UpdateDsToDB(DataSet dsTables, Dictionary<string, string> dicDtFields = null)
        {
            bool bolFb = false;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    MySqlTransaction tsOprate = conn.BeginTransaction();
                    try
                    {
                        MySqlCommand cmd = conn.CreateCommand();

                        foreach (DataTable dtTemp in dsTables.Tables)
                        {
                            string strComFields = "*";
                            if (dicDtFields != null && dicDtFields.Count > 0 && dicDtFields.ContainsKey(dtTemp.TableName))
                            {
                                strComFields = dicDtFields[dtTemp.TableName];
                            }
                            cmd.CommandText = GetColumnsNameSql(dtTemp.TableName, strComFields);
                            cmd.Transaction = tsOprate;
                            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                            adapter.UpdateCommand = new MySqlCommandBuilder(adapter).GetUpdateCommand();

                            adapter.Update(dtTemp.GetChanges());
                            dtTemp.AcceptChanges();
                        }
                        dsTables.AcceptChanges();

                        tsOprate.Commit();
                    }
                    catch (Exception e)
                    {
                        tsOprate.Rollback();
                        throw e;
                    }

                }
                bolFb = true;
            }
            catch (Exception e)
            {
                bolFb = false;
                throw new Exception("Err-DataHelper-UpdateDtToDB:" + e.Message);
            }

            return bolFb;
        }


        public override string GetColumnsNameSql(string strTbName, string strField = "*")
        {
            string strSqlTxt = "select top 0 " + strField + " from " + strTbName;
            return strSqlTxt;
        }


        public override DbParameter GetOneDbParameter(string name, object value)
        {
            MySqlParameter cur = new MySqlParameter(name, value);
            return cur;
        }

        public override DbParameter[] ParameterPrepare(DbParameter[] parameters)
        {
            var paras = parameters.Select(a => new MySqlParameter(a.ParameterName, a.Value)).ToArray();
            return paras;
        }
    }


}




