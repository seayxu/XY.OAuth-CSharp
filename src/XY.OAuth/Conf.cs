using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using XY.OAuth.Model;
using XY.Utility;

namespace XY.OAuth
{
    public static class Conf
    {
        #region 变量
        private static string _OSC_Client_ID;
        private static string _OSC_Client_Secret;
        private static string _OSC_Callback_URL;
        private static string _Sina_Client_ID;
        private static string _Sina_Client_Secret;
        private static string _Sina_Callback_URL;
        private static string _QQ_Client_ID;
        private static string _QQ_Client_Secret;
        private static string _QQ_Callback_URL;
        private static string _Baidu_Client_ID;
        private static string _Baidu_Client_Secret;
        private static string _Baidu_Callback_URL;
        private static string _GitHub_Client_ID;
        private static string _GitHub_Client_Secret;
        private static string _GitHub_Callback_URL;
        private static string _FaceBook_Client_ID;
        private static string _FaceBook_Client_Secret;
        private static string _FaceBook_Callback_URL;
        #endregion

        /// <summary>
        /// 开源中国应用OAuth2ID
        /// </summary>
        public static string OSC_Client_ID {
            get
            {
                if (string.IsNullOrEmpty(Conf._OSC_Client_ID))
                {
                    Conf._OSC_Client_ID = ConfigurationManager.AppSettings["OSC_Client_ID"].ToString();
                }
                return Conf._OSC_Client_ID;
            }
            set
            {
                Conf._OSC_Client_ID = value;
            }
        }
        /// <summary>
        /// 开源中国应用OAuth2密钥
        /// </summary>
        public static string OSC_Client_Secret
        {
            get
            {
                if (string.IsNullOrEmpty(Conf._OSC_Client_Secret))
                {
                    Conf._OSC_Client_Secret = ConfigurationManager.AppSettings["OSC_Client_Secret"].ToString();
                }
                return Conf._OSC_Client_Secret;
            }
            set
            {
                Conf._OSC_Client_Secret = value;
            }
        }
        /// <summary>
        /// 开源中国应用回掉地址
        /// </summary>
        public static string OSC_Callback_URL
        {
            get
            {
                if (string.IsNullOrEmpty(Conf._OSC_Callback_URL))
                {
                    Conf._OSC_Callback_URL = ConfigurationManager.AppSettings["OSC_Callback_URL"].ToString();
                }
                return Conf._OSC_Callback_URL;
            }
            set
            {
                Conf._OSC_Callback_URL = value;
            }
        }

        /// <summary>
        /// 新浪微博应用OAuth2ID
        /// </summary>
        public static string Sina_Client_ID
        {
            get
            {
                if (string.IsNullOrEmpty(Conf._Sina_Client_ID))
                {
                    Conf._Sina_Client_ID = ConfigurationManager.AppSettings["Sina_Client_ID"].ToString();
                }
                return Conf._Sina_Client_ID;
            }
            set
            {
                Conf._Sina_Client_ID = value;
            }
        }
        /// <summary>
        /// 新浪微博应用OAuth2密钥
        /// </summary>
        public static string Sina_Client_Secret
        {
            get
            {
                if (string.IsNullOrEmpty(Conf._Sina_Client_Secret))
                {
                    Conf._Sina_Client_Secret = ConfigurationManager.AppSettings["Sina_Client_Secret"].ToString();
                }
                return Conf._Sina_Client_Secret;
            }
            set
            {
                Conf._Sina_Client_Secret = value;
            }
        }
        /// <summary>
        /// 新浪微博应用回掉地址
        /// </summary>
        public static string Sina_Callback_URL
        {
            get
            {
                if (string.IsNullOrEmpty(Conf._Sina_Callback_URL))
                {
                    Conf._Sina_Callback_URL = ConfigurationManager.AppSettings["Sina_Callback_URL"].ToString();
                }
                return Conf._Sina_Callback_URL;
            }
            set
            {
                Conf._Sina_Callback_URL = value;
            }
        }

        /// <summary>
        /// 腾讯QQ应用OAuth2ID
        /// </summary>
        public static string QQ_Client_ID
        {
            get
            {
                if (string.IsNullOrEmpty(Conf._QQ_Client_ID))
                {
                    Conf._QQ_Client_ID = ConfigurationManager.AppSettings["QQ_Client_ID"].ToString();
                }
                return Conf._QQ_Client_ID;
            }
            set
            {
                Conf._QQ_Client_ID = value;
            }
        }
        /// <summary>
        /// 腾讯QQ应用OAuth2密钥
        /// </summary>
        public static string QQ_Client_Secret
        {
            get
            {
                if (string.IsNullOrEmpty(Conf._QQ_Client_Secret))
                {
                    Conf._QQ_Client_Secret = ConfigurationManager.AppSettings["QQ_Client_Secret"].ToString();
                }
                return Conf._QQ_Client_Secret;
            }
            set
            {
                Conf._QQ_Client_Secret = value;
            }
        }
        /// <summary>
        /// 腾讯QQ应用回掉地址
        /// </summary>
        public static string QQ_Callback_URL
        {
            get
            {
                if (string.IsNullOrEmpty(Conf._QQ_Callback_URL))
                {
                    Conf._QQ_Callback_URL = ConfigurationManager.AppSettings["QQ_Callback_URL"].ToString();
                }
                return Conf._QQ_Callback_URL;
            }
            set
            {
                Conf._QQ_Callback_URL = value;
            }
        }

