﻿using Log2Net.Config;
using Log2Net.Models;
using Log2Net.Util.DBUtil.Direct2DB;
using Log2Net.Util.DBUtil.EF2DB;
using Log2Net.Util.DBUtil.Models;
using System;
using System.Linq;

namespace Log2Net.Util.DBUtil
{
    //数据库访问方式：1、ADO.NET ;2、EF ;3、NH
    internal abstract class DBAccessFac<T> where T : class
    {
        static readonly object locker = new object();
        static DBAccess<T> dBAccess = null;
        public DBAccess<T> DBAccessFactory()
        {
            if (dBAccess == null)
            {
                lock (locker)
                {
                    if (dBAccess == null)
                    {
                        var dbAccessType = Log2NetConfig.GetConfigVal("DBAccessTypeKey");
                        DBAccessType dBAccessType = DBAccessType.ADONET;
                        try { dBAccessType = StringEnum.GetEnumValue<DBAccessType>(dbAccessType); } catch { }
                        string exMsg = "";
                        try { dBAccess = GetDalByDBAccessType(dBAccessType); }
                        catch (Exception ex) { exMsg = ", " + ex.Message; }

                        string msg = typeof(T).Name + "的数据库访问方式为【" + dBAccessType.ToString() + "】" + exMsg;
                        LogApi.WriteMsgToDebugFile(new { 内容 = msg });

                    }
                }
            }
            return dBAccess;

        }

        public abstract DBAccess<T> GetDalByDBAccessType(DBAccessType dbAccessType);




    }


    internal class Log_OperateTraceDBAccessFac : DBAccessFac<Log_OperateTrace>
    {

        public override DBAccess<Log_OperateTrace> GetDalByDBAccessType(DBAccessType dbAccessType)
        {
            if (dbAccessType == DBAccessType.EF)
            {
                Log_OperateTraceEFDal log_OperateTraceDal = new Log_OperateTraceEFDal(new Log_OperateTraceContext());
                return log_OperateTraceDal;
            }
            else if (dbAccessType == DBAccessType.NH)
            {
                throw new Exception("Not define dal methods when DBAccessType = NH");
            }
            else
            {
                return new Log_OperateTraceDirectDAL();
            }

        }
    }


    internal class Log_SystemMonitorDBAccessFac : DBAccessFac<Log_SystemMonitor>
    {


        public override DBAccess<Log_SystemMonitor> GetDalByDBAccessType(DBAccessType dbAccessType)
        {
            if (dbAccessType == DBAccessType.EF)
            {
                Log_SystemMonitorEFDal Log_SystemMonitorDal = new Log_SystemMonitorEFDal(new Log_SystemMonitorContext());
                return Log_SystemMonitorDal;
            }
            else if (dbAccessType == DBAccessType.NH)
            {
                throw new Exception("Not define dal methods when DBAccessType = NH");
            }
            else
            {
                return new Log_SystemMonitorDirectDAL();
            }


        }
    }



    internal abstract class DBAccess<T> where T : class
    {

        internal abstract ExeResEdm /* IQueryable<T>*/ GetAll(PageSerach<T> para);

        internal abstract ExeResEdm Add(AddDBPara<T> dBPara);

    }




}