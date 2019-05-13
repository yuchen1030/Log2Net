using Log2Net.Config;
using Log2Net.Models;
using Log2Net.Util;
using Log2Net.Util.DBUtil.Models;
using System.Threading;

namespace Log2Net.Appender
{
    internal class MQ2DBAppender : DirectDBAppender
    {
        static MQ2DBAppender()
        {
            new MQ2DBAppender().StartMQTask();
        }


        //开启线程消费队列中的内容
        void StartMQTask()
        {
            //开启线程消费队列中的内容
            ThreadPool.QueueUserWorkItem(StartWriteTraceDataService, null);//启动写trace日志数据服务
            ThreadPool.QueueUserWorkItem(StartWriteMonitorDataService, null);//启动写monitor日志数据服务
        }

        //将Log_OperateTraceR内容写到队列中
        protected override ExeResEdm WriteLog(Log_OperateTrace model)
        {
            return SendLogToQueue(model, MQType.TraceLog);
        }

        //将Log_SystemMonitorR内容写到队列中
        protected override ExeResEdm WriteLog(Log_SystemMonitorMQ model)
        {
            return SendLogToQueue(model, MQType.MonitorLog);
        }


        //将日志发送到队列中
        ExeResEdm SendLogToQueue(object model, MQType mqType)
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



        #region 队列数据消费
        //启动队列服务，检查队列，若有消息则写数据到数据库
        void StartWriteTraceDataService(object o)
        {
            RabbitMQManager.Recive<Log_OperateTrace>(WriteTraceDataToDB, MQType.TraceLog);
            //Log_OperateTraceBllEdm logModel = new Log_OperateTraceBllEdm() { Detail = "轨迹日志队列服务启动" };
            //LogApi.WriteLog(logModel);
        }

        //启动队列服务，检查队列，若有消息则写数据到数据库
        void StartWriteMonitorDataService(object o)
        {
            RabbitMQManager.Recive<Log_SystemMonitorMQ>(WriteMonitorDataToDB, MQType.MonitorLog);
            //Log_OperateTraceBllEdm logModel = new Log_OperateTraceBllEdm() { Detail = "监控日志队列服务启动" };
            //LogApi.WriteLog(logModel);
        }

        //写轨迹数据到MS SQL和InfluxDB
        void WriteTraceDataToDB(Log_OperateTrace obj)
        {
            base.WriteLog(obj);
            return;
        }

        //写监控数据到MS SQL和InfluxDB
        void WriteMonitorDataToDB(Log_SystemMonitorMQ obj)
        {
            base.WriteLog(obj);
            return;
        }

        #endregion 队列数据消费



    }
}
