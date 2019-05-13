using AutoMapper;
using Log2Net.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using static Log2Net.LogInfo.LogCom;

namespace Log2Net.Util
{

    //AutoMapper扩展方法
    static class AutoMapperExtension
    {

        /// <summary>
        /// 对象到对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T MapTo<T>(this object obj)
        {
            if (obj == null) return default(T);
            try
            {
                Mapper.Initialize(ctx => ctx.CreateMap(obj.GetType(), typeof(T)));
            }
            catch //(Exception ex)
            {

            }

            return (T)Mapper.Map(obj, obj.GetType(), typeof(T));
            //return Mapper.Map<T>(obj);

        }

        /// <summary>
        /// 集合到集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<T> MapTo<T>(this IEnumerable obj)
        {
            if (obj == null) throw new ArgumentNullException();
            try
            {
                Mapper.Initialize(ctx => ctx.CreateMap(obj.GetType(), typeof(T)));
            }
            catch
            {

            }
            return Mapper.Map<List<T>>(obj);//       return obj.MapTo<T>();
        }


    }





}
