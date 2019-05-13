using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Log2Net.Util
{
    /// <summary>
    /// xml文件和实体转换帮助类
    /// </summary>
    internal class XmlSerializeHelper
    {
        /// <summary>
        /// 将实体类转换成XML文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns>string字符串</returns>
        public static string XmlSerialize<T>(T obj)
        {
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            XmlWriterSettings setting = new XmlWriterSettings();
            setting.Encoding = new UTF8Encoding(false);
            setting.Indent = true;
            using (XmlWriter writer = XmlWriter.Create(stream, setting))
            {
                xs.Serialize(writer, obj);
            }
            return Encoding.UTF8.GetString(stream.ToArray());

            //上面使用的是UTF8编码，StringWriter使用的是UTF16编码
            using (StringWriter sw = new StringWriter())
            {
                Type t = obj.GetType();
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(sw, obj);
                sw.Close();
                return sw.ToString();
            }
        }


        /// <summary>
        /// XML转换成相应的实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strXML"></param>
        /// <returns></returns>
        public static T DESerializer<T>(string strXML) where T : class
        {
            //  return DESerializer2<T>(strXML);
            try
            {
                var typeClass = typeof(T);
                var xmlRootName = "ArrayOf" + typeClass.GenericTypeArguments[0].Name;
                strXML = strXML.Replace("<DocumentElement>", "<" + xmlRootName + ">").Replace("</DocumentElement>", "</" + xmlRootName + ">")
                    .Replace("<DocumentElement />", "<" + xmlRootName + " />");

                using (StringReader sr = new StringReader(strXML))
                {
                    //设置Position属性代码：
                    //sr.Position = 0;
                    //sr.Seek(0, SeekOrigin.Begin);

                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(sr) as T;
                }
            }
            catch //(Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// 集合转换成SQLXML
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="TCollection">泛型集合</param>
        /// <returns>SqlXml对象</returns>
        public static SqlXml ToSqlXml<T>(IEnumerable<T> TCollection)
        {
            if (TCollection == null)
            {
                return null;
            }
            //先把集合转换成数据表，然后把数据表转换成SQLXML
            return DataTableToSqlXml(CollectionToDataTable(TCollection));
        }

        /// <summary>
        /// 集合转换成数据表
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="TCollection">泛型集合</param>
        /// <returns></returns>
        static DataTable CollectionToDataTable<T>(IEnumerable<T> TCollection)
        {
            //获取泛型的具体类型
            Type type = typeof(T);
            //获取类型的公共属性
            PropertyInfo[] properties = type.GetProperties();
            //创建数据表，表名为类型名称
            DataTable table = new DataTable(type.Name);
            //把公共属性转行成表格列，再把表格列添加到表格中
            foreach (var property in properties)
            {
                //创建一个表格列，列名为属性名，列数据类型为属性的类型
                DataColumn column = new DataColumn(property.Name, property.PropertyType);
                //把表格列添加到表格中
                table.Columns.Add(column);
            }
            //把泛型集合元素添加到数据行中
            foreach (var item in TCollection)
            {
                //创建和表格行架构相同的表格行
                DataRow row = table.NewRow();
                //读取元素所有属性列的值，并根据属性名称，把属性值添加到表格行中
                foreach (var property in properties)
                    row[property.Name] = property.GetValue(item, null);
                //把表格行添加到表格中
                table.Rows.Add(row);
            }
            return table;
        }

        /// <summary>
        /// 数据表转换成SQLXML
        /// </summary>
        /// <param name="table">数据表</param>
        /// <returns></returns>
        static SqlXml DataTableToSqlXml(DataTable table)
        {
            SqlXml xml;
            //如果表格名为空，则设置表格名
            if (string.IsNullOrEmpty(table.TableName))
                table.TableName = "TableName";
            //把数据表转换成XML
            using (var ms = new MemoryStream())
            {
                //把数据表转换成XML格式，并写入内存流
                table.WriteXml(ms);
                //把内存流读取标记设置回起点
                ms.Position = 0;
                XmlReaderSettings setting = new XmlReaderSettings();
                xml = new SqlXml(XmlReader.Create(ms));
            }
            return xml;
        }


    }

}
