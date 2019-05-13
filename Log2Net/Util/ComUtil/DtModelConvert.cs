using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Reflection;
using System.Text;

namespace Log2Net.Util
{
    internal class DtModelConvert<T> where T : class
    {
        public static Dictionary<string, object> GetPropertity(object model)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            List<string> list = new List<string>();

            Type t = model.GetType();
            //Type t = typeof(   Login );
            //FieldInfo[] fields = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            //string msg = "property : ";
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object obj = property.GetValue(model, null);
                dic.Add(property.Name, property.GetValue(model, null));
                //list.Add(property.Name);
            }
            return dic;
        }


        public static List<T> DatatableToList(DataTable dt)
        {
            List<T> list = new List<T>();
            if (dt == null || dt.Rows.Count <= 0)
            {
                return list;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(DataRowToModel(dt.Rows[i]));
            }
            return list;
        }

        public static T DataRowToModel(DataRow dr)
        {
            if (dr == null)
            {
                return default(T);
            }
            return TableRowToModel(dr);
        }

        public static DataTable ModelListToDT(IList<T> list)
        {
            return ModelListToDT(list, null);
        }

        public static DataTable ModelListToDT(IList<T> list, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
            {
                propertyNameList.AddRange(propertyName);
            }

            DataTable result = new DataTable();
            if (list.Count > 0 && list[0] != null)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        result.Columns.Add(pi.Name);
                        //result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                        {
                            result.Columns.Add(pi.Name);
                            //result.Columns.Add(pi.Name, pi.PropertyType);
                        }

                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        public static DataTable GetDtHeaders(T objmodel)
        {
            Type modelType = typeof(T);
            DataTable dt = new DataTable(modelType.Name);
            PropertyInfo[] modelpropertys = modelType.GetProperties();
            //遍历model每一个属性并赋值DataRow对应的列
            foreach (PropertyInfo pi in modelpropertys)
            {
                dt.Columns.Add(pi.Name, pi.PropertyType);
                //dt.Columns.Add(pi.Name);
            }
            return dt;
        }

        public static DataTable ConvertToDataSet22(IList<T> list)
        {
            if (list == null || list.Count <= 0)
            {
                DataTable result = new DataTable();
                if (list.Count > 0)
                {
                    PropertyInfo[] propertys = list[0].GetType().GetProperties();
                    foreach (PropertyInfo pi in propertys)
                    {
                        //if (!(pi.Name.GetType() is System.Nullable))
                        //if (pi!=null)
                        {
                            //pi = (PropertyInfo)temp;  
                            result.Columns.Add(pi.Name, pi.PropertyType);
                        }
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        ArrayList tempList = new ArrayList();
                        foreach (PropertyInfo pi in propertys)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        object[] array = tempList.ToArray();
                        result.LoadDataRow(array, true);
                    }
                }
                return result;
            }
            else
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable(typeof(T).Name);
                DataColumn column;
                DataRow row;
                System.Reflection.PropertyInfo[] myPropertyInfo =
                    typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                foreach (T t in list)
                {
                    if (t == null) continue;
                    row = dt.NewRow();
                    for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                    {
                        System.Reflection.PropertyInfo pi = myPropertyInfo[i];
                        String name = pi.Name;
                        if (dt.Columns[name] == null)
                        {
                            if (pi.PropertyType.UnderlyingSystemType.ToString() == "System.Nullable`1[System.Int32]")
                            {
                                column = new DataColumn(name, typeof(Int32));
                                dt.Columns.Add(column);
                                //row[name] = pi.GetValue(t, new object[] {i});//PropertyInfo.GetValue(object,object[])
                                if (pi.GetValue(t, null) != null)
                                    row[name] = pi.GetValue(t, null);
                                else
                                    row[name] = System.DBNull.Value;
                            }
                            else
                            {
                                column = new DataColumn(name, pi.PropertyType);
                                dt.Columns.Add(column);
                                row[name] = pi.GetValue(t, null);
                            }
                        }
                    }
                    dt.Rows.Add(row);
                }
                ds.Tables.Add(dt);
                return ds.Tables[0];
            }
        }

        //表中有数据或无数据时使用,可排除DATASET不支持System.Nullable错误
        public static DataTable ConvertToDataSet(IList<T> list)
        {
            if (list == null || list.Count <= 0)
            //return null;
            {
                DataTable result = new DataTable();
                if (list.Count > 0)
                {
                    PropertyInfo[] propertys = list[0].GetType().GetProperties();
                    foreach (PropertyInfo pi in propertys)
                    {
                        //if (!(pi.Name.GetType() is System.Nullable))
                        //if (pi!=null)
                        {
                            //pi = (PropertyInfo)temp;  
                            result.Columns.Add(pi.Name, pi.PropertyType);
                        }
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        ArrayList tempList = new ArrayList();
                        foreach (PropertyInfo pi in propertys)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        object[] array = tempList.ToArray();
                        result.LoadDataRow(array, true);
                    }
                }
                return result;
            }
            else
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable(typeof(T).Name);
                DataColumn column;
                DataRow row;
                System.Reflection.PropertyInfo[] myPropertyInfo =
                    typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                foreach (T t in list)
                {
                    if (t == null) continue;
                    row = dt.NewRow();
                    for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                    {
                        System.Reflection.PropertyInfo pi = myPropertyInfo[i];
                        String name = pi.Name;
                        if (dt.Columns[name] == null)
                        {
                            if (pi.PropertyType.UnderlyingSystemType.ToString() == "System.Nullable`1[System.Int32]")
                            {
                                column = new DataColumn(name, typeof(Int32));
                                dt.Columns.Add(column);
                                //row[name] = pi.GetValue(t, new object[] {i});//PropertyInfo.GetValue(object,object[])
                                if (pi.GetValue(t, null) != null)
                                    row[name] = pi.GetValue(t, null);
                                else
                                    row[name] = System.DBNull.Value;
                            }
                            else
                            {
                                column = new DataColumn(name, pi.PropertyType);
                                dt.Columns.Add(column);
                                row[name] = pi.GetValue(t, null);
                            }
                        }
                    }
                    dt.Rows.Add(row);
                }
                ds.Tables.Add(dt);
                return ds.Tables[0];
            }
        }

        public static DataTable ListToDT(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }


        static T TableRowToModel(DataRow dtRow)
        {
            T objmodel = System.Activator.CreateInstance<T>();// default(T);
            //获取model的类型
            Type modelType = typeof(T);
            //获取model中的属性
            PropertyInfo[] modelpropertys = modelType.GetProperties();
            //遍历model每一个属性并赋值DataRow对应的列
            foreach (PropertyInfo pi in modelpropertys)
            {
                //获取属性名称
                String name = pi.Name;
                if (dtRow.Table.Columns.Contains(name))
                {
                    try
                    {
                        //非泛型
                        if (!pi.PropertyType.IsGenericType)
                        {
                            if (pi.PropertyType.Name == "String")
                            {
                                pi.SetValue(objmodel, string.IsNullOrEmpty(dtRow[name].ToString()) ? null : Convert.ChangeType(dtRow[name].ToString(), pi.PropertyType), null);
                            }
                            else if (pi.PropertyType.BaseType.Name == "Enum")
                            {
                                pi.SetValue(objmodel, Enum.Parse(pi.PropertyType, dtRow[name].ToString()), null);
                                // pi.SetValue(objmodel, Convert.ChangeType(Enum.Parse(pi.PropertyType, dtRow[name].ToString()), pi.PropertyType), null);

                            }
                            else if (pi.PropertyType.Name == "SqlXml")
                            {
                                var str = dtRow[name].ToString();
                                SqlXml temp = null;
                                if (!string.IsNullOrEmpty(str))
                                {
                                    StringReader Reader = new StringReader(str);
                                    byte[] byteArray = Encoding.UTF8.GetBytes(str);
                                    MemoryStream stream = new MemoryStream(byteArray);
                                    temp = new SqlXml(stream);
                                }

                                pi.SetValue(objmodel, temp, null);
                                //  pi.SetValue(objmodel, Convert.ChangeType(temp, pi.PropertyType), null);
                            }
                            else
                            {
                                pi.SetValue(objmodel, string.IsNullOrEmpty(dtRow[name].ToString()) ? null : Convert.ChangeType(dtRow[name], pi.PropertyType), null);
                            }
                        }
                        //泛型Nullable<>
                        else
                        {
                            Type genericTypeDefinition = pi.PropertyType.GetGenericTypeDefinition();
                            //model属性是可为null类型，进行赋null值
                            if (genericTypeDefinition == typeof(Nullable<>))
                            {
                                //返回指定可以为 null 的类型的基础类型参数
                                pi.SetValue(objmodel, string.IsNullOrEmpty(dtRow[name].ToString()) ? null : Convert.ChangeType(dtRow[name], Nullable.GetUnderlyingType(pi.PropertyType)), null);
                            }
                        }

                    }
                    catch
                    //(Exception ex)
                    {
                        //  LogCom.WriteExceptToFile(ex, "TableRowToModel");
                    }


                }
            }
            return objmodel;
        }


        static T TableRowToModel_old(DataRow dr)
        {
            T model = (T)Activator.CreateInstance(typeof(T));

            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);

                if (propertyInfo != null && dr[i] != DBNull.Value)
                {
                    object _value = dr[i];
                    //propertyInfo.SetValue(model, dr[i], null);

                    string type = propertyInfo.PropertyType.ToString();   //System.Nullable`1[System.Decimal]

                    if (type.Contains("[") && type.Contains("]"))
                    {
                        type = type.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries)[1];
                    }

                    switch (type)
                    {
                        case "System.String":
                            {
                                _value = Convert.ToString(_value);//字符串是全球通用类型，也可以不用转换 
                                propertyInfo.SetValue(model, _value, null);
                            }
                            break;
                        case "System.Int32":
                            {
                                _value = Convert.ToInt32(_value);
                                propertyInfo.SetValue(model, _value, null);
                            }
                            break;
                        case "System.Single":
                            {
                                _value = Convert.ToSingle(_value);
                                propertyInfo.SetValue(model, _value, null);
                            }
                            break;
                        case "System.Decimal":
                            {
                                _value = Convert.ToDecimal(_value);
                                propertyInfo.SetValue(model, _value, null);
                            }
                            break;
                        case "System.Double":
                            {
                                _value = Convert.ToDouble(_value);
                                propertyInfo.SetValue(model, _value, null);
                            }
                            break;
                        case "":
                            {
                                _value = Convert.ToDateTime(_value);
                                propertyInfo.SetValue(model, _value, null);
                            }
                            break;
                        default:
                            break;
                    }
                }

                else continue;
            }

            //foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            //{
            //    if (dr.Table.Columns.Contains(propertyInfo.Name) && dr[propertyInfo.Name] != DBNull.Value)
            //        propertyInfo.SetValue(model, dr[propertyInfo.Name], null);
            //    else continue;
            //}
            return model;
        }


    }

}
