using Log2Net.LogInfo;
using Log2Net.Models;
using Log2Net.Util.DBUtil.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Log2Net.Util
{

    // 发送消息的服务 
    class RabbitSendMessageService : ReceiveServerBase
    {
        public RabbitSendMessageService(RabbitSendConfigModel config)
            : base(config)
        {

        }

        /// <summary>
        /// 发送消息，泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        //public bool Send<T>(T message)
        //{
        //    string value = SerializerHelper.SerializeToString(message);
        //    return this.Send(value);
        //}
        bool bErrorMQSetting = false;
        static List<string> exList = new List<string>();

        #region 发送消息给队列服务器

        /// <summary>
        /// 发送消息给队列服务器
        /// </summary>
        /// <returns></returns>
        public ExeResEdm Send(string message)
        {
            ExeResEdm exeResEdm = new ExeResEdm() { ErrCode = 1 };
            if (string.IsNullOrWhiteSpace(message) || bErrorMQSetting)
            {
                exeResEdm.ErrMsg = bErrorMQSetting ? "消息队列设置错误" : "消息为空";
                return exeResEdm;
            }
            try
            {
                using (var channel = this.GetConnection().CreateModel())
                {
                    //推送消息
                    byte[] bytes = Encoding.UTF8.GetBytes(message);

                    ////声明一个交换机和队列，然后绑定在一起。 
                    //if (!string.IsNullOrWhiteSpace(this.RabbitConfig.Exchange))
                    //    //使用自定义的路由
                    //    channel.ExchangeDeclare(this.RabbitConfig.Exchange, this.RabbitConfig.ExchangeType.ToString(), this.RabbitConfig.DurableQueue, false, null);
                    ////channel.ExchangeDeclare(this.RabbitConfig.Exchange, this.RabbitConfig.ExchangeType);
                    //else
                    //    //声明消息队列，且为可持久化的  ，如果队列的名称不存在，系统会自动创建，有的话不会覆盖
                    //    channel.QueueDeclare(this.RabbitConfig.QueueName, this.RabbitConfig.DurableQueue, false, false, null);

                    channel.ExchangeDeclare(this.RabbitConfig.Exchange, this.RabbitConfig.ExchangeType.ToString(), this.RabbitConfig.DurableQueue, false, null);
                    channel.QueueDeclare(this.RabbitConfig.QueueName, this.RabbitConfig.DurableQueue, false, false, null);

                    IBasicProperties properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = Convert.ToByte(this.RabbitConfig.DurableMessage ? 2 : 1); //支持持久化数据   
                    channel.QueueBind(this.RabbitConfig.QueueName, RabbitConfig.Exchange, RabbitConfig.RoutingKey);

                    //将详细写入队列
                    if (string.IsNullOrEmpty(this.RabbitConfig.Exchange))
                        //没有配置路由，使用系统默认的路由
                        //推送消息
                        channel.BasicPublish("", this.RabbitConfig.QueueName, properties, bytes);
                    else
                        //推送消息
                        channel.BasicPublish(this.RabbitConfig.Exchange, this.RabbitConfig.RoutingKey, properties, bytes);

                    exeResEdm.ErrCode = 0;
                    return exeResEdm;

                }
            }
            catch (Exception ex)
            {
                //  mqErrCnt++;
                if (ex.Message == "None of the specified endpoints were reachable")
                {
                    bErrorMQSetting = true;
                }
                exeResEdm.ExBody = ex;
                exeResEdm.Module = "RabbitMQHelper.Send<string>方法";
                return exeResEdm;
            }
        }
        #endregion

    }


    // 声明处理接受信息的委托
    delegate void ReceiveMessageDelegate<T>(T value);

    //  接受消息的服务 
    class RabbitReceiveMessageService : ReceiveServerBase
    {
        public RabbitReceiveMessageService(RabbitConfigModel config)
            : base(config)
        {

        }

        #region 接受消息，使用委托进行处理
        /// <summary>
        /// 接受消息，使用委托进行处理，使用QueueingBasicConsumer方法（已过时）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="receiveMethod"></param>
        public ExeResEdm Receive<T>(ReceiveMessageDelegate<T> receiveMethod)
        {
            ExeResEdm exeResEdm = new ExeResEdm() { };
            try
            {
                using (var channel = this.GetConnection().CreateModel())
                {
                    //是否使用路由
                    if (!string.IsNullOrWhiteSpace(this.RabbitConfig.Exchange))
                    {
                        //声明路由
                        //   channel.ExchangeDeclare(this.RabbitConfig.Exchange, this.RabbitConfig.ExchangeType.ToString(), this.RabbitConfig.DurableQueue);

                        //声明队列且与交换机绑定
                        channel.QueueDeclare(this.RabbitConfig.QueueName, this.RabbitConfig.DurableQueue, false, false, null);
                        channel.QueueBind(this.RabbitConfig.QueueName, this.RabbitConfig.Exchange, this.RabbitConfig.RoutingKey);
                    }
                    else
                        channel.QueueDeclare(this.RabbitConfig.QueueName, this.RabbitConfig.DurableQueue, false, false, null);

                    //输入1，那如果接收一个消息，但是没有应答，则客户端不会收到下一个消息
                    channel.BasicQos(0, 1, false);  ///告诉RabbitMQ同一时间给一个消息给消费者  
                    //在队列上定义一个消费者
                    var consumer = new QueueingBasicConsumer(channel);
                    //消费队列，并设置应答模式为程序主动应答
                    channel.BasicConsume(this.RabbitConfig.QueueName, false, consumer);

                    while (true)
                    {
                        //阻塞函数，获取队列中的消息
                        ProcessingResultsEnum processingResult = ProcessingResultsEnum.Retry;
                        ulong deliveryTag = 0;
                        try
                        {
                            //获取信息
                            var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                            deliveryTag = ea.DeliveryTag;
                            byte[] bytes = ea.Body;
                            string str = Encoding.UTF8.GetString(bytes);

                            T v = SerializerHelper.DeserializeToObject<T>(str);
                            receiveMethod(v);
                            processingResult = ProcessingResultsEnum.Accept; //处理成功
                        }
                        catch //(Exception ex)
                        {
                            processingResult = ProcessingResultsEnum.Reject; //系统无法处理的错误
                        }
                        finally
                        {
                            System.Threading.Thread.Sleep(100);//暂停0.1秒，防止CPU爆满的问题

                            switch (processingResult)
                            {
                                case ProcessingResultsEnum.Accept:
                                    //回复确认处理成功
                                    channel.BasicAck(deliveryTag, false);
                                    break;
                                case ProcessingResultsEnum.Retry:
                                    //发生错误了，但是还可以重新提交给队列重新分配
                                    channel.BasicNack(deliveryTag, false, true);
                                    break;
                                case ProcessingResultsEnum.Reject:
                                    //发生严重错误，无法继续进行，这种情况应该写日志或者是发送消息通知管理员
                                    channel.BasicNack(deliveryTag, false, false);

                                    //写日志
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exeResEdm.ErrCode = 1;
                exeResEdm.ExBody = ex;
                exeResEdm.Module = "RabbitMQHelper.Receive方法";
                LogCom.WriteExceptToFile(ex, "RabbitMQHelper.Receive");
            }
            return exeResEdm;
        }


        /// <summary>
        /// 接受消息，使用委托进行处理，使用 EventingBasicConsumer 方法，3.6.12版本服务器接收出错
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="receiveMethod"></param>
        public void Receive2<T>(ReceiveMessageDelegate<T> receiveMethod)
        {
            try
            {
                using (var channel = this.GetConnection().CreateModel())
                {
                    //是否使用路由
                    if (!string.IsNullOrWhiteSpace(this.RabbitConfig.Exchange))
                    {
                        //声明路由
                        //   channel.ExchangeDeclare(this.RabbitConfig.Exchange, this.RabbitConfig.ExchangeType.ToString(), this.RabbitConfig.DurableQueue);

                        //声明队列且与交换机绑定
                        channel.QueueDeclare(this.RabbitConfig.QueueName, this.RabbitConfig.DurableQueue, false, false, null);
                        // channel.QueueBind(this.RabbitConfig.QueueName, this.RabbitConfig.Exchange, this.RabbitConfig.RoutingKey);
                    }
                    else
                        channel.QueueDeclare(this.RabbitConfig.QueueName, this.RabbitConfig.DurableQueue, false, false, null);

                    //输入1，那如果接收一个消息，但是没有应答，则客户端不会收到下一个消息
                    channel.BasicQos(0, 1, false);  ///告诉RabbitMQ同一时间给一个消息给消费者  
                    //在队列上定义一个消费者
                    var consumer = new EventingBasicConsumer(channel);

                    //消费队列，并设置应答模式为程序主动应答，oAck设置false,告诉broker，发送消息之后，消息暂时不要删除，等消费者处理完成再说
                    channel.BasicConsume(this.RabbitConfig.QueueName, false, consumer);

                    consumer.Received += (model, ea) =>
                    {
                        ProcessingResultsEnum processingResult = ProcessingResultsEnum.Retry;
                        ulong deliveryTag = 0;
                        try
                        {
                            //获取信息
                            deliveryTag = ea.DeliveryTag;
                            byte[] bytes = ea.Body;
                            string str = Encoding.UTF8.GetString(bytes);

                            T v = SerializerHelper.DeserializeToObject<T>(str);
                            receiveMethod(v);
                            processingResult = ProcessingResultsEnum.Accept; //处理成功
                                                                             //   channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false); //处理完成，告诉Broker可以服务端可以删除消息，分配新的消息过来
                        }
                        catch (Exception ex)
                        {
                            LogCom.WriteExceptToFile(ex, "RabbitMQHelper.Receive");
                            processingResult = ProcessingResultsEnum.Reject; //系统无法处理的错误
                        }
                        finally
                        {
                            System.Threading.Thread.Sleep(100);//暂停0.1秒，防止CPU爆满的问题

                            switch (processingResult)
                            {
                                case ProcessingResultsEnum.Accept:
                                    //回复确认处理成功
                                    channel.BasicAck(deliveryTag, false);
                                    break;
                                case ProcessingResultsEnum.Retry:
                                    //发生错误了，但是还可以重新提交给队列重新分配
                                    channel.BasicNack(deliveryTag, false, true);
                                    break;
                                case ProcessingResultsEnum.Reject:
                                    //发生严重错误，无法继续进行，这种情况应该写日志或者是发送消息通知管理员
                                    channel.BasicNack(deliveryTag, false, false);

                                    //写日志
                                    break;
                            }
                        }
                    };

                }
            }
            catch (Exception ex)
            {
                LogCom.WriteExceptToFile(ex, "RabbitMQHelper.Receive");
            }
        }


        #endregion


        #region 获取单条消息
        /// <summary>
        /// 获取单条消息
        /// </summary>
        /// <param name="basicGetMethod">回调函数</param>
        /// <param name="data">传过来，再传回去</param>
        public void BasicGet(ReceiveMessageDelegate<Tuple<bool, string, Dictionary<string, object>>> basicGetMethod, Dictionary<string, object> data = null)
        {
            try
            {
                using (var channel = this.GetConnection().CreateModel())
                {
                    BasicGetResult res = channel.BasicGet(RabbitConfig.QueueName, false);
                    if (res != null)
                    {
                        //普通使用方式BasicGet
                        //noAck = true，不需要回复，接收到消息后，queue上的消息就会清除
                        //noAck = false，需要回复，接收到消息后，queue上的消息不会被清除，直到调用channel.basicAck(deliveryTag, false); queue上的消息才会被清除 而且，在当前连接断开以前，其它客户端将不能收到此queue上的消息

                        IBasicProperties props = res.BasicProperties;
                        bool t = res.Redelivered;
                        t = true;
                        string result = Encoding.UTF8.GetString(res.Body);
                        channel.BasicAck(res.DeliveryTag, false);
                        basicGetMethod(new Tuple<bool, string, Dictionary<string, object>>(true, result, data));
                    }
                    else
                    {
                        basicGetMethod(new Tuple<bool, string, Dictionary<string, object>>(false, "未找到所需数据", data));
                    }
                }
            }
            catch (Exception ex)
            {
                //  mqErrCnt++;
                LogCom.WriteExceptToFile(ex, "RabbitMQHelper.BasicGet");
            }
        }
        #endregion

    }



    // RabbitMQ基础服务类
    class RabbitBaseService
    {
        /// <summary>
        /// 获取队列服务器的连接对象
        /// </summary>
        /// <param name="ip">服务器ip</param>
        ///  <param name="port">服务器的端口</param>
        /// <param name="userName">登录账户</param>
        /// <param name="password">登录密码</param>
        /// <param name="virtualHost">虚拟主机</param>
        /// <param name="heartbeat">心跳检测时间</param>
        /// <returns></returns>
        public static IConnection GetConnection(string ip, int port, string userName, string password, string virtualHost, ushort heartbeat)
        {
            try
            {
                ConnectionFactory cf = new ConnectionFactory()
                {
                  //  HostName = ip,
                    Port = port,
                    Endpoint = new AmqpTcpEndpoint(new Uri("amqp://" + ip + "/")),
                    UserName = userName,
                    Password = password,
                    VirtualHost = virtualHost,
                    RequestedHeartbeat = heartbeat,
                   // AutomaticRecoveryEnabled = true
                };
                return cf.CreateConnection();
            }
            catch (Exception ex)
            {
                LogCom.WriteExceptToFile(ex, "RabbitMQHelper.GetConnection");
                return null;
            }

        }
    }

    // RabbitMQ 发送方的配置 
    class RabbitSendConfigModel : RabbitConfigModel
    {
    }

    // RabbitMQ 接收方的配置  
    class RabbitReceiveConfigModel : RabbitConfigModel
    {

    }

    abstract class ReceiveServerBase
    {
        /// <summary>
        /// 服务器配置
        /// </summary>
        public RabbitConfigModel RabbitConfig { get; set; }

        public static IConnection Connection;

        public ReceiveServerBase(RabbitConfigModel config)
        {
            this.RabbitConfig = config;
        }

        /// <summary>
        /// 获取队列服务器的链接
        /// </summary>
        /// <returns></returns>
        public IConnection GetConnection()
        {
            if (Connection == null)
            {
                Connection = RabbitBaseService.GetConnection(this.RabbitConfig.IP, this.RabbitConfig.Port, this.RabbitConfig.UserName, this.RabbitConfig.Password, this.RabbitConfig.VirtualHost, 60);
            }
            if (Connection == null)
            {
                // mqErrCnt++;
                throw new Exception("创建消息队列失败(" + RabbitConfig.IP + ":" + RabbitConfig.Port + ";" + RabbitConfig.UserName + ";" + RabbitConfig.Password + ")");
            }
            return Connection;
        }

    }


    #region RabbitMQ Models
    // RabbitMQ服务器的配置项
    class RabbitConfigModel
    {
        /// <summary>
        /// 服务器IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 服务器端口，默认是 5672
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 路由名称
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 路由的类型枚举 当type = "fanout"时不需要指定routing key，设置了也没有用.
        /// </summary>
        public ExchangeTypeEnum ExchangeType { get; set; }

        /// <summary>
        /// 路由的关键字
        /// </summary>
        public string RoutingKey { get; set; }

        /// <summary>
        /// 登录用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 虚拟主机名称
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// 是否持久化该队列
        /// </summary>
        public bool DurableQueue { get; set; }

        /// <summary>
        /// 是否持久化队列中的消息
        /// </summary>
        public bool DurableMessage { get; set; }

        ///// <summary>
        ///// 管理后台链接地址
        ///// </summary>
        //public string HostUrl => $"http://{IP}:15672"; //属性中使用Lambda表达式，C#6.0的语法糖  //   public string HostUrl222 => System.Web.Configuration.WebConfigurationManager.AppSettings[""];

        ///// <summary>
        ///// 查询Exchanges信息接口
        ///// </summary>
        //public string ExchangesApi => HostUrl + "/api/exchanges";

        ///// <summary>
        ///// 查询queues信息接口
        ///// </summary>
        //public string QueuesApi => HostUrl + "/api/queues";

        ///// <summary>
        ///// 查询Bingdings信息接口
        ///// </summary>
        //public string BingdingsApi => HostUrl + "/api/bindings";

    }

    // RabbitMQ 路由类型
    enum ExchangeTypeEnum
    {
        /*
       不处理路由键。你只需要简单的将队列绑定到交换机上。一个发送到交换机的消息都会被转发到与该交换机绑定的所有队列上。
       很像子网广播，每台子网内的主机都获得了一份复制的消息。Fanout交换机转发消息是最快的。
       */
        fanout = 1,

        /*
        处理路由键。需要将一个队列绑定到交换机上，要求该消息与一个特定的路由键完全匹配
        。这是一个完整的匹配。如果一个队列绑定到该交换机上要求路由键 “dog”，
        则只有被标记为“dog”的消息才被转发，不会转发dog.puppy，也不会转发dog.guard，只会转发dog。 
      */
        direct = 2,

        /*将路由键和某模式进行匹配。此时队列需要绑定要一个模式上。
        符号“#”匹配一个或多个词，符号“*”匹配不多不少一个词。
        因此“audit.#”能够匹配到“audit.irs.corporate”，但是“audit.*” 只会匹配到“audit.irs”
        */
        topic = 3,

        header = 4
    }

    // 接受消息以后针对消息的最终处理结果，用于通知RabbitMQ的处理状态
    enum ProcessingResultsEnum
    {
        /// <summary>
        /// 处理成功
        /// </summary>
        Accept,

        /// <summary>
        /// 可以重试的错误
        /// </summary>
        Retry,

        /// <summary>
        /// 无需重试的错误
        /// </summary>
        Reject,
    }



    #endregion RabbitMQ Models



}
