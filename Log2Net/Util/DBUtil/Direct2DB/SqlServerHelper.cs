using Log2Net.Models;
using Log2Net.Util.DBUtil.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Log2Net.Util.DBUtil.Direct2DB
{
    /// <summary>
    /// sql server 数据库访问类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class SqlServerHelper<T> : Direct2DBBase<T>, IDirect2DBBase<T> where T : class
    {
        readonly DBBaseAttr dbBaseAttr = new DBBaseAttr() { DataBaseType = DataBaseType.SqlServer, LeftPre = "[", ParaPreChar = "@", RightSuf = "]" };

        protected override DBBaseAttr DBBaseAttr { get { return dbBaseAttr; } }

        public SqlServerHelper(string strConnStr) : base(strConnStr)
        {
            connstr = strConnStr;
        }


        public override ExeResEdm SqlCMD_DT(string cmdText, Func<DbDataAdapter, int> fun, params DbParameter[] parameters)
        {
            ExeResEdm dBResEdm = new ExeResEdm();
            try
            {
                parameters = ParameterPrepare(parameters);
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = cmdText;
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange((parameters));
                    }
                    var da = new SqlDataAdapter(cmd);
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






        protected override ExeResEdm SqlCMD(string sql, Func<DbCommand, object> fun, params DbParameter[] pms)
        {
            ExeResEdm dBResEdm = new ExeResEdm();
            try
            {
                pms = ParameterPrepare(pms);
                using (SqlConnection con = new SqlConnection(connstr))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
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


        // UpdateDtToDB2( new DataTable(), (cmd, bul) => { cmd.UpdateCommand = bul.GetUpdateCommand(); });
        public bool UpdateDtToDBWithDelegate(DataTable dtInfos, Action<DbDataAdapter, DbCommandBuilder> fun, string strComFields = "*")
        {
            bool bolFb = false;
            string strTableName = dtInfos.TableName;
            try
            {
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = GetColumnsNameSql(strTableName, strComFields);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    var cmdBuild = new SqlCommandBuilder(adapter);            
                    fun(adapter, cmdBuild);

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




        public override bool UpdateDtToDB(DataTable dtInfos, string strComFields = "*")
        {
            bool bolFb = false;
            string strTableName = dtInfos.TableName;
            try
            {
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = GetColumnsNameSql(strTableName, strComFields);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.UpdateCommand = new SqlCommandBuilder(adapter).GetUpdateCommand();
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
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    conn.Open();
                    SqlTransaction tsOprate = conn.BeginTransaction();
                    try
                    {
                        SqlCommand cmd = conn.CreateCommand();

                        foreach (DataTable dtTemp in dsTables.Tables)
                        {
                            string strComFields = "*";
                            if (dicDtFields != null && dicDtFields.Count > 0 && dicDtFields.ContainsKey(dtTemp.TableName))
                            {
                                strComFields = dicDtFields[dtTemp.TableName];
                            }
                            cmd.CommandText = GetColumnsNameSql(dtTemp.TableName, strComFields);
                            cmd.Transaction = tsOprate;
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            adapter.UpdateCommand = new SqlCommandBuilder(adapter).GetUpdateCommand();
                            adapter.Update(dtTemp.GetChanges());
                            dtTemp.AcceptChanges();

                            //   var tttt = SqlCMD_DT("", adt => adt.Fill(dt), parameters);
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
            SqlParameter cur = new SqlParameter(name, value);
            return cur;
        }

        public override DbParameter[] ParameterPrepare(DbParameter[] parameters)
        {
            var paras = parameters.Select(a => new SqlParameter(a.ParameterName, a.Value)).ToArray();
            return paras;
        }
    }


}
