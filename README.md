Log2Net是一个用于收集日志到数据库或文件的组件，支持.NET和.NetCore平台。

此组件自动收集系统的运行日志（服务器运行情况、在线人数等）、异常日志。程序员还可以添加自定义日志。

该组件支持.NET平台和.NETCore平台，支持将日志写入到文本文件、SQL Server、Oracle、MySQL，可以方便地扩展到其他数据库。

该组件支持直接写到数据库、通过普通队列写到数据库、通过Rabbit消息队列写到数据库三种方式。读写数据库支持ADO.Net方式和EF两种方式，可以方便地扩展到NHibernate/SqlSugar/Dapper等其他方式。

相关博文： https://www.cnblogs.com/yuchen1030/p/10992259.html 。

Log2NET is a tool for collecting log to databases or files for .NET and .NETCore.

This component automatically collects the system's running logs (server operation, online statistics, etc.), exception logs. programmers can also add custom logs.

This component supports.NET platform and.NETCore platform, supports file, sql server, oracle, mysql, and can be easily extended to other databases.

This component supports writing directly to the database , through queues to the database, through rabbitmq to the database. Read-write database supports ADO. Net mode and EF mode, which can be easily extended to other modes such as NHibernate/SqlSugar/Dapper.
