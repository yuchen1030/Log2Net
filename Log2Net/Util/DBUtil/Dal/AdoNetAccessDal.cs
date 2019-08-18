using Log2Net.LogInfo;
using Log2Net.Models;
using Log2Net.Util.DBUtil.AdoNet;
using Log2Net.Util.DBUtil.AdoNet.Oracle;
using Log2Net.Util.DBUtil.Models;
using System;
using System.Collections.Generic;

namespace Log2Net.Util.DBUtil.Dal
{

    class Log_OperateTraceAdoDal : AdoNetBaseDal<Log_OperateTrace>
    {
        protected override CurrentDalParas CurDalParas
        {
            get
            {
                var curDBType = DBType.LogTrace;
                return new CurrentDalParas()
                {
                    DBType = curDBType,
                    CurDatabaseType = ComDBFun.GetDBGeneralInfo(curDBType).DataBaseType,
                    TableName = "Log_OperateTrace",
                    PrimaryKey = "Id",
                    SkipCols = new string[] { "Id" },
                    Orderby = "Id",
                };
            }
        }

    }


    class Log_SystemMonitorAdoDal : AdoNetBaseDal<Log_SystemMonitor>
    {
        protected override CurrentDalParas CurDalParas
        {
            get
            {
                var curDBType = DBType.LogMonitor;
                return new CurrentDalParas()
                {
                    DBType = curDBType,
                    CurDatabaseType = ComDBFun.GetDBGeneralInfo(curDBType).DataBaseType,
                    TableName = "Log_SystemMonitor",
                    PrimaryKey = "Id",
                    SkipCols = new string[] { "Id" },
                    Orderby = "Id",
                };
            }
        }

    }


    internal abstract class AdoNetBaseDal<T> : IDBAccessDal<T> where T : class
    {
        public AdoNetBaseDal()
        {
            tableName = CurDalParas.TableName;
            primaryKey = CurDalParas.PrimaryKey;
            skipCols = CurDalParas.SkipCols;
            updateKeys = CurDalParas.UpdateKeys;
            deleteKeys = CurDalParas.DeleteKeys;
            orderby = CurDalParas.Orderby;
            conStr = ComDBFun.GetConnectionString(CurDalParas.DBType);
            GetBaseDBByDBType();
        }

        protected abstract CurrentDalParas CurDalParas { get; }//抽象属性，要求子类必须实现

        string tableName = "";
        string primaryKey = "";
        string[] skipCols = new string[] { "" };
        List<string> updateKeys = new List<string>() { "" };
        List<string> deleteKeys = new List<string>() { "" };
        string orderby = "";
        string conStr = "";
        IAdoNetBase<T> baseDB = null;

        internal struct CurrentDalParas
        {
            public DBType DBType { get; set; }
            public DataBaseType CurDatabaseType { get; set; }

            public string TableName { get; set; }

            public string PrimaryKey { get; set; }

            public string[] SkipCols { get; set; } //自增字段时
            public List<string> UpdateKeys { get; set; }
            public List<string> DeleteKeys { get; set; }
            public string Orderby { get; set; }//排序时使用


            public IAdoNetBase<T> AdoNetBase { get; set; }

        }

        void GetBaseDBByDBType()
        {

            bool bOK = false;
            try
            {
                DataBaseType dataBaseType = CurDalParas.CurDatabaseType;
                switch (dataBaseType)
                {
                    case DataBaseType.SqlServer:
                        baseDB = new SqlServerHelper<T>(conStr);
                        break;

                    case DataBaseType.Oracle:
                        // baseDB = new OracleHelperFactory<T>(conStr).GetInstance();
                        baseDB = OracleHelperFactory<T>.GetInstance(conStr);
                        break;
                    case DataBaseType.MySql:
                        baseDB = new MySqlHelper<T>(conStr);
                        break;
                }
                bOK = true;
            }
            catch
            {
            }
            if (!bOK || baseDB == null)
            {
                string msg = "您配置" + typeof(T).Name + "的数据库类型为【" + CurDalParas.CurDatabaseType.ToString() + "】，但代码中尚未实现。";
                LogCom.WriteModelToFileForDebug(new { 内容 = msg });
                throw new Exception(msg);
            }

        }


        public ExeResEdm GetAll(PageSerach<T> para)
        {
            var data = baseDB.GetListByPage(tableName, para);
            return data;
        }

        //添加数据
        public ExeResEdm Add(AddDBPara<T> dBPara)
        {
            return baseDB.Add(tableName, dBPara.Model, skipCols);
        }


    }



}
