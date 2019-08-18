using Log2Net.Config;
using Log2Net.Models;
using Log2Net.Util;
using Log2Net.Util.DBUtil.Models;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Log2Net.Appender
{

    //使用Queue队列的Appender
    internal class Queue2DBAppender : Buffer2DBAppender
    {
        static ConcurrentQueue<Log_OperateTrace> OperateLogQueue = new ConcurrentQueue<Log_OperateTrace>();
        static ConcurrentQueue<Log_SystemMonitorMQ> MonitorLogQueue = new ConcurrentQueue<Log_SystemMonitorMQ>();

        protected override string BufferType { get { return "Queue队列"; } }

        #region 队列数据的生产和消费的子类实现

        //将日志发送到队列中
        protected override ExeResEdm SendLogToQueue(object model, MQType mqType)
        {
            ExeResEdm exeResEdm = new ExeResEdm() { };
            if (mqType == MQType.TraceLog)//是操作轨迹类数据
            {
                var item = model as Log_OperateTrace;
                OperateLogQueue.Enqueue(item);
                return exeResEdm;
            }
            else  //是监控类数据
            {
                var item = model as Log_SystemMonitorMQ;
                MonitorLogQueue.Enqueue(item);
                return exeResEdm;
            }
        }

        //启动队列服务，检查队列，若有消息则写数据到数据库
        protected override void StartWriteTraceDataService(object o)
        {
            DataConsume(WriteTraceDataToDB, OperateLogQueue);
        }

        //启动队列服务，检查队列，若有消息则写数据到数据库
        protected override void StartWriteMonitorDataService(object o)
        {
            DataConsume(WriteMonitorDataToDB, MonitorLogQueue);
        }

        void DataConsume<T>(Action<T> taskFun, ConcurrentQueue<T> queue)
        {
            while (true)
            {
                T cur = default(T);
                if (queue.Count > 0)
                {
                    if (queue.TryDequeue(out cur))
                    {
                        taskFun(cur);
                    }
                }
                else
                {
                    Thread.Sleep(5000);
                }
            }
        }


        #endregion 队列数据的生产和消费的子类实现



    }


}
