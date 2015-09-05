using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using XY.OAuth;

namespace XY.Utility
{
    public class Http
    {
        #region 全局常量
        //protected static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        protected static readonly string DefaultUserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2383.0 Safari/537.36";
        protected static readonly Encoding DefaultEncoding = Encoding.UTF8; 
        #endregion

        #region GET请求

        #region GET请求 string Get(string url)
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <returns>返回字符串</returns>
        /// <example>
        /// Http.Get("http://api.vaihao.com?id=1234&name=seay");
        /// </example>
        public static string Get(string url)
        {
            return Get(url, null, null, null, null);
        } 
        #endregion

        #region GET请求 string Get(string url, Encoding encoding, int? timeout, string userAgent, CookieCollection cookies)
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="encoding">字符集编码，默认UTF-8</param>
        /// <param name="timeout">超时的时间，单是秒</param>
        /// <param name="userAgent">代理字符串</param>
        /// <param name="cookies">Cookie设置</param>
        /// <returns>返回字符串</returns>
        public static string Get(string url, Encoding encoding, int? timeout, string userAgent, CookieCollection cookies)
        {
            string res = string.Empty;
            if (encoding == null)
            {
                encoding = DefaultEncoding;
            }
            try
            {
                HttpWebResponse response = CreateGetHttpResponse(url, timeout, userAgent, cookies);
                using (var stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        var reader = new StreamReader(stream, encoding);
                        var sb = new StringBuilder();
                        while (-1 != reader.Peek())
                        {
                            sb.Append(reader.ReadLine());
                        }
                        res = sb.ToString();
                    }
                }
                response.Close();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        #endregion

        #region 创建GET方式的HTTP请求
        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        protected static HttpWebResponse CreateGetHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            //if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            //{
            //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            //    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            //}
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //request.Method
            request.Method = "GET";
            //request.UserAgent
            request.UserAgent = DefaultUserAgent;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            //request.ContentType
            //if (!string.IsNullOrEmpty(contentType))
            //{
            //    request.ContentType = contentType; 
            //}
            ////request.Headers
            //if (headers != null && headers.Count > 0)
            //{
            //    request.Headers.Add(headers); 
            //}
            //request.Timeout
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            //request.CookieContainer
            if (cookies != null && cookies.Count>0)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request.GetResponse() as HttpWebResponse;
        }
        #endregion

        #endregion

        #region POST请求

