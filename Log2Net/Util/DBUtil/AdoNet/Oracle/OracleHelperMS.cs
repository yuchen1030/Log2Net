#if NET

using Log2Net.Util.DBUtil.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;


namespace Log2Net.Util.DBUtil.AdoNet.Oracle
{
    /// <summary>
    /// 使用System.Data.OracleClient实现的oracle 数据库访问类(需安装客户端)，有32位/64位之分
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class OracleHelperMS<T> : OracleHelperBase<T> where T : class
    {
        internal OracleHelperMS(string strConnStr) : base(strConnStr)
        {
            connstr = strConnStr;
        }

        protected override ExeResEdm SqlCMD_DT(string cmdText, Func<DbDataAdapter, int> fun, params DbParameter[] parameters)
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

        protected override DbParameter GetOneDbParameter(string name, object value)
        {
            System.Data.OracleClient.OracleParameter cur = new System.Data.OracleClient.OracleParameter(name, value);
            return cur;
        }
        
        protected override DbParameter[] ParameterPrepare(DbParameter[] parameters)
        {
            var paras = parameters.Select(a => new System.Data.OracleClient.OracleParameter(a.ParameterName, a.Value.ToString())).ToArray();
            return paras;
        }
    }
}

#endif