        /// <summary>
        /// 百度应用OAuth2ID
        /// </summary>
        public static string Baidu_Client_ID
        {
            get
            {
                if (string.IsNullOrEmpty(Conf._Baidu_Client_ID))
                {
                    Conf._Baidu_Client_ID = ConfigurationManager.AppSettings["Baidu_Client_API"].ToString();
                }
                return Conf._Baidu_Client_ID;
            }
            set
            {
                Conf._Baidu_Client_ID = value;
            }
        }
        /// <summary>
        /// 百度应用OAuth2密钥
        /// </summary>
        public static string Baidu_Client_Secret
        {
            get
            {
                if (string.IsNullOrEmpty(Conf._Baidu_Client_Secret))
                {
                    Conf._Baidu_Client_Secret = ConfigurationManager.AppSettings["Baidu_Client_Secret"].ToString();
                }
                return Conf._Baidu_Client_Secret;
            }
            set
            {
                Conf._Baidu_Client_Secret = value;
            }
        }
        /// <summary>
        /// 百度应用回掉地址
        /// </summary>
        public static string Baidu_Callback_URL
        {
            get
            {
                if (string.IsNullOrEmpty(Conf._Baidu_Callback_URL))
                {
                    Conf._Baidu_Callback_URL = ConfigurationManager.AppSettings["Baidu_Callback_URL"].ToString();
                }
                return Conf._Baidu_Callback_URL;
            }
            set
            {
                Conf._Baidu_Callback_URL = value;
            }
        }

        /// <summary>
        /// GitHub应用OAuth2ID
        /// </summary>
        public static string GitHub_Client_ID
        {
            get
            {
                if (string.IsNullOrEmpty(Conf._GitHub_Client_ID))
                {
                    Conf._GitHub_Client_ID = ConfigurationManager.AppSettings["GitHub_Client_ID"].ToString();
                }
                return Conf._GitHub_Client_ID;
            }
            set
            {
                Conf._GitHub_Client_ID = value;
            }
        }
        /// <summary>
        /// GitHub应用OAuth2密钥
        /// </summary>
        public static string GitHub_Client_Secret
        {
            get
            {
                if (string.IsNullOrEmpty(Conf._GitHub_Client_Secret))
                {
                    Conf._GitHub_Client_Secret = ConfigurationManager.AppSettings["GitHub_Client_Secret"].ToString();
                }
                return Conf._GitHub_Client_Secret;
            }
            set
            {
                Conf._GitHub_Client_Secret = value;
            }
        }
        /// <summary>
        /// GitHub应用回掉地址
        /// </summary>
        public static string GitHub_Callback_URL
        {
            get
            {
                if (string.IsNullOrEmpty(Conf._GitHub_Callback_URL))
                {
                    Conf._GitHub_Callback_URL = ConfigurationManager.AppSettings["GitHub_Callback_URL"].ToString();
                }
                return Conf._GitHub_Callback_URL;
            }
            set
            {
                Conf._GitHub_Callback_URL = value;
            }
        }

        /// <summary>
        /// FaceBook应用OAuth2ID
        /// </summary>
        public static string FaceBook_Client_ID
        {
            get
            {
                if (string.IsNullOrEmpty(Conf._FaceBook_Client_ID))
                {
                    Conf._FaceBook_Client_ID = ConfigurationManager.AppSettings["FaceBook_Client_ID"].ToString();
                }
                return Conf._FaceBook_Client_ID;
            }
            set
            {
                Conf._FaceBook_Client_ID = value;
            }
        }
        /// <summary>
        /// FaceBook应用OAuth2密钥
        /// </summary>
        public static string FaceBook_Client_Secret
        {
            get
            {
                if (string.IsNullOrEmpty(Conf._FaceBook_Client_Secret))
                {
                    Conf._FaceBook_Client_Secret = ConfigurationManager.AppSettings["FaceBook_Client_Secret"].ToString();
                }
                return Conf._FaceBook_Client_Secret;
            }
            set
            {
                Conf._FaceBook_Client_Secret = value;
            }
        }
        /// <summary>
        /// FaceBook应用回掉地址
        /// </summary>
        public static string FaceBook_Callback_URL
        {
            get
            {
                if (string.IsNullOrEmpty(Conf._FaceBook_Callback_URL))
                {
                    Conf._FaceBook_Callback_URL = ConfigurationManager.AppSettings["FaceBook_Callback_URL"].ToString();
                }
                return Conf._FaceBook_Callback_URL;
            }
            set
            {
                Conf._FaceBook_Callback_URL = value;
            }
        }

        #region GET方式获取字典集合
        /// <summary>
        /// GET方式获取字典集合
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetDictionary(string url, params object[] param)
        {
            try
            {
                if (param != null && param.Length > 0)
                {
                    url = string.Format(url, param);
                }
                string json = Http.Get(url);
                Dictionary<string, object> result = JsonHelper.Deserialize<Dictionary<string, object>>(json);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region POST方式获取字典集合
        /// <summary>
        /// POST方式获取字典集合
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static Dictionary<string, object> PostDictionary(string url, params object[] param)
        {
            try
            {
                if (param != null && param.Length > 0)
                {
                    url = string.Format(url, param);
                }
                string json = Http.Post(url,null);
                Dictionary<string, object> result = JsonHelper.Deserialize<Dictionary<string, object>>(json);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}