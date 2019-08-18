using Log2Net.Util.DBUtil.Models;

#if NET
using System.Data.Entity;
#else
using Microsoft.EntityFrameworkCore;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Log2Net.Util.DBUtil.Dal;
using Log2Net.Models;

namespace Log2Net.Util.DBUtil.EF2DB
{
    internal class Log_OperateTraceEFDal : EFBaseDal<Log_OperateTrace>
    {
        public Log_OperateTraceEFDal(Log_OperateTraceContext context) : base(context)
        {

        }
    }

    internal class Log_SystemMonitorEFDal : EFBaseDal<Log_SystemMonitor>
    {
        public Log_SystemMonitorEFDal(Log_SystemMonitorContext context) : base(context)
        {

        }
    }


    internal class EFBaseDal<T> : IDBAccessDal<T> where T : class
    {
        protected DbContext Context;
        protected DbSet<T> DbSet;
        public EFBaseDal(DbContext context)
        {
            this.Context = context;
            this.DbSet = context.Set<T>();
        }

        public  ExeResEdm GetAll(PageSerach<T> para)
        {
            if (para == null)
            {
                return new ExeResEdm() { ErrCode = 1, Module = "GetAll-EFBaseDal", ErrMsg = "参数不能为空" };
            }
            IQueryable<T> query = this.DbSet;
            var filter = para.Filter;
            var orderBy = para.OrderBy;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (para.IncludeProps != null && para.IncludeProps.Count > 0)
            {
                foreach (var item in para.IncludeProps)
                {
                    //要先判断是否有此导航属性
                    query = query.Include(item);
                }
            }

            if (orderBy != null)
            {
                query = orderBy.Compile()(query);
            }

            int total = query.Count();
            return new ExeResEdm() { ExeModel = query };



        }

        public  ExeResEdm Add(AddDBPara<T> dBPara)
        {
            ExeResEdm dBResEdm = new ExeResEdm() { };
            try
            {
                dBResEdm = Insert(dBPara.Model);
                return dBResEdm;
            }

            catch (Exception ex)
            {
                dBResEdm.Module = "Add 方法";
                dBResEdm.ExBody = ex;
                dBResEdm.ErrCode = 1;
                return dBResEdm;
            }
        }

        ExeResEdm Insert(T entity)
        {
            ExeResEdm dBResEdm = new ExeResEdm() { };
            try
            {
                var t = DbSet.Add(entity);
                this.Context.SaveChanges();
                dBResEdm.ExeModel = t;
                dBResEdm.ExeNum = 1;
                return dBResEdm;
            }
            catch (Exception ex)
            {
                dBResEdm.Module = "Add 方法";
                dBResEdm.ExBody = ex;
                dBResEdm.ErrCode = 1;
                return dBResEdm;
            }


        }

    }
}
