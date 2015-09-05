using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XY.OAuth.Model;
using XY.OAuth.Utility;
using XY.Utility;

namespace XY.OAuth
{
	public class BaiduOAuth
    {
        #region 变量属性
        /// <summary>
        /// 基本的URL
        /// </summary>
        private static readonly string BasetUrl = "https://openapi.baidu.com/";
        /// <summary>
        /// 请求的URL
        /// </summary>
		private static string AuthorizeUrl = BasetUrl + "oauth/2.0/authorize?response_type=code&client_id={0}&redirect_uri={1}&state={2}&display={3}";
        /// <summary>
        /// 验证口令的接口
        /// </summary>
        private static string TokenUrl = BasetUrl + "oauth/2.0/token?grant_type=authorization_code&response_type=code&client_id={0}&client_secret={1}&redirect_uri={2}&code={3}";
        /// <summary>
        /// 获取登录用户基本信息接口
        /// </summary>
        private static string UserBaseInfoUrl = BasetUrl + "rest/2.0/passport/users/getLoggedInUser?access_token={0}&format={1}";
        /// <summary>
        /// 获取指定用户基本信息接口
        /// </summary>
        private static string UserDetailInfoUrl = BasetUrl + "rest/2.0/passport/users/getInfo?access_token={0}&format={1}";

		/// <summary>
		/// client端的状态值。用于第三方应用防止CSRF攻击，成功授权后回调时会原样带回
		/// </summary>
		private static string State;

        /// <summary>
		/// 要获取的Access Token
        /// </summary>
		public string Access_Token;
        /// <summary>
        /// refresh_token值
        /// </summary>
		public string Refresh_Token;
        /// <summary>
		/// Access Token的有效期，以秒为单位
        /// </summary>
		public int Expires_In;
        /// <summary>
		/// 基于http调用Social API时计算参数签名用的签名密钥
        /// </summary>
		public string Session_Secret;
        /// <summary>
		/// 基于http调用Social API时所需要的Session Key，其有效期与Access Token一致
        /// </summary>
		public string Session_Key;
        /// <summary>
        /// scope
        /// </summary>
		public string Scope;
        /// <summary>
        /// 当前登录用户的数字ID
        /// </summary>
        public string UID;
        /// <summary>
        /// 当前登录用户的用户名，值可能为空
        /// </summary>
        public string UName;
        /// <summary>
        /// 当前登录用户的头像
        /// small image: http://tb.himg.baidu.com/sys/portraitn/item/{$portrait} 
        /// large image: http://tb.himg.baidu.com/sys/portrait/item/{$portrait}
        /// </summary>
        public string Portrait;
        /// <summary>
        /// 百度用户基本信息类
        /// </summary>
        public BaiduUserInfo UserInfo;
        #endregion

		#region 构造函数
		/// <summary>
		/// 构造函数
		/// </summary>
		public BaiduOAuth()
		{
			State = Common.GetRandom().ToString();
		} 
		#endregion

        //1.OpenAPI 授权登录页面
        #region 生成请求URL
        /// <summary>
        /// 生成授权请求URL
        /// </summary>
		/// <param name="displayType">
		/// 0->page：全屏形式的授权页面(默认)，适用于web应用。
		/// 1->popup: 弹框形式的授权页面，适用于桌面软件应用和web应用。
		/// 2->dialog:浮层形式的授权页面，只能用于站内web应用。
		/// 3->mobile: Iphone/Android等智能移动终端上用的授权页面，适用于Iphone/Android等智能移动终端上的应用。
		/// 4->tv: 电视等超大显示屏使用的授权页面。
		/// 5->pad: IPad/Android等智能平板电脑使用的授权页面。
		/// </param>
		/// <returns>授权请求URL</returns>
		public static string AuthorizeURL(int displayType)
        {
            try
			{
				string display;
				switch (displayType)
				{
					case 0:
						display = "page";
						break;
					case 1:
						display = "popup";
						break;
					case 2:
						display = "dialog";
						break;
					case 3:
						display = "mobile";
						break;
					case 4:
						display = "tv";
						break;
					case 5:
						display = "pad";
						break;
					default:
						display = "page";
						break;
				}
				string url = string.Format(AuthorizeUrl, Conf.Baidu_Client_ID, Conf.Baidu_Callback_URL,State, display);
                return url;
            }
            catch (Exception ex)
            {
				throw ex;
            }
        }
        #endregion

        //2.获取Token
        #region 获取Token
        /// <summary>
        /// 获取Token
		/// </summary>
		/// <param name="code">调用authorize获得的code值</param>
		/// <param name="state">成功授权后回调时会原样带回client端的状态值</param>
		/// <returns>Token</returns>
        public string Token(string code, string state)
        {
            try
            {
				//判断client端的状态值，用于第三方应用防止CSRF攻击
				//if (State != state)
				//{
				//	return null;
				//}
                string json = Http.Post(string.Format(TokenUrl, Conf.Baidu_Client_ID, Conf.Baidu_Client_Secret, Conf.Baidu_Callback_URL, code),null);
                Dictionary<string, object> result = JsonHelper.Deserialize<Dictionary<string, object>>(json);
                if (result != null && result.Count > 0)
                {
                    this.Access_Token = result["access_token"].ToString();
                    this.Refresh_Token = result["refresh_token"].ToString();
                    this.Expires_In = int.Parse(result["expires_in"].ToString());
                    this.Scope = result["scope"].ToString();
                    this.Session_Secret = result["session_secret"].ToString();
                    this.Session_Key = result["session_key"].ToString();
                }
				return this.Access_Token;
            }
            catch (Exception ex)
            {
				throw ex;
            }
        }
        #endregion

        //3.获取用户信息
        #region 获取用户基本信息
        /// <summary>
        /// 获取用户基本信息
        /// </summary>
		/// <returns>用户基本信息</returns>
		public BaiduUserInfo GetUserInfo()
        {
            try
            {
				BaiduUserInfo model = null;
                Dictionary<string, object> result = Conf.GetDictionary(UserDetailInfoUrl, Access_Token, "json");
                if (result != null && result.Count > 0)
                {
					this.UID = result["userid"].ToString();//当前登录用户的数字ID
					this.UName = result["username"].ToString();//当前登录用户的用户名，值可能为空
					this.Portrait = result["portrait"].ToString();//当前登录用户的头像
					model = new BaiduUserInfo()
                    {
						userid = result["userid"].ToString(),//当前登录用户的数字ID
						username = result["username"].ToString(),//当前登录用户的用户名，值可能为空
						blood = result["blood"].ToString(),//血型
						figure = result["figure"].ToString(),//体型
						portrait = result["portrait"].ToString(),//当前登录用户的头像
						birthday = result["birthday"].ToString(),//生日，以yyyy-mm-dd格式显示
						marriage = result["marriage"].ToString(),//婚姻状况
						sex = result["sex"].ToString(),//性别
						constellation = result["constellation"].ToString(),//星座
						education = result["education"].ToString(),//学历
						trade = result["trade"].ToString(),//当前职业
                        is_bind_mobile = result["is_bind_mobile"].ToString(),
						job = result["job"].ToString()//职位
                    };
					this.UserInfo = model;
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