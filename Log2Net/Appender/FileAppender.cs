using Log2Net.Models;
using Log2Net.Util.DBUtil.Models;
using static Log2Net.LogInfo.LogCom;

namespace Log2Net.Appender
{
    internal class FileAppender : BaseAppender
    {
        protected override ExeResEdm WriteLog(Log_OperateTrace model)
        {
            return WriteLogToFile(model, MQType.TraceLog);
        }

        protected override ExeResEdm WriteLog(Log_SystemMonitorMQ model)
        {
            return WriteLogToFile(model, MQType.MonitorLog);
        }
    }
}
