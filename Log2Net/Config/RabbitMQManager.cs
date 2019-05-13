using Log2Net.LogInfo;
using Log2Net.Models;
using Log2Net.Util;
using Log2Net.Util.DBUtil.Models;
using System;
using System.Collections.Generic;

namespace Log2Net.Config
{
    internal class RabbitMQManager
    {
        static string IP = "127.0.0.1";
        static string UserName = "guest";
        static string Password = "guest";
        static int Port = 5672;
        static bool bSendMsgUseMQ = true;

        static Dictionary<MQType, string> queNameDic = new Dictionary<MQType, string>() {
           {MQType.TraceLog,"Log2NetCompTraceLogQue"},        //操作轨迹类日志队列名称
            {MQType.MonitorLog,"Log2NetCompMonitorLogQue"}, //系统监控类日志队列名称

                //        {MQType.TraceLog,"TraceLogQue"},        //操作轨迹类日志队列名称
              //{MQType.MonitorLog,"MonitorLogQue"}, //系统监控类日志队列名称
        };

        static Dictionary<MQType, RabbitSendMessageService> RabbitSendDic = new Dictionary<MQType, RabbitSendMessageService>();


        //  static Dictionary<MQType, RabbitReceiveConfigModel> RabbitReceiveConfigDic = new Dictionary<MQType, RabbitReceiveConfigModel>();

        static Dictionary<MQType, RabbitReceiveMessageService> RabbitReceiveDic = new Dictionary<MQType, RabbitReceiveMessageService>();



        static RabbitMQManager()
        {
            GetInitConfig();

            //发送队列初始化
            foreach (var item in queNameDic)
            {
                var queType = item.Key;
                var queueName = item.Value;
                RabbitSendConfigModel RabbitSendConfig = new RabbitSendConfigModel
                {
                    IP = IP,
                    UserName = UserName,
                    Password = Password,
                    Port = Port,
                    VirtualHost = "/",
                    DurableQueue = true,
                    // QueueName = name,
                    Exchange = "Exchange",
                    ExchangeType = ExchangeTypeEnum.direct,
                    DurableMessage = true,
                    //RoutingKey = name + "RoutingKey",
                };
                RabbitSendConfig.QueueName = queueName;
                RabbitSendConfig.RoutingKey = queueName;
                RabbitSendDic.Add(queType, new RabbitSendMessageService(RabbitSendConfig));
            }

            //接收队列初始化
            foreach (var item in queNameDic)
            {
                var queType = item.Key;
                var queueName = item.Value;
                RabbitReceiveConfigModel RabbitReceiveConfig = new RabbitReceiveConfigModel
                {
                    IP = IP,
                    UserName = UserName,
                    Password = Password,
                    Port = Port,
                    VirtualHost = "/",
                    DurableQueue = true,
                    // QueueName = name,
                    Exchange = "Exchange",
                    ExchangeType = ExchangeTypeEnum.direct,
                    DurableMessage = true,
                    //RoutingKey = name + "RoutingKey",
                };

                RabbitReceiveConfig.QueueName = queueName;
                RabbitReceiveConfig.RoutingKey = queueName;
                RabbitReceiveDic.Add(queType, new RabbitReceiveMessageService(RabbitReceiveConfig));

            }

        }

        #region RabbitMQ初始化配置（静态）
        static void GetInitConfig()
        {
            string serverStr = Log2NetConfig.GetConfigVal("RabbitMQServer_Log"); 
            serverStr = string.IsNullOrEmpty(serverStr) ? "127.0.0.1:5672;oawxAdmin1;admin123.123" : serverStr;
            if (string.IsNullOrEmpty(serverStr) || !serverStr.Contains(";") || !serverStr.Contains(":"))
            {
                return;
            }
            var arrs = serverStr.Split(';');
            UserName = arrs[1];
            Password = arrs[2];
            var ips = arrs[0].Split(':');
            IP = ips[0];
            Port = Convert.ToInt32(ips[1]);
        }

        #endregion RabbitMQ初始化配置（静态）


        #region 队列操作

        static object mqObj = new object();


        //队列消息生产
        internal static ExeResEdm Send<T>(T model, MQType mqType)
        {
            lock (mqObj)
            {
                if (!bSendMsgUseMQ)
                {
                    return new ExeResEdm();
                }
                string value = SerializerHelper.SerializeToString(model);
                var tt = RabbitSendDic[mqType];
                try
                {
                    return tt.Send(value);
                }
                catch (Exception ex)
                {
                    LogCom.WriteExceptToFile(ex, "RabbitMQHelper.Send<T>");
                    return new ExeResEdm() { ErrCode = 1, ExBody = ex, Module = "RabbitMQHelper.Send<T>" };
                }
            }

        }

        //队列消息消费
        public static void Recive<T>(ReceiveMessageDelegate<T> receiveMethod, MQType mqType)
        {
            if (!bSendMsgUseMQ)
            {
                return;
            }
            RabbitReceiveDic[mqType].Receive(receiveMethod);
        }

        static object SetReceivefigModelObj = new object();
        static object SetSendfigModelObj = new object();

        #endregion 队列操作

    }


}
