using Log2Net.Models;
using Log2Net.Util.DBUtil.Direct2DB.Oracle;
using Log2Net.Util.DBUtil.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Log2Net.Util.DBUtil.Direct2DB
{


    class Log_OperateTraceDirectDAL : DirectDBAccessBaseDal<Log_OperateTrace>
    {


        public override DataBaseType CurDatabaseType
        {
            get
            {
                var dbGeneral = ComDBFun.GetDBGeneralInfo(DBType.LogTrace);
                return dbGeneral.DataBaseType;
            }
            // set { typeStr = value; }
        }

        public override void SetCurretnDalParas()
        {
            CurDalParas = new DirectDBAccessBaseDal<Log_OperateTrace>.CurrentDalParas()
            {
                DBType = DBType.LogTrace,
                TableName = "Log_OperateTrace",
                PrimaryKey = "Id",
                SkipCols = new string[] { "Id" },
                Orderby = "Id",
            };
        }

    }


    class Log_SystemMonitorDirectDAL : DirectDBAccessBaseDal<Log_SystemMonitor>
    {

        public override DataBaseType CurDatabaseType
        {
            get
            {
                var dbGeneral = ComDBFun.GetDBGeneralInfo(DBType.LogMonitor);
                return dbGeneral.DataBaseType;
            }
            // set { typeStr = value; }
        }


        public override void SetCurretnDalParas()
        {

            CurDalParas = new DirectDBAccessBaseDal<Log_SystemMonitor>.CurrentDalParas()
            {
                DBType = DBType.LogMonitor,
                TableName = "Log_SystemMonitor",
                PrimaryKey = "Id",
                SkipCols = new string[] { "Id" },
                Orderby = "Id",
            };
        }

    }


    internal abstract class DirectDBAccessBaseDal<T> : DBAccess<T> where T : class
    {

        public DirectDBAccessBaseDal()
        {
            SetCurretnDalParas();
            tableName = CurDalParas.TableName;
            primaryKey = CurDalParas.PrimaryKey;
            skipCols = CurDalParas.SkipCols;
            updateKeys = CurDalParas.UpdateKeys;
            deleteKeys = CurDalParas.DeleteKeys;
            orderby = CurDalParas.Orderby;
            conStr = ComDBFun.GetConnectionString(CurDalParas.DBType);
            GetBaseDBByDBType();
        }

        string tableName = "";
        string primaryKey = "";
        string[] skipCols = new string[] { "" };
        List<string> updateKeys = new List<string>() { "" };
        List<string> deleteKeys = new List<string>() { "" };
        string orderby = "";

        string conStr = "";
        IDirect2DBBase<T> baseDB = null;

        internal struct CurrentDalParas
        {
            public DBType DBType { get; set; }

            public string TableName { get; set; }

            public string PrimaryKey { get; set; }

            public string[] SkipCols { get; set; } //自增字段时
            public List<string> UpdateKeys { get; set; }
            public List<string> DeleteKeys { get; set; }
            public string Orderby { get; set; }//排序时使用


            public IDirect2DBBase<T> Direct2DBBase { get; set; }

        }

        void GetBaseDBByDBType()
        {

            bool bOK = false;
            try
            {
                DataBaseType dataBaseType = CurDatabaseType;
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
                string msg = "您配置" + typeof(T).Name + "的数据库类型为【" + CurDatabaseType.ToString() + "】，但代码中尚未实现。";
                LogApi.WriteMsgToDebugFile(new { 内容 = msg });
                throw new Exception(msg);
            }

        }

        internal CurrentDalParas CurDalParas = new CurrentDalParas();

        public abstract void SetCurretnDalParas();//抽象类，要求子类必须实现

        public abstract DataBaseType CurDatabaseType { get; }



        internal override ExeResEdm /* IQueryable<T>*/ GetAll(PageSerach<T> para)
        {
            var data = baseDB.GetListByPage(tableName, para);
            return data;


        }

        //添加数据
        internal override ExeResEdm Add(AddDBPara<T> dBPara)
        {
            return baseDB.Add(tableName, dBPara.Model, skipCols);
        }







    }



}
