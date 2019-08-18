using Log2Net.Models;
using Log2Net.Util.DBUtil.Dal;
using Log2Net.Util.DBUtil.EF2DB;
using System;
using Log2Net.LogInfo;

namespace Log2Net.Util.DBUtil
{
    //数据库访问方式：1、ADO.NET ;2、EF ;3、NH
    internal abstract class DBAccessFac<T> where T : class
    {
        static readonly object locker = new object();
        static IDBAccessDal<T> dBAccess = null;
        public IDBAccessDal<T> DBAccessFactory()
        {
            if (dBAccess == null)
            {
                lock (locker)
                {
                    if (dBAccess == null)
                    {             
                        DBAccessType dBAccessType = AppConfig.GetFinalConfig("DBAccessTypeKey", DBAccessType.ADONET, LogApi.GetDBAccessType());
                        string exMsg = "";
                        try { dBAccess = GetDalByDBAccessType(dBAccessType); }
                        catch (Exception ex) { exMsg = ", " + ex.Message; }

                        string msg = typeof(T).Name + "的数据库访问方式为【" + dBAccessType.ToString() + "】" + exMsg;
                        LogCom.WriteModelToFileForDebug(new { 内容 = msg });

                    }
                }
            }
            return dBAccess;

        }

        public abstract IDBAccessDal<T> GetDalByDBAccessType(DBAccessType dbAccessType);
    }


    internal class Log_OperateTraceDBAccessFac : DBAccessFac<Log_OperateTrace>
    {

        public override IDBAccessDal<Log_OperateTrace> GetDalByDBAccessType(DBAccessType dbAccessType)
        {
            if (dbAccessType == DBAccessType.EF)
            {
                Log_OperateTraceEFDal log_OperateTraceDal = new Log_OperateTraceEFDal(new Log_OperateTraceContext());
                return log_OperateTraceDal;
            }
            else if (dbAccessType == DBAccessType.NH)
            {
                throw new Exception("Not define dal methods when DBAccessType = NH");
            }
            else
            {
                return new Log_OperateTraceAdoDal();
            }

        }
    }


    internal class Log_SystemMonitorDBAccessFac : DBAccessFac<Log_SystemMonitor>
    {
        public override IDBAccessDal<Log_SystemMonitor> GetDalByDBAccessType(DBAccessType dbAccessType)
        {
            if (dbAccessType == DBAccessType.EF)
            {
                Log_SystemMonitorEFDal Log_SystemMonitorDal = new Log_SystemMonitorEFDal(new Log_SystemMonitorContext());
                return Log_SystemMonitorDal;
            }
            else if (dbAccessType == DBAccessType.NH)
            {
                throw new Exception("Not define dal methods when DBAccessType = NH");
            }
            else
            {
                return new Log_SystemMonitorAdoDal();
            }


        }
    }








}
