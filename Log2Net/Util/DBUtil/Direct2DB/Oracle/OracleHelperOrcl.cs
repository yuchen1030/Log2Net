
//#define MS_OracleClient  // 是采用微软oracle类库还是oracle自家的类库

using Log2Net.Util.DBUtil.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

#if MS_OracleClient
using System.Data.OracleClient;
#else
using Oracle.ManagedDataAccess.Client;
#endif

namespace Log2Net.Util.DBUtil.Direct2DB.Oracle
{
    /// <summary>
    /// 使用Oracle.ManagedDataAccess.Client实现的oracle 数据库访问类(无需安装客户端)，无32位/64位之分，但仅支持Oracle10g及以上
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class OracleHelper<T> : OracleHelperBase<T> where T : class
    {
        public OracleHelper(string strConnStr) : base(strConnStr)
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
                parameters =  ParameterPrepare( parameters);
                using (OracleConnection conn = new OracleConnection(connstr))
                {
                    conn.Open();
                    OracleCommand cmd = conn.CreateCommand();
                    cmd.CommandText = cmdText;
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange((parameters));
                    }
                    var da = new OracleDataAdapter(cmd);
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
                using (OracleConnection con = new OracleConnection(connstr))
                {
                    using (OracleCommand cmd = new OracleCommand(sql, con))
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


                using (OracleConnection conn = new OracleConnection(connstr))

                {
                    conn.Open();

                    OracleCommand cmd = conn.CreateCommand();

                    cmd.CommandText = GetColumnsNameSql(strTableName, strComFields);


                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);

                    adapter.UpdateCommand = new OracleCommandBuilder(adapter).GetUpdateCommand();

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
                using (OracleConnection conn = new OracleConnection(connstr))

                {
                    conn.Open();
                    OracleTransaction tsOprate = conn.BeginTransaction();
                    try
                    {

                        OracleCommand cmd = conn.CreateCommand();

                        cmd.Transaction = tsOprate;
                        foreach (DataTable dtTemp in dsTables.Tables)
                        {
                            string strComFields = "*";
                            if (dicDtFields != null && dicDtFields.Count > 0 && dicDtFields.ContainsKey(dtTemp.TableName))
                            {
                                strComFields = dicDtFields[dtTemp.TableName];
                            }
                            cmd.CommandText = GetColumnsNameSql(dtTemp.TableName, strComFields);


                            OracleDataAdapter adapter = new OracleDataAdapter(cmd);

                            adapter.UpdateCommand = new OracleCommandBuilder(adapter).GetUpdateCommand();

                            //dtTemp.GetChanges(DataRowState.Modified).Rows.Count();

                            DataTable dtChangesTemp = dtTemp.GetChanges();

                            adapter.FillSchema(dtChangesTemp, SchemaType.Mapped);//new added


                            adapter.Update(dtChangesTemp);
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


        public bool DeleteRecords(string cmdText, int intExpectRowNums, params OracleParameter[] parameters)
        {
            bool bolFb = false;

            try
            {

                using (OracleConnection conn = new OracleConnection(connstr))

                {
                    conn.Open();
                    OracleTransaction tsOprate = conn.BeginTransaction();
                    try
                    {

                        OracleCommand cmd = conn.CreateCommand();
                        cmd.CommandText = cmdText;
                        cmd.Transaction = tsOprate;
                        cmd.Parameters.AddRange(parameters);
                        int intRes = cmd.ExecuteNonQuery();
                        if (intRes != intExpectRowNums)
                            throw new Exception("Delete records nums not equal to Expected nums!");

                        tsOprate.Commit();
                        bolFb = true;
                    }
                    catch (Exception e)
                    {
                        tsOprate.Rollback();
                        throw e;
                    }
                }
            }
            catch (Exception e)
            {
                bolFb = false;
                throw new Exception("Err-DataHelper-UpdateDtToDB:" + e.Message);
            }

            return bolFb;
        }


        public bool UpdateFromSqlContianer(List<OraSqlContianer> ltSqls)
        {
            bool bolFb = false;
            string curSQL = "";
            try
            {
                using (OracleConnection conn = new OracleConnection(connstr))

                {
                    conn.Open();
                    OracleTransaction oraOprate = conn.BeginTransaction();
                    try
                    {

                        OracleCommand cmd = conn.CreateCommand();

                        cmd.Transaction = oraOprate;
                        foreach (OraSqlContianer objOraSqlCon in ltSqls)
                        {
                            cmd.CommandText = objOraSqlCon.strSqlTxt;
                            curSQL = objOraSqlCon.strSqlTxt;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(objOraSqlCon.ltOraParams.ToArray());
                            int intRes = cmd.ExecuteNonQuery();
                            if (objOraSqlCon.intExpectNums >= 0)
                            {
                                if (intRes != objOraSqlCon.intExpectNums)
                                    throw new Exception("Update records not match the expect nums");
                            }
                            else if (objOraSqlCon.intExpectNums != Int16.MinValue)
                            {
                                if (intRes != 0 && intRes != objOraSqlCon.intExpectNums * -1)
                                    throw new Exception("Update records not match the expect nums");
                            }

                        }
                        oraOprate.Commit();
                    }
                    catch (Exception e)
                    {
                        oraOprate.Rollback();
                        throw e;
                    }

                }
                bolFb = true;
            }
            catch (Exception e)
            {
                bolFb = false;
                throw new Exception("Err-DataHelper-UpdateSqlContianer:" + e.Message);
            }

            return bolFb;
        }



        public override DbParameter GetOneDbParameter(string name, object value)
        {
            OracleParameter cur = new OracleParameter(name, value);
            return cur;
        }

        public override DbParameter[] ParameterPrepare(DbParameter[] parameters)
        {
            var paras = parameters.Select(a => new OracleParameter(a.ParameterName, a.Value)).ToArray();
            return paras;
        }

    }

}
