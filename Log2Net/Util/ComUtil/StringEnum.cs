using System;
using System.Collections.Generic;
using System.Linq;

namespace Log2Net.Util
{
    internal class StringEnum
    {
        [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
        public sealed class EnumDescriptionAttribute : Attribute
        {
            private string description;
            public string Description { get { return description; } }

            public EnumDescriptionAttribute(string description)
                : base()
            {
                this.description = description;
            }
        }

        public static class EnumHelper
        {
            public static string GetDescription(Enum value)
            {
                if (value == null)
                {
                    throw new ArgumentException("value");
                }
                string description = value.ToString();
                var fieldInfo = value.GetType().GetField(description);
                var attributes = (EnumDescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    description = attributes[0].Description;
                }
                return description;
            }



        }


        public static T GetEnumValue<T>(string enumString)
        {
            enumString = HandEnumString(typeof(T), enumString);
            var enumVal = (T)(Enum.Parse(typeof(T), enumString));
            return enumVal;
        }

        public static object GetEnumValue(Type enumType, string enumString)
        {
            enumString = HandEnumString(enumType, enumString);
            var enumVal = Enum.Parse(enumType, enumString);
            return enumVal;
        }

        //获取某个枚举的键值对
        public static Dictionary<string, int> GetDicFromEnumType(object enumType)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            try
            {
                var arrayEnum = Enum.GetValues(enumType.GetType());
                foreach (var myCode in arrayEnum)
                {
                    try
                    {
                        string strVaule = myCode.ToString();//获取名称   //   string strName = Enum.GetName(typeof(UserGroups), myCode);//获取名称
                        dic.Add(strVaule, (int)myCode);
                    }
                    catch
                    {

                    }
                }
                dic = dic.OrderBy(a => a.Value).ToDictionary(k => k.Key, v => v.Value);
            }
            catch
            {

            }
            return dic;
        }


        static string HandEnumString(Type enumType, string enumString)
        {
            enumString = enumString ?? "";
            enumString = enumString.Trim().ToLower();
            foreach (var value in Enum.GetValues(enumType))
            {
                if (Convert.ToInt32(value).ToString() == enumString || value.ToString().ToLower() == enumString)
                {
                    return value.ToString();
                }
            }
            return enumString;
        }
    }

}
