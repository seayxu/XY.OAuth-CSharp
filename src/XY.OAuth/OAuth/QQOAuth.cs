using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using XY.OAuth.Model;
using XY.OAuth.Utility;
using XY.Utility;

namespace XY.OAuth
{
	/// <summary>
	/// QQ授权类
	/// </summary>
    public class QQOAuth
    {
		#region //URL 信息
		/// <summary>
		/// 基本的URL
		/// </summary>
		private static readonly string BaseUrl = "https://graph.qq.com/";
		/// <summary>
		/// 基本的URL
		/// </summary>
		private static readonly string BaseUrl_WAP = "https://graph.z.qq.com";
		/// <summary>
		/// 请求的URL
		/// </summary>
		private static string AuthorizeUrl = BaseUrl + "oauth2.0/authorize?response_type=code&client_id={0}&redirect_uri={1}&state={2}&scope={3}";
		/// <summary>
		/// WAP请求的URL
		/// </summary>
		private static string AuthorizeUrl_WAP = BaseUrl_WAP + "moc2/authorize?response_type=code&client_id={0}&redirect_uri={1}&state={2}&scope={3}";
		/// <summary>
		/// 验证口令的接口
		/// </summary>
		private static string TokenUrl = BaseUrl + "oauth2.0/token?client_id={0}&client_secret={1}&grant_type=authorization_code&redirect_uri={2}&code={3}";
		/// <summary>
		/// WAP验证口令的接口
		/// </summary>
		private static string TokenUrl_WAP = BaseUrl_WAP + "moc2/token?client_id={0}&client_secret={1}&grant_type=authorization_code&redirect_uri={2}&code={3}";
		/// <summary>
		/// 获取用户OpenID的接口
		/// </summary>
		private static string OpenIDUrl = BaseUrl + "oauth2.0/me?access_token={0}";
		/// <summary>
		/// WAP获取用户OpenID的接口
		/// </summary>
		private static string OpenIDUrl_WAP = BaseUrl_WAP + "moc2/me?access_token={0}";
		/// <summary>
		/// 获取用户基本信息接口
		/// </summary>
		private static string UserBaseInfoUrl = BaseUrl + "user/get_user_info?access_token={0}&oauth_consumer_key={1}&openid={2}"; 
		#endregion

		#region //变量
		/// <summary>
		/// client端的状态值。用于第三方应用防止CSRF攻击，成功授权后回调时会原样带回
		/// </summary>
		public static string State;

		/// <summary>
		/// access_token值
		/// </summary>
		public string Access_Token;
		/// <summary>
		/// 超时时间(单位秒)
		/// </summary>
		public int Expires_In;
		/// <summary>
		/// 在授权自动续期步骤中，获取新的Access_Token时需要提供的参数。
		/// </summary>
		public string Refresh_Token;
		/// <summary>
		/// 用户OpenID
		/// </summary>
		public string Open_ID;

		/// <summary>
		/// 用户信息类
		/// </summary>
		public QQUserInfo UserInfo; 
		#endregion

		#region 构造函数
		/// <summary>
		/// 构造函数
		/// </summary>
		public QQOAuth()
		{
			//生成状态码
			State = Common.GetRandom().ToString();
		} 
		#endregion

        //1.生成请求URL
        #region 生成请求URL
        /// <summary>
        /// 生成授权请求URL
        /// </summary>
		/// <param name="device">仅PC网站接入时使用。用于展示的样式。不传则默认展示为PC下的样式。如果传入“mobile”|“wap”，则展示为mobile端下的样式。</param>
		/// <returns>授权请求URL</returns>
		public static string AuthorizeURL(string device)
        {
			string format = AuthorizeUrl;
			switch (device)
			{
				case "mobile":
				case "wap":
					format = AuthorizeUrl_WAP;
					format += "&display=mobile";
					break;
				default:
					format = AuthorizeUrl;
					break;
			}
			string url = string.Format(format, Conf.QQ_Client_ID, Conf.QQ_Callback_URL, State, "get_user_info");
			return url;
        }
        #endregion

