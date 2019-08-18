using InfluxData.Net.Common.Enums;
using InfluxData.Net.InfluxDb;
using InfluxData.Net.InfluxDb.Models;
using InfluxData.Net.InfluxDb.Models.Responses;
using Log2Net.Config;
using Log2Net.LogInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Log2Net.Util
{
    internal class InfluxDBHelper
    {
        static InfluxDbClient influxDbClient = null;
        bool bOK = true;
        static bool bCannotConnect = false;
        string _dbName = "";
        static InfluxDBHelper()
        {
            if (AppConfig.GetFinalConfig("bWriteToInfluxDB", false, LogApi.IsWriteToInfluxDB()) )
            {
                var serverStr = AppConfig.GetFinalConfig("InfluxDBServer_Log", "http://127.0.0.1:8086/;logAdmin;sa123.123", LogApi.GetInfluxDBServer_Log()); //http://127.0.0.1:8086/;logAdmin;sa123.123
                var temp = serverStr.Split(';');
                influxDbClient = new InfluxDbClient(temp[0], temp[1], temp[2], InfluxDbVersion.Latest);
            }
            else
            {
                influxDbClient = null;
            }
        }
        public InfluxDBHelper(string dbName)
        {
            if (!bOK || bCannotConnect || influxDbClient == null)
            {
                bOK = false;
                return;
            }

            if (string.IsNullOrEmpty(dbName))
            {
                dbName = "DefaultDB";
            }
            _dbName = dbName;
            try
            {
                var response = influxDbClient.Database.CreateDatabaseAsync(dbName).Result;//若表存在，则跳过（不新建）
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    bOK = false;
                }
            }
            catch (Exception ex)
            {
                bOK = false;
                LogCom.WriteExceptToFile(ex, "InfluxDBHelper.构造函数");
                try
                {
                    if (ex.InnerException.InnerException.Message == "无法连接到远程服务器")
                    {
                        bCannotConnect = true;
                    }
                }
                catch
                {

                }
            }

        }




        public bool WriteData<T>(T model)
        {
            if (!bOK)
            {
                return false;
            }
            try
            {
                Point pt = ConvertToInfluxDBRec<T>(model);
                var responseWrite = influxDbClient.Client.WriteAsync(pt, _dbName).Result;
                return responseWrite.Success;
            }
            catch (Exception ex)
            {

                LogCom.WriteExceptToFile(ex, "InfluxDBHelper.WriteData");
                return false;
            }

        }

        public List<Serie> GetData(string query)
        {
            if (!bOK)
            {
                return null;
            }
            List<Serie> series = influxDbClient.Client.QueryAsync(query, _dbName).Result.ToList();
            return series;
        }

        public List<Serie> GetData(IEnumerable<string> queries)
        {
            if (!bOK)
            {
                return null;
            }
            List<Serie> series = influxDbClient.Client.QueryAsync(queries, _dbName).Result.ToList();
            return series;
        }

        //从一个实体中得到一条Influxdb数据，
        Point ConvertToInfluxDBRec<T>(T model)
        {
            Dictionary<string, object> tags = new Dictionary<string, object>();
            Dictionary<string, object> fields = new Dictionary<string, object>();
            Type t = model.GetType();
            PropertyInfo[] PropertyList = t.GetProperties();
            var sqlXmlType = typeof(System.Data.SqlTypes.SqlXml);
            foreach (PropertyInfo item in PropertyList)
            {
                string name = item.Name;
                object value = item.GetValue(model, null);
                if (value != null && sqlXmlType == item.PropertyType)
                {
                    value = (value as System.Data.SqlTypes.SqlXml).Value;
                }
                else if (value == null)
                {
                    value = "";
                }
                if (item.GetCustomAttribute(typeof(Models.IsInfluxTags)) != null) //is tag
                {
                    tags.Add(name, value);
                }
                else
                {
                    fields.Add(name, value);
                }
            }
            Point pt = new Point()
            {
                Name = typeof(T).Name,
                Tags = tags,
                Fields = fields,
                Timestamp = DateTime.Now
            };
            return pt;
        }


    }

}
