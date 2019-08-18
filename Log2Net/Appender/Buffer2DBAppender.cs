using Log2Net.Config;
using Log2Net.Models;
using Log2Net.Util;
using Log2Net.Util.DBUtil.Models;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Log2Net.Appender
{

    //使用队列或消息队列写数据库的抽象基类，各子类需要实现生产数据和消费数据的方法
    internal abstract class Buffer2DBAppender : DirectDBAppender
    {
        static readonly object locker = new object();
        static bool flag = false;

        protected abstract string BufferType { get; }

        public Buffer2DBAppender()
        {
            if (!flag)
            {
                lock (locker)
                {
                    if (!flag)
                    {
                        StartMQTask();
                        flag = true;
                    }
                }
            }
        }
        //static Buffer2DBAppender()
        //{
        //    StartMQTask();
        //}

        //开启线程消费队列中的内容
        void StartMQTask()
        {
            //开启线程消费队列中的内容
            ThreadPool.QueueUserWorkItem(StartWriteTraceDataService, null);//启动写trace日志数据服务
            ThreadPool.QueueUserWorkItem(StartWriteMonitorDataService, null);//启动写monitor日志数据服务
            LogTraceEdm logModel = new LogTraceEdm() { Detail = "日志记录的" + BufferType + "消费服务启动" };
            LogApi.WriteMsgToDebugFile(new { msg = logModel.Detail });
            // LogApi.WriteLog(LogLevel.Info, logModel);
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

        //写轨迹数据到MS SQL和InfluxDB
        protected void WriteTraceDataToDB(Log_OperateTrace obj)
        {
            base.WriteLog(obj);
            return;
        }

        //写监控数据到MS SQL和InfluxDB
        protected void WriteMonitorDataToDB(Log_SystemMonitorMQ obj)
        {
            base.WriteLog(obj);
        }


        #region 抽象方法，队列数据的生产和消费

        //将日志发送到队列中
        protected abstract ExeResEdm SendLogToQueue(object model, MQType mqType);

        //启动队列服务，检查队列，若有消息则写数据到数据库
        protected abstract void StartWriteTraceDataService(object o);

        //启动队列服务，检查队列，若有消息则写数据到数据库
        protected abstract void StartWriteMonitorDataService(object o);

        #endregion 抽象方法，队列数据的生产和消费






    }

}
