
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

namespace Log2Net.Util.DBUtil.AdoNet.Oracle
{
    /// <summary>
    /// 使用Oracle.ManagedDataAccess.Client实现的oracle 数据库访问类(无需安装客户端)，无32位/64位之分，但仅支持Oracle10g及以上
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class OracleHelper<T> : OracleHelperBase<T> where T : class
    {
        internal OracleHelper(string strConnStr) : base(strConnStr)
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

        protected override DbParameter GetOneDbParameter(string name, object value)
        {
            OracleParameter cur = new OracleParameter(name, value);
            return cur;
        }

        protected override DbParameter[] ParameterPrepare(DbParameter[] parameters)
        {
            var paras = parameters.Select(a => new OracleParameter(a.ParameterName, a.Value)).ToArray();
            return paras;
        }

    }

}