        //2.获取Token
		#region 获取Token
		/// <summary>
		/// 获取Token
		/// </summary>
		/// <param name="code">如果用户成功登录并授权，则会跳转到指定的回调地址，并在URL中带上Authorization Code。例如，回调地址为www.qq.com/my.php，则跳转到：http://www.qq.com/my.php?code=520DD95263C1CFEA087****** 注意此code会在10分钟内过期。</param>
		/// <param name="state">client端的状态值。用于第三方应用防止CSRF攻击。</param>
		/// <returns></returns>
		public string Token(string code, string state)
		{
			try
			{
				//判断client端的状态值，用于第三方应用防止CSRF攻击
				//if (State!=state)
				//{
				//	return null;
				//}
				//2.获取Token
				string token = Http.Get(string.Format(TokenUrl, Conf.QQ_Client_ID, Conf.QQ_Client_Secret, Conf.QQ_Callback_URL, code));
				if (!string.IsNullOrEmpty(token))
				{
					string[] str = token.Split('&');
					foreach (string item in str)
					{
						string[] kv = item.Split('=');
						switch (kv[0])
						{
							case "access_token":
								this.Access_Token = kv[1];
								break;
							case "refresh_token":
								this.Refresh_Token = kv[1];
								break;
							case "expires_in":
								this.Expires_In = int.Parse(kv[1]);
								break;
						}
					}
				}
				return this.Access_Token;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		//3.获取用户OpenID
		#region 获取用户OpenID
		/// <summary>
        /// 获取用户OpenID
        /// </summary>
		/// <returns>用户OpenID</returns>
        public string OpenID()
        {
            try
            {
                Dictionary<string, object> result = null;
                //3.获取用户OpenID
                if (!string.IsNullOrEmpty(this.Access_Token))
                {
                    string json = Http.Get(string.Format(OpenIDUrl, this.Access_Token));
					json = json.Replace("callback(", "").Replace(");", "").Replace("\r\n", "").Trim();
                    result = JsonHelper.Deserialize<Dictionary<string, object>>(json);
                    if (result != null && result.Count > 0)
                    {
                        this.Open_ID = result["openid"].ToString();
                    }
                }
				return this.Open_ID;
            }
            catch (Exception ex)
            {
				throw ex;
            }
        }
        #endregion

        //4.获取用户信息
        #region 获取用户基本信息
        /// <summary>
        /// 获取用户基本信息
        /// </summary>
		/// <returns>用户基本信息对象</returns>
		public QQUserInfo GetUserInfo()
        {
            try
            {
				QQUserInfo model = null;
                Dictionary<string, object> result = Conf.GetDictionary(UserBaseInfoUrl, Access_Token, Conf.QQ_Client_ID,Open_ID);
                if (result != null && result.Count > 0)
                {
					string ret = result["ret"].ToString();//返回码
					string msg = result["msg"].ToString();//如果ret<0，会有相应的错误信息提示，返回数据全部用UTF-8编码
					if (ret == "0")
					{
						model = new QQUserInfo()
						{
							NickName = result["nickname"].ToString(),
							FigureURL = result["figureurl"].ToString(),
							FigureURL_1 = result["figureurl_1"].ToString(),
							FigureURL_2 = result["figureurl_2"].ToString(),
							FigureURL_QQ_1 = result["figureurl_qq_1"].ToString(),
							FigureURL_QQ_2 = result["figureurl_qq_2"].ToString(),
							Gender = result["gender"].ToString(),
							Is_Yellow_VIP = result["is_yellow_vip"].ToString(),
							Yellow_VIP_Level = result["yellow_vip_level"].ToString(),
							Is_Yellow_Year_VIP = result["is_yellow_year_vip"].ToString()
						};
						this.UserInfo = model;
					}
                }
				return model;
            }
            catch (Exception ex)
            {
				throw ex;
            }
        }
        #endregion
    }
}