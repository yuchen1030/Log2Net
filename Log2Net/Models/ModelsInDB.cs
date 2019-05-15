using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace Log2Net.Models
{

    /// <summary>
    /// 操作轨迹实体，写到数据库，队列存储时使用
    /// </summary>
    [Serializable]
    public class Log_OperateTrace
    {
        public long Id { get; set; }
        [IsInfluxTags]
        public DateTime LogTime { get; set; }
        public string UserID { get; set; }//用户工号
        public string UserName { get; set; }//用户姓名
        [IsInfluxTags]
        public LogType LogType { get; set; } //日志类型
        [IsInfluxTags]
        public SysCategory SystemID { get; set; }//业务系统编号
        [IsInfluxTags]
        public string ServerHost { get; set; }//服务器名称
        public string ServerIP { get; set; }//服务器IP地址
        public string ClientHost { get; set; }//客户端名称
        public string ClientIP { get; set; }//客户端IP地址
        public string TabOrModu { get; set; }//表名或模块名称
        public string Detail { get; set; } //日志内容
        public string Remark { get; set; } //其他信息

    }


    /// <summary>
    /// 系统监控信息实体,写到数据库中用
    /// </summary>
    [Serializable]
    public class Log_SystemMonitor //: Log_SystemMonitorBase  //使用Log_SystemMonitorBase基类将导致EF创建DB时基类字段在后面，使用FluentApi的HasColumnOrder时将需要设置所有字段
    {
        public long Id { get; set; }
        [IsInfluxTags]
        public DateTime LogTime { get; set; }
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
   
        public /*SqlXml*/ string PageViewNum { get; set; }//重要页面访问量    

        public /*SqlXml*/ string DiskSpace { get; set; }//磁盘使用情况      
        public string Remark { get; set; } //其他信息
    }





}