        #region POST请求 string Post(string url, Encoding encoding)
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <returns>返回字符串</returns>
        /// <example>
        /// Http.Post("http://api.vaihao.com?id=1234&name=seay")
        /// </example>
        public static string Post(string url, Encoding encoding)
        {
            try
            {
                string querySting = null;
                Dictionary<string, object> dict = null;
                //判断url中是否含有参数
                if (url.Contains("?"))
                {
                    int index = url.IndexOf("?");
                    if (url.Length != index + 1)
                    {
                        querySting = url.Substring(index + 1);
                    }
                    url = url.Substring(0, index);
                }
                //判断参数字符串中是否有效
                if (!string.IsNullOrEmpty(querySting))
                {
                    //判断参数字符串中是否含有'&'，表示有多个参数
                    if (querySting.Contains("&"))
                    {
                        dict = new Dictionary<string, object>();
                        string[] str = querySting.Split('&');
                        foreach (string item in str)
                        {
                            string[] kv = item.Split('=');
                            if (!string.IsNullOrEmpty(kv[0]))
                            {
                                dict.Add(kv[0], kv[1]);
                            }
                        }
                    }
                    else if (querySting.Contains("="))//判断参数字符串中是否含有'='，表示只有1个参数
                    {
                        dict = new Dictionary<string, object>();
                        string[] kv = querySting.Split('=');
                        if (!string.IsNullOrEmpty(kv[0]))
                        {
                            dict.Add(kv[0], kv[1]);
                        }
                    }
                }
                return Post(url, dict, encoding, null, null, null);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region POST请求 string Post(string url, object data, Encoding encoding)
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="dict">参数对象</param>
        /// <returns>返回字符串</returns>
        /// <example>
        /// Http.Post("http://api.vaihao.com",new{id="1234",name="seay"})
        /// </example>
        public static string Post(string url, object data, Encoding encoding)
        {
            try
            {
                Dictionary<string, object> dict = null;
                string querySting = null;
                //判断url中是否含有参数
                if (url.Contains("?"))
                {
                    int index = url.IndexOf("?");
                    if (url.Length != index + 1)
                    {
                        string param = null;
                        param = url.Substring(index + 1) + "&";
                        param += querySting;
                        querySting = param;
                    }
                    url = url.Substring(0, index);
                }
                //判断参数字符串中是否有效
                if (!string.IsNullOrEmpty(querySting))
                {
                    if (querySting.Contains("&"))
                    {
                        if (dict == null)
                        {
                            dict = new Dictionary<string, object>();
                        }
                        string[] str = querySting.Split('&');
                        foreach (string item in str)
                        {
                            string[] kv = item.Split('=');
                            if (dict.ContainsKey(kv[0]))
                            {
                                dict.Remove(kv[0]);
                            }
                            if (!string.IsNullOrEmpty(kv[0]))
                            {
                                dict.Add(kv[0], kv[1]);
                            }
                        }
                    }
                    else if (querySting.Contains("="))//判断参数字符串中是否含有'='，表示只有1个参数
                    {
                        dict = new Dictionary<string, object>();
                        string[] kv = querySting.Split('=');
                        if (dict.ContainsKey(kv[0]))
                        {
                            dict.Remove(kv[0]);
                        }
                        if (!string.IsNullOrEmpty(kv[0]))
                        {
                            dict.Add(kv[0], kv[1]);
                        }
                    }
                }
                if (data!=null)
                {
                    if (dict == null)
                    {
                        dict = new Dictionary<string, object>();
                    }
                    foreach (var item in data.GetType().GetProperties())
                    {
                        if (dict.ContainsKey(item.Name))
                        {
                            dict.Remove(item.Name);
                        }
                        if (!string.IsNullOrEmpty(item.Name))
                        {
                            dict.Add(item.Name, item.GetValue(data, null));
                        }
                    }
                }
                return Post(url, dict, encoding, null, null, null);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region POST请求 string Post(string url, Dictionary<string, object> parameters,Encoding encoding, int? timeout, string userAgent, CookieCollection cookies)
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="parameters">请求的参数</param>
        /// <param name="encoding">字符集编码，默认UTF-8</param>
        /// <param name="timeout">超时的时间，单是秒</param>
        /// <param name="userAgent">代理字符串</param>
        /// <param name="cookies">Cookie设置</param>
        /// <returns>返回字符串</returns>
        public static string Post(string url, Dictionary<string, object> parameters, Encoding encoding, int? timeout, string userAgent, CookieCollection cookies)
        {
            string res = string.Empty; 
            if (encoding == null)
            {
                encoding = DefaultEncoding;
            }
            try
            {
                HttpWebResponse response = CreatePostHttpResponse(url, parameters, timeout, userAgent, encoding, cookies);
                using (var stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        var reader = new StreamReader(stream, encoding);
                        var sb = new StringBuilder();
                        while (-1 != reader.Peek())
                        {
                            sb.Append(reader.ReadLine());
                        }
                        res = sb.ToString();
                    }
                }
                response.Close();
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        #endregion
        #region 创建POST方式的HTTP请求
        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        protected static HttpWebResponse CreatePostHttpResponse(string url, Dictionary<string, object> parameters, int? timeout, string userAgent, Encoding encoding, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (encoding == null)
            {
                encoding = DefaultEncoding;
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //如果需要POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = encoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }
        protected static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
        #endregion

        #endregion
    }
}