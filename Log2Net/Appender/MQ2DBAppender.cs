using Log2Net.Config;
using Log2Net.Models;
using Log2Net.Util;
using Log2Net.Util.DBUtil.Models;
using System.Threading;

namespace Log2Net.Appender
{
    //使用 RabbitMQ消息队列的Appender
    internal class MQ2DBAppender : Buffer2DBAppender
    {       
 
        protected override string BufferType { get { return "RabbitMQ消息队列"; } }

        #region 队列数据的生产和消费的子类实现

        //将日志发送到队列中
        protected override ExeResEdm SendLogToQueue(object model, MQType mqType)
        {
            if (mqType == MQType.TraceLog)//是操作轨迹类数据
            {
                var item = model as Log_OperateTrace;
                ExeResEdm bOK = RabbitMQManager.Send(item, mqType);
                return bOK;

            }
            else  //是监控类数据
            {
                var item = model as Log_SystemMonitorMQ;
                ExeResEdm bOK = RabbitMQManager.Send(item, mqType);
                return bOK;
            }
        }


        //启动队列服务，检查队列，若有消息则写数据到数据库
        protected override void StartWriteTraceDataService(object o)
        {
            RabbitMQManager.Recive<Log_OperateTrace>(WriteTraceDataToDB, MQType.TraceLog);
        }

        //启动队列服务，检查队列，若有消息则写数据到数据库
        protected override void StartWriteMonitorDataService(object o)
        {
            RabbitMQManager.Recive<Log_SystemMonitorMQ>(WriteMonitorDataToDB, MQType.MonitorLog);
            //LogTraceEdm logModel = new LogTraceEdm() { Detail = "监控日志队列服务启动" };
            //LogApi.WriteLog(logModel);
        }

        #endregion 队列数据的生产和消费的子类实现


    }
}
