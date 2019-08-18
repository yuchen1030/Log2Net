using System;

namespace Log2Net.Models
{
    public class Log_SystemMonitorBase
    {
        public long Id { get; set; }
        [IsInfluxTags]
        public string LogTime { get; set; }
        [IsInfluxTags]
        public SysCategory SystemID { get; set; }//业务系统编号
        [IsInfluxTags]
        public string ServerHost { get; set; }//服务器名称
        public string ServerIP { get; set; }//服务器IP地址
        public int OnlineCnt { get; set; }    //在线人数
        public int AllVisitors { get; set; }//历史访客
        public double RunHours { get; set; }//运行时长
        public double CpuUsage { get; set; }//cpu使用率
        public double MemoryUsage { get; set; }//内存使用率
        public int ProcessNum { get; set; }//进程总数
        public int ThreadNum { get; set; }//进程总数 -----------
        public int CurProcThreadNum { get; set; }//当前进程的线程数 -----------
        public double CurProcMem { get; set; }//当前进程内存（单位M） -----------
        public double CurProcMemUse { get; set; }//当前进程内存使用率 -----------
        public double CurProcCpuUse { get; set; }//当前进程cpu使用率 -----------
        public double CurSubProcMem { get; set; }//当前程序内存（单位M） -----------
        public string Remark { get; set; } //其他信息

    }



    //标记某字段是tag类型（influxdb中使用）
    public class IsInfluxTags : System.Attribute
    {

    }


    public enum CacheConst
    {
        webName,
        serverIP,
        serverHost,
        systemName,
        firstRequest,
        userCfgInCode,
    }

    public enum MQType
    {
        TraceLog,
        MonitorLog,
        TraceLogEx,
    }


    public enum DBType
    {
        LogTrace,
        LogMonitor,
    }

    /// <summary>
    ///类据库类型
    /// </summary>
    public enum DataBaseType
    {
        NoSelect = 0,
        SqlServer = 1,//realize by zz on 20190407
        Oracle = 2, //realize by zz on 20190407
        MySql = 3,//realize by zz on 20190407
        Access = 4,//realize by xxx on yyyyMMdd
        PostgreSQL = 5,//realize by xxx on yyyyMMdd
        SQLite = 6,//realize by xxx on yyyyMMdd
    }


    /// <summary>
    /// 访问数据库的方式
    /// </summary>
    public enum DBAccessType
    {
        NoSelect = 0,
        ADONET = 1,
        EF = 2,
        NH = 3,//TBD
    }

    public enum AppKey
    {
        OnLineUserCnt,
        AllVisitorCnt,
    }

    //文件类型枚举
    public enum FileType
    {
        Backup,
        File,
        Debug,
        Info,
    }

    //磁盘使用情况实体
    public class DiskSpaceEdm
    {
        public string DiscName { get; set; }//盘符名称
        public double Free { get; set; }//可用大小
        public double Rate { get; set; }//使用率
    }


    //队列中的SQLXML类型实体
    public class SQLXMLEdm    //MQPage
    {
        public bool IsNull { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// 消息队列中的存储的系统监控信息实体
    /// </summary>
    [Serializable]
    public class Log_SystemMonitorMQ : Log_SystemMonitorBase 
    {
        public SQLXMLEdm PageViewNumMQ { get; set; }//重要页面访问量           /MQPage     
        public SQLXMLEdm DiskSpaceMQ { get; set; }//磁盘使用情况 

    }






}
