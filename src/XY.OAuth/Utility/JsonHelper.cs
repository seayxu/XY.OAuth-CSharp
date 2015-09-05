using System.IO;
using System.Collections.Generic;
using System;
// add using Newtonsoft.Json
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XY.Utility
{
    /// <summary>
    /// JSON操作助手类
    /// </summary>
    /// <remarks>
    ///  2015-08-31 15:05 Created By SeayXu
    /// </remarks>
    public static class JsonHelper
    {
        /// <summary>
        /// The json serializer
        /// </summary>
        private static JsonSerializer JsonSerializer = new JsonSerializer();
        /// <summary>
        /// 将一个对象序列化JSON字符串
        /// </summary>
        /// <remarks>
        ///  2015-08-31 15:05 Created By SeayXu
        /// </remarks>
        /// <param name="obj">待序列化的对象</param>
        /// <returns>JSON字符串</returns>
        public static string Serialize(object obj)
        {
            var sw = new StringWriter();
            JsonSerializer.Serialize(new JsonTextWriter(sw), obj);
            return sw.GetStringBuilder().ToString();
        }

        /// <summary>
        /// 将JSON字符串反序列化为一个Object对象
        /// </summary>
        /// <remarks>
        ///  2015-08-31 15:05 Created By SeayXu
        /// </remarks>
        /// <param name="json">JSON字符串</param>
        /// <returns>Object对象</returns>
        public static object Deserialize(string json)
        {
            var sr = new StringReader(json);
            return JsonSerializer.Deserialize(new JsonTextReader(sr));
        }

        /// <summary>
        /// 将JSON字符串反序列化为一个指定类型对象
        /// </summary>
        /// <remarks>
        ///  2015-08-31 15:05 Created By SeayXu
        /// </remarks>
		/// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">JSON字符串</param>
        /// <returns>指定类型对象</returns>
		public static T Deserialize<T>(string json) where T : class
        {
            var sr = new StringReader(json);
			return JsonSerializer.Deserialize(new JsonTextReader(sr), typeof(T)) as T;
        }


        /// <summary>
        /// 将JSON字符串反序列化为一个JObject对象
        /// </summary>
        /// <remarks>
        ///  2015-08-31 15:05 Created By SeayXu
        /// </remarks>
        /// <param name="json">JSON字符串</param>
        /// <returns>JObject对象</returns>
        public static JObject DeserializeObject(string json)
        {
            return JsonConvert.DeserializeObject(json) as JObject;
        }
        /// <summary>
        /// 将JSON字符串反序列化为一个JArray数组
        /// </summary>
        /// <remarks>
        ///  2015-08-31 15:05 Created By SeayXu
        /// </remarks>
        /// <param name="json">JSON字符串</param>
        /// <returns>JArray对象</returns>
        public static JArray DeserializeArray(string json)
        {
            return JsonConvert.DeserializeObject(json) as JArray;
        }
    }
}
