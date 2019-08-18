using Log2Net.LogInfo;
using Log2Net.Models;
using Log2Net.Util.DBUtil.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Log2Net.Util.DBUtil
{

    internal class DBBaseAttr
    {
        public DBBaseAttr()
        {
            DataBaseType = DataBaseType.NoSelect;
            ParaPreChar = "";
            LeftPre = "";
            RightSuf = "";
        }
        public DataBaseType DataBaseType { get; set; }
        public string ParaPreChar { get; set; }  //参数化查询时参数前的符号
        public string LeftPre { get; set; }  //参数名左边的符号
        public string RightSuf { get; set; }//参数名右边的符号



    }
    internal class ComDBFun
    {
        public ComDBFun(DBBaseAttr dBBaseAttr)
        {
            paraChar = dBBaseAttr.ParaPreChar;
            leftPre = dBBaseAttr.LeftPre;
            rightSuf = dBBaseAttr.RightSuf;
        }


        string paraChar = "@"; // define parameter sign
        string leftPre = "";
        string rightSuf = "";

        //获取数据库连接字符串
        public static string GetConnectionString(DBType dbType)
        {
            if (AppConfig.GetFinalConfig("ConnectStrIsInCode", false, LogApi.IsConnectStrInCode()))
            {
                return dbType == DBType.LogTrace ? LogApi.GetTraceDBConnectionString() : LogApi.GetMonitorDBConnectionString();
            }
            else
            {
                string sqlStrKey = GetConnectionStringKey(dbType);
                string conStr = AppConfig.GetDBConnectString(sqlStrKey);
                return conStr;
            }

        }

        //获取数据库连接字符串的key
        public static string GetConnectionStringKey(DBType dbType)
        {
            string sqlStrKey = "sqlStrTest";
            switch (dbType)
            {
                case DBType.LogTrace:
                    sqlStrKey = AppConfig.GetFinalConfig("UserCfg_TraceDBConKey", "logTraceSqlStr", LogApi.GetUserCfg_TraceDBConKey());
                    break;
                case DBType.LogMonitor:
                    sqlStrKey = AppConfig.GetFinalConfig("UserCfg_MonitorDBConKey", "logMonitorSqlStr", LogApi.GetUserCfg_MonitorDBConKey());
                    break;
            }
            return sqlStrKey;
        }


        static Dictionary<DBType, DBGeneral> DBGeneralDic = new Dictionary<DBType, DBGeneral>();

        public static DBGeneral GetDBGeneralInfo(DBType dbType)
        {
            if (DBGeneralDic.ContainsKey(dbType))
            {
                return DBGeneralDic[dbType];
            }

            DataBaseType curDBType = DataBaseType.SqlServer;
            if (dbType == DBType.LogTrace)
            {
                curDBType = AppConfig.GetFinalConfig("UserCfg_TraceDBTypeKey", DataBaseType.SqlServer, LogApi.GetUserCfg_TraceDBTypeKey());
            }
            else
            {
                curDBType = AppConfig.GetFinalConfig("UserCfg_MonitorDBTypeKey", DataBaseType.SqlServer, LogApi.GetUserCfg_MonitorDBTypeKey());
            }

            DBGeneral dBGeneral = new DBGeneral() { DataBaseType = curDBType };

            if (curDBType == DataBaseType.SqlServer)
            {
                dBGeneral.SchemaName = "dbo";
            }
            else if (curDBType == DataBaseType.Oracle)
            {
                dBGeneral.SchemaName = "scott";
            }
            else if (curDBType == DataBaseType.MySql)
            {
                // dBGeneral.SchemaName = "";
            }
            DBGeneralDic.Add(dbType, dBGeneral);
            string msg = dbType.ToString() + "的数据库类型为【" + dBGeneral.DataBaseType.ToString() + "】";
            LogCom.WriteModelToFileForDebug(new { 内容 = msg });
            return dBGeneral;
        }

        public string GetSQLText(List<string> colNames)
        {
            if (colNames == null || colNames.Count <= 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder("(" + leftPre);
            sb.Append(string.Join(rightSuf + "," + leftPre, colNames));
            sb.Append(rightSuf + ")");
            string[] colParamNames = GetColumnNames(colNames, paraChar);  //得到数组形式的@列名    

            //sb.Append(" values([").Append(string.Join("],[", colParamNames)).Append("])");//values([@UserName],[@UserPWD])
            sb.Append(" values(").Append(string.Join(",", colParamNames)).Append(")"); //values(@UserName,@UserPWD)
            return sb.ToString();
        }

        public string GetUpdateSQLText(List<string> colNames)
        {
            if (colNames == null || colNames.Count <= 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < colNames.Count; i++)
            {
                sb.Append(leftPre + colNames[i] + rightSuf + "=" + paraChar + RemoveSpecialChar(colNames[i]) + ",");
            }
            return sb.ToString().TrimEnd(',');
        }

        public string GetWhereCondition(List<string> colNames, string and_or, params Dictionary<string, object>[] kvDic)
        {
            if (colNames == null || colNames.Count <= 0)
            {
                return "";
            }

            string result = "";
            for (int i = 0; i < colNames.Count; i++)
            {
                if (kvDic != null && kvDic.Length > 0 && kvDic[0].ContainsKey(colNames[i]))
                {
                    string[] values = kvDic[0][colNames[i]].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < values.Length; j++)
                    {
                        result += (leftPre + colNames[i] + rightSuf + "=" + paraChar + RemoveSpecialChar(colNames[i]) /*+ j*/);
                        if (j != values.Length - 1)
                        {
                            result += " " + and_or + " ";
                        }
                    }
                }
                else
                {
                    if (!colNames[i].Contains("="))
                    {
                        result += (leftPre + colNames[i] + rightSuf + "=" + paraChar + RemoveSpecialChar(colNames[i]));
                    }
                    else
                    {
                        result += colNames[i];
                    }
                }

                if (i != colNames.Count - 1)
                {
                    result += " " + and_or + " ";
                }
            }
            if (result != "")
            {
                result = " where " + result;
            }
            return result;
        }

        public string[] GetColumnNames(List<string> colNames, string preFlag) ////得到数组形式的@列名
        {
            if (colNames == null || colNames.Count <= 0)
            {
                return new string[0];
            }
            string[] colnames = new string[colNames.Count];
            for (int i = 0; i < colNames.Count; i++)
            {

                colnames[i] = preFlag + RemoveSpecialChar(colNames[i]);
            }
            return colnames;
        }

        public static string RemoveSpecialChar(string text)    //处理 "小时数/天数"之类的列名
        {
            text = text.Replace(' ', '_').Replace('/', '_').Replace('\\', '_').Replace('(', '_').Replace(')', '_').
                Replace('[', '_').Replace(']', '_').Trim();//最好用正则表达式匹配
            return text;
        }





    }


}


