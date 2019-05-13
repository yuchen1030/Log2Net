using AutoMapper;
using Log2Net.LogInfo;
using Log2Net.Models;
using Log2Net.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Log2Net.LogInfo.LogCom;

namespace Log2Net.Config
{
    //AutoMapper映射配置
    internal class AutoMapperConfig
    {

        //若引用该组件的业务系统也使用AutoMapper，则重复Mapper.Initialize可能会导致错误，因此本组件暂不使用
        public static void Configure()
        {
            try
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Log_OperateTrace, Log_OperateTraceR>();
                    RegisterAutoMapperProfiles(cfg);      //cfg.AddProfile<Log_SystemMonitorProfile>();
                });
                Mapper.AssertConfigurationIsValid();
            }
            catch (Exception ex)
            {
                LogCom.WriteExceptToFile(ex, "AutoMapperConfig.Configure");
            }

        }


        static void RegisterAutoMapperProfiles(IMapperConfigurationExpression cfg)
        {
            // return;
            var profileName = typeof(Profile).FullName;
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes().Where(a => a.BaseType != null && a.BaseType.FullName == profileName).ToList();
            foreach (var type in typesToRegister)
            {
                cfg.AddProfile(type);
                //亦可使用AddProfile的重载方法：
                // dynamic configurationInstance = Activator.CreateInstance(type);
                // cfg.AddProfile(configurationInstance);
            }
        }

        //基础数据映射配置
        class Log_SystemMonitorProfile : Profile
        {
            public Log_SystemMonitorProfile()
            {
                CreateMap<Log_SystemMonitorMQ, Log_SystemMonitor>()
                  .ForMember(d => d.DiskSpace, opt => { opt.Ignore(); })
                  .ForMember(d => d.PageViewNum, opt => { opt.Ignore(); })
                ;

                CreateMap<Log_SystemMonitorR, Log_SystemMonitorMQ>()
                  .ForMember(d => d.DiskSpaceMQ, opt => { opt.Ignore(); })
                  .ForMember(d => d.PageViewNumMQ, opt => { opt.Ignore(); })
                    ;
            }
        }




        public static Log_OperateTrace GetLog_OperateTraceModel(Log_OperateTraceR oldModel)
        {
            if (oldModel == null)
            {
                return null;
            }
            try
            {
                return AutoMapperExtension.MapTo<Log_OperateTrace>(oldModel);
            }
            catch
            {
                var newModel = SerializerHelper.ObjectDeepCopy<Log_OperateTrace>(oldModel);
                return newModel;
            }
        }


        public static Log_SystemMonitor GetLog_SystemMonitorModel(Log_SystemMonitorMQ oldModel)
        {
            if (oldModel == null)
            {
                return null;
            }
            Log_SystemMonitor newModel = new Log_SystemMonitor();
            try
            {
                newModel = AutoMapperExtension.MapTo<Log_SystemMonitor>(oldModel);
            }
            catch
            {
                newModel = SerializerHelper.ObjectDeepCopy<Log_SystemMonitor>(oldModel);
            }

            newModel.DiskSpace =/* GetSQLXml */  GetSQLXmlString<DiskSpaceEdm>(oldModel.DiskSpaceMQ);
            newModel.PageViewNum = /* GetSQLXml */  GetSQLXmlString<PageVist>(oldModel.PageViewNumMQ);
            return newModel;
        }


        public static Log_SystemMonitorMQ GetLog_SystemMonitorMQModel(Log_SystemMonitorR oldModel)
        {
            if (oldModel == null)
            {
                return null;
            }
            Log_SystemMonitorMQ newModel = new Log_SystemMonitorMQ();
            try
            {
                newModel = AutoMapperExtension.MapTo<Log_SystemMonitorMQ>(oldModel);
            }
            catch
            {
                newModel = SerializerHelper.ObjectDeepCopy<Log_SystemMonitorMQ>(oldModel);
            }

            if (oldModel.DiskSpaceR != null)
            {
                newModel.DiskSpaceMQ = new SQLXMLEdm() { IsNull = oldModel.DiskSpaceR.IsNull, Value = oldModel.DiskSpaceR.Value };
            }
            if (oldModel.PageViewNumR != null)
            {
                newModel.PageViewNumMQ = new SQLXMLEdm() { IsNull = oldModel.PageViewNumR.IsNull, Value = oldModel.PageViewNumR.Value };
            }
            return newModel;
        }


        static SqlXml GetSQLXml<T>(SQLXMLEdm sqlXMLEdm)
        {
            SqlXml sqlXml = new SqlXml();
            if (sqlXMLEdm != null && !sqlXMLEdm.IsNull && !string.IsNullOrEmpty(sqlXMLEdm.Value))
            {
                var pages = XmlSerializeHelper.DESerializer<List<T>>(sqlXMLEdm.Value);
                sqlXml = XmlSerializeHelper.ToSqlXml(pages);
            }
            return sqlXml;
        }

        static string GetSQLXmlString<T>(SQLXMLEdm sqlXMLEdm)
        {
            SqlXml sqlXml = new SqlXml();
            if (sqlXMLEdm != null && !sqlXMLEdm.IsNull && !string.IsNullOrEmpty(sqlXMLEdm.Value))
            {
                var pages = XmlSerializeHelper.DESerializer<List<T>>(sqlXMLEdm.Value);
                sqlXml = XmlSerializeHelper.ToSqlXml(pages);
            }
            if (sqlXml == null || sqlXml.IsNull == true)
            {
                return null;
            }
            else
            {
                return sqlXml.Value;
            }

        }


    }



}
