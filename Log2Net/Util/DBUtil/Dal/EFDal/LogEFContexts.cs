using Log2Net.Models;


#if NET
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
#else
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Oracle.EntityFrameworkCore;  //ORACLE 官方，仅支持11g以上，不支持11g
#endif


namespace Log2Net.Util.DBUtil.EF2DB
{


#if NET


    internal class Log_OperateTraceInitializer : CreateDatabaseIfNotExists<Log_OperateTraceContext>
    {
    }

    internal class Log_SystemMonitorInitializer : CreateDatabaseIfNotExists<Log_SystemMonitorContext>
    {
    }

    internal class Log_OperateTraceContext : DbContext
    {
        public Log_OperateTraceContext() : base("name=" + ComDBFun.GetConnectionStringKey(DBType.LogTrace))
        {

        }

        public virtual DbSet<Log_OperateTrace> Log_OperateTrace { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var dbGeneral = ComDBFun.GetDBGeneralInfo(DBType.LogTrace);
            //  if (dbGeneral.DataBaseType != DataBaseType.Oracle)
            {
                Database.SetInitializer(new Log_OperateTraceInitializer());//oracle 不建议使用EF，会导致字段名和数据库名必须加引号
            }

            modelBuilder.Configurations.Add(new Log_OperateTraceMap());
            //   modelBuilder.Entity<Log_OperateTrace>().HasKey(s => s.Id);

            if (!string.IsNullOrEmpty(dbGeneral.SchemaName))
            {
                modelBuilder.HasDefaultSchema(dbGeneral.SchemaName.ToUpper());
            }
            base.OnModelCreating(modelBuilder);
        }

        class Log_OperateTraceMap : EntityTypeConfiguration<Log_OperateTrace>
        {
            public Log_OperateTraceMap()
            {
                ToTable("Log_OperateTrace");
                HasKey(m => m.Id);
                Property(m => m.LogTime).IsRequired();//.HasColumnType("datetime"); 
                Property(m => m.UserID).HasMaxLength(100);
                Property(m => m.UserName).HasMaxLength(200);
                Property(m => m.ServerIP).HasMaxLength(20).IsRequired();
                Property(m => m.ServerHost).HasMaxLength(40).IsRequired();
                Property(m => m.ClientHost).HasMaxLength(40).IsRequired();
                Property(m => m.ClientIP).HasMaxLength(20).IsRequired();
                Property(m => m.TabOrModu).HasMaxLength(100);
                Property(m => m.Detail).HasMaxLength(2000).IsRequired();
                Property(m => m.Remark).HasMaxLength(2000);
            }
        }


    }

    internal class Log_SystemMonitorContext : DbContext
    {
        public Log_SystemMonitorContext() : base("name=" + ComDBFun.GetConnectionStringKey(DBType.LogMonitor))
        {

        }

        public virtual DbSet<Log_SystemMonitor> Log_SystemMonitor { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var dbGeneral = ComDBFun.GetDBGeneralInfo(DBType.LogMonitor);
            //   if (dbGeneral.DataBaseType != DataBaseType.Oracle)
            {
                Database.SetInitializer(new Log_SystemMonitorInitializer());
            }

            modelBuilder.Configurations.Add(new Log_SystemMonitorMap());

            //  modelBuilder.Entity<Log_SystemMonitor>().Property(a => a.DiskSpace).HasColumnType("xml"); //非空类型才能用作泛型参数

            if (!string.IsNullOrEmpty(dbGeneral.SchemaName))
            {
                modelBuilder.HasDefaultSchema(dbGeneral.SchemaName.ToUpper());
            }
            base.OnModelCreating(modelBuilder);
        }



        class Log_SystemMonitorMap : EntityTypeConfiguration<Log_SystemMonitor>
        {
            public Log_SystemMonitorMap()
            {
                ToTable("Log_SystemMonitor");
                HasKey(m => m.Id);
                Property(m => m.LogTime).IsRequired();//.HasColumnType("datetime"); 
                Property(m => m.ServerIP).HasMaxLength(20).IsRequired();
                Property(m => m.ServerHost).HasMaxLength(40).IsRequired();
                Property(m => m.Remark).HasMaxLength(2000);
                Property(m => m.PageViewNum).HasMaxLength(2000);//.HasColumnOrder(11);
                Property(m => m.DiskSpace).HasMaxLength(2000);//.HasColumnOrder(12);

                //  HasOptional(a => a.PageViewNum);
                //  HasOptional(a => a.DiskSpace);             
            }
        }


    }


#else

    internal class Log_OperateTraceContext : DbContext
    {     
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbGeneral = ComDBFun.GetDBGeneralInfo(DBType.LogTrace);
            if (dbGeneral.DataBaseType == DataBaseType.SqlServer)
            {
                optionsBuilder.UseSqlServer(ComDBFun.GetConnectionString(DBType.LogTrace));
            }
            else if (dbGeneral.DataBaseType == DataBaseType.Oracle)
            {

                optionsBuilder.UseOracle(ComDBFun.GetConnectionString(DBType.LogTrace));
            }
            else if (dbGeneral.DataBaseType == DataBaseType.MySql)
            {
                optionsBuilder.UseMySQL(ComDBFun.GetConnectionString(DBType.LogTrace));
            }


            base.OnConfiguring(optionsBuilder);
        }

