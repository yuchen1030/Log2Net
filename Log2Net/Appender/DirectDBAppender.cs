using Log2Net.Config;
using Log2Net.Models;
using Log2Net.Util.DBUtil;
using Log2Net.Util.DBUtil.Models;
using System;

namespace Log2Net.Appender
{

    internal class DirectDBAppender : BaseAppender
    {
        
        protected override ExeResEdm WriteLog(Log_OperateTrace model)
        {
            if (model == null)
            {
                return new ExeResEdm();
            }
            var item = model;
            var dbAccessFac = new Log_OperateTraceDBAccessFac().DBAccessFactory();
            AddDBPara<Log_OperateTrace> addDBPara = new AddDBPara<Log_OperateTrace>()
            {
                DBType = DBType.LogTrace,
                Model = model,
                SkipCols = new string[] { "LogId" },
                TableName = "Log_OperateTrace",
            };
            try
            {
                var n = dbAccessFac.Add(addDBPara);
                return n;
            }
            catch (Exception ex)
            {
                ExeResEdm dbResEdm = new ExeResEdm() { ErrCode = 1, ExBody = ex, Module = "WriteLog方法" };
                return dbResEdm;
            }

        }



        protected override ExeResEdm WriteLog(Log_SystemMonitorMQ model)
        {
            if (model == null)
            {
                return new ExeResEdm();
            }
            //  AdoSQLHelper<Log_SystemMonitor> sqlHelper = new AdoSQLHelper<Log_SystemMonitor>();

            var dbAccessFac = new Log_SystemMonitorDBAccessFac().DBAccessFactory();
            Log_SystemMonitor dbModel = AutoMapperConfig.GetLog_SystemMonitorModel(model);

            AddDBPara<Log_SystemMonitor> addDBPara = new AddDBPara<Log_SystemMonitor>()
            {
                DBType = DBType.LogMonitor,
                Model = dbModel,
                SkipCols = new string[] { "LogId" },
                TableName = "Log_SystemMonitor",
            };
            try
            {
                var n = dbAccessFac.Add(addDBPara);
                return n;
            }
            catch (Exception ex)
            {
                ExeResEdm dbResEdm = new ExeResEdm() { ErrCode = 1, ExBody = ex, Module = "WriteLog方法" };
                return dbResEdm;
            }


        }



    }


}
