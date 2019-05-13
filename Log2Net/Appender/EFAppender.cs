//using Log2Net.Config;
//using Log2Net.Models;
//using Log2Net.Util.DBUtil.EF2DB;
//using Log2Net.Util.DBUtil.Models;
//using System;

//namespace Log2Net.Appender
//{
    
//    internal class EFAppender : BaseAppender
//    {
//        Log_OperateTraceEFDal log_OperateTraceDal = new Log_OperateTraceEFDal(new Log_OperateTraceContext());
//        Log_SystemMonitorEFDal Log_SystemMonitorDal = new Log_SystemMonitorEFDal(new Log_SystemMonitorContext());

//        protected override ExeResEdm WriteLog(Log_OperateTrace model)
//        {
//            if (model == null)
//            {
//                return new ExeResEdm();
//            }
//            var t = log_OperateTraceDal.Insert(model);
//            return t;


//        }

//        protected override ExeResEdm WriteLog(Log_SystemMonitorMQ model)
//        {
//            if (model == null)
//            {
//                return new ExeResEdm();
//            }
//            Log_SystemMonitor dbModel = AutoMapperConfig.GetLog_SystemMonitorModel(model);
//            var t = Log_SystemMonitorDal.Insert(dbModel);
//            return t;

//        }

//    }
//}