        //public Log_OperateTraceContext(DbContextOptions<Log_OperateTraceContext> options) :
        //   // base(options)
        //   base(new DbContextOptionsBuilder().UseSqlServer(ComDBFun.GetConnectionString(DBType.LogTrace)).Options)
        //{

        //}

        public virtual DbSet<Log_OperateTrace> Log_OperateTrace { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var dbGeneral = ComDBFun.GetDBGeneralInfo(DBType.LogTrace);
            //  if (dbGeneral.DataBaseType != DataBaseType.Oracle)
            {
                //  Database.SetInitializer(new Log_OperateTraceInitializer());//oracle 不建议使用EF，会导致字段名和数据库名必须加引号
            }

            modelBuilder.ApplyConfiguration(new Log_OperateTraceMap());
            //var typesToRegister = Assembly.GetExecutingAssembly().GetTypes().Where(q => q.GetInterface(typeof(IEntityTypeConfiguration<>).FullName) != null);
            //foreach (var type in typesToRegister)
            //{
            //    dynamic configurationInstance = Activator.CreateInstance(type);
            //    modelBuilder.ApplyConfiguration(configurationInstance);
            //}


            if (!string.IsNullOrEmpty(dbGeneral.SchemaName))
            {
                modelBuilder.HasDefaultSchema(dbGeneral.SchemaName.ToUpper());
            }
            base.OnModelCreating(modelBuilder);
        }

        class Log_OperateTraceMap : IEntityTypeConfiguration<Log_OperateTrace>
        {
            public void Configure(EntityTypeBuilder<Log_OperateTrace> builder)
            {
                builder.ToTable("Log_OperateTrace");
                builder.HasKey(m => m.Id);
                builder.Property(m => m.LogTime).IsRequired();//.HasColumnType("datetime"); 
                builder.Property(m => m.UserID).HasMaxLength(100);
                builder.Property(m => m.UserName).HasMaxLength(200);
                builder.Property(m => m.ServerIP).HasMaxLength(20).IsRequired();
                builder.Property(m => m.ServerHost).HasMaxLength(40).IsRequired();
                builder.Property(m => m.ClientHost).HasMaxLength(40).IsRequired();
                builder.Property(m => m.ClientIP).HasMaxLength(20).IsRequired();
                builder.Property(m => m.TabOrModu).HasMaxLength(100);
                builder.Property(m => m.Detail).HasMaxLength(2000).IsRequired();
                builder.Property(m => m.Remark).HasMaxLength(2000);


            }
        }


    }

    internal class Log_SystemMonitorContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbGeneral = ComDBFun.GetDBGeneralInfo(DBType.LogMonitor);
            if (dbGeneral.DataBaseType == DataBaseType.SqlServer)
            {
                optionsBuilder.UseSqlServer(ComDBFun.GetConnectionString(DBType.LogMonitor));
            }

            else if (dbGeneral.DataBaseType == DataBaseType.Oracle)
            {
                optionsBuilder.UseOracle(ComDBFun.GetConnectionString(DBType.LogMonitor));
            }

            else if (dbGeneral.DataBaseType == DataBaseType.MySql)
            {
                optionsBuilder.UseMySQL(ComDBFun.GetConnectionString(DBType.LogMonitor));
            }

            base.OnConfiguring(optionsBuilder);
        }

        public Log_SystemMonitorContext()
        {
        }

        public virtual DbSet<Log_SystemMonitor> Log_SystemMonitor { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var dbGeneral = ComDBFun.GetDBGeneralInfo(DBType.LogMonitor);

            //   Database.SetInitializer(new Log_SystemMonitorInitializer());        

            modelBuilder.ApplyConfiguration(new Log_SystemMonitorMap());
            //var typesToRegister = Assembly.GetExecutingAssembly().GetTypes().Where(q => q.GetInterface(typeof(IEntityTypeConfiguration<>).FullName) != null);
            //foreach (var type in typesToRegister)
            //{
            //    dynamic configurationInstance = Activator.CreateInstance(type);
            //    modelBuilder.ApplyConfiguration(configurationInstance);
            //}

            if (!string.IsNullOrEmpty(dbGeneral.SchemaName))
            {
                modelBuilder.HasDefaultSchema(dbGeneral.SchemaName.ToUpper());
            }
            base.OnModelCreating(modelBuilder);
        }






        class Log_SystemMonitorMap : IEntityTypeConfiguration<Log_SystemMonitor>
        {
            public void Configure(EntityTypeBuilder<Log_SystemMonitor> builder)
            {
                builder.ToTable("Log_SystemMonitor");
                builder.HasKey(m => m.Id);
                builder.Property(m => m.LogTime).IsRequired();//.HasColumnType("datetime"); 
                builder.Property(m => m.ServerIP).HasMaxLength(20).IsRequired();
                builder.Property(m => m.ServerHost).HasMaxLength(40).IsRequired();
                builder.Property(m => m.Remark).HasMaxLength(2000);
                builder.Property(m => m.PageViewNum).HasMaxLength(2000);//.HasColumnOrder(11);
                builder.Property(m => m.DiskSpace).HasMaxLength(2000);//.HasColumnOrder(12);

                // builder.  HasOptional(a => a.PageViewNum);
                //  builder. HasOptional(a => a.DiskSpace);    

            }
        }



    }






#endif





}
