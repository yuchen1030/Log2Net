#if NET

using Log2Net.Util.DBUtil.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;


namespace Log2Net.Util.DBUtil.Direct2DB.Oracle
{
    /// <summary>
    /// 使用System.Data.OracleClient实现的oracle 数据库访问类(需安装客户端)，有32位/64位之分
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class OracleHelperMS<T> : OracleHelperBase<T> where T : class
    {

        //DataBaseType _dataBaseType = DataBaseType.Oracle;
        //public override DataBaseType dataBaseType
        //{
        //    get { return _dataBaseType; }
        //    set { _dataBaseType = value; }
        //}



        public OracleHelperMS(string strConnStr) : base(strConnStr)
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
                using (System.Data.OracleClient.OracleConnection conn = new System.Data.OracleClient.OracleConnection(connstr))
                {
                    conn.Open();
                    System.Data.OracleClient.OracleCommand cmd = conn.CreateCommand();
                    cmd.CommandText = cmdText;
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange((parameters));
                    }
                    var da = new System.Data.OracleClient.OracleDataAdapter(cmd);
                    var n = fun(da);
                    dBResEdm.ExeNum = n;
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
                using (System.Data.OracleClient.OracleConnection con = new System.Data.OracleClient.OracleConnection(connstr))
                {
                    using (System.Data.OracleClient.OracleCommand cmd = new System.Data.OracleClient.OracleCommand(sql, con))
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


                using (System.Data.OracleClient.OracleConnection conn = new System.Data.OracleClient.OracleConnection(connstr))


                {
                    conn.Open();

                    System.Data.OracleClient.OracleCommand cmd = conn.CreateCommand();

                    cmd.CommandText = GetColumnsNameSql(strTableName, strComFields);


                    System.Data.OracleClient.OracleDataAdapter adapter = new System.Data.OracleClient.OracleDataAdapter(cmd);



                    adapter.UpdateCommand = new System.Data.OracleClient.OracleCommandBuilder(adapter).GetUpdateCommand();

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


                using (System.Data.OracleClient.OracleConnection conn = new System.Data.OracleClient.OracleConnection(connstr))


                {
                    conn.Open();
                    System.Data.OracleClient.OracleTransaction tsOprate = conn.BeginTransaction();
                    try
                    {

                        System.Data.OracleClient.OracleCommand cmd = conn.CreateCommand();

                        cmd.Transaction = tsOprate;
                        foreach (DataTable dtTemp in dsTables.Tables)
                        {
                            string strComFields = "*";
                            if (dicDtFields != null && dicDtFields.Count > 0 && dicDtFields.ContainsKey(dtTemp.TableName))
                            {
                                strComFields = dicDtFields[dtTemp.TableName];
                            }
                            cmd.CommandText = GetColumnsNameSql(dtTemp.TableName, strComFields);


                            System.Data.OracleClient.OracleDataAdapter adapter = new System.Data.OracleClient.OracleDataAdapter(cmd);



                            adapter.UpdateCommand = new System.Data.OracleClient.OracleCommandBuilder(adapter).GetUpdateCommand();

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


        public bool DeleteRecords(string cmdText, int intExpectRowNums, params System.Data.OracleClient.OracleParameter[] parameters)
        {
            bool bolFb = false;

            try
            {

                using (System.Data.OracleClient.OracleConnection conn = new System.Data.OracleClient.OracleConnection(connstr))


                {
                    conn.Open();
                    System.Data.OracleClient.OracleTransaction tsOprate = conn.BeginTransaction();
                    try
                    {

                        System.Data.OracleClient.OracleCommand cmd = conn.CreateCommand();

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


                using (System.Data.OracleClient.OracleConnection conn = new System.Data.OracleClient.OracleConnection(connstr))


                {
                    conn.Open();
                    System.Data.OracleClient.OracleTransaction oraOprate = conn.BeginTransaction();
                    try
                    {

                        System.Data.OracleClient.OracleCommand cmd = conn.CreateCommand();

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
            System.Data.OracleClient.OracleParameter cur = new System.Data.OracleClient.OracleParameter(name, value);
            return cur;
        }

        
        public override DbParameter[] ParameterPrepare(DbParameter[] parameters)
        {
            var paras = parameters.Select(a => new System.Data.OracleClient.OracleParameter(a.ParameterName, a.Value.ToString())).ToArray();
            return paras;
        }
    }
}

#endif