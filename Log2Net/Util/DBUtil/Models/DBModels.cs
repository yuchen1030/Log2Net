using Log2Net.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace Log2Net.Util.DBUtil.Models
{


    internal class DBBasePara<T> where T : class
    {
        public DBType DBType { get; set; }
        public string TableName { get; set; }
    }

    internal class GetDBPara<T> : DBBasePara<T> where T : class
    {
        public GetDBPara()
        {
            Params = new DbParameter[0];
            TopNum = -1;
        }
        public string WhereSql { get; set; }
        public int TopNum { get; set; }
        public DbParameter[] Params { get; set; }
    }


    internal class SelectSql
    {
        public string Sql { get; set; }
        public DbParameter[] PMS { get; internal set; }

    }


    internal class AddDBPara<T> : DBBasePara<T> where T : class
    {
        public AddDBPara()
        {
            SkipCols = new string[0];
        }


        public T Model { get; set; }
        public string[] SkipCols { get; set; }

    }


    [Serializable]
    internal class SearchParam
    {
        public SearchParam()
        {
            PageIndex = 1;
            PageSize = 10;
            Orderby = "FBillNo";
        }
        public string TableName { get; set; }
        public string Orderby { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string StrWhere { get; set; }

        public int TotalCount { get; set; }
    }


    internal class PageSerach<T> : BaseSerach<T>
    {
        public PageSerach()
        {
            PageIndex = 1;
            PageSize = 10;
        }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }

    internal class BaseSerach<T>
    {
        public Expression<Func<T, bool>> Filter { get; set; }
        //lambda难以表达时直接使用的where语句。
        public string StrWhere { get; set; }
        //public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; set; }//排序条件
        //public Func<IQueryable<T>, IQueryable<T>> OrderBy { get; set; }//排序条件
        public Expression<Func<IQueryable<T>, IQueryable<T>>> OrderBy { get; set; }

        //需要包含的导航属性
        public List<string> IncludeProps { get; set; }

    }


    //执行结果Model
    internal class ExeResEdm
    {
        public int ErrCode { get; set; }  //错误码，0为成功，1为失败，>1为具体的错误代码
        public string ErrMsg { get; set; } //错误信息
        public string Module { get; set; } //模块
        public Exception ExBody { get; set; } //异常Exception
        public object ExeModel { get; set; }  //执行结果
        public int ExeNum { get; set; } //影响的行数


    }







    internal class DBGeneral
    {
        public DataBaseType DataBaseType { get; set; }
        public string SchemaName { get; set; }
    }



}
