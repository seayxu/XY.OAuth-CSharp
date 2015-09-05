using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using XY.OAuth.Model;
using XY.OAuth.Utility;

namespace XY.OAuth
{
	/// <summary>
	/// 新浪微博授权类
	/// </summary>
	public class SinaOAuth
    {
		#region //URL 信息
		/// <summary>
		/// 基本的URL
		/// </summary>
		private static readonly string BasetUrl = "https://api.weibo.com/";
		/// <summary>
		/// 请求的URL
		/// </summary>
		private static string AuthorizeUrl = BasetUrl + "oauth2/authorize?client_id={0}&redirect_uri={1}&scope={2}&state={3}&display={4}&forcelogin={5}&language={6}";
		/// <summary>
		/// 验证口令的接口
		/// </summary>
		private static string TokenUrl = BasetUrl + "oauth2/access_token?client_id={0}&client_secret={1}&grant_type=authorization_code&redirect_uri={2}&code={3}";
		/// <summary>
		/// 获取用户基本信息接口
		/// </summary>
		private static string UserBaseInfoUrl = BasetUrl + "2/users/show.json?access_token={0}&uid={1}";
		#endregion

		#region //变量
		/// <summary>
		/// client端的状态值。用于第三方应用防止CSRF攻击，成功授权后回调时会原样带回
		/// </summary>
		private static string State;

		/// <summary>
		/// access_token值
		/// </summary>
		private string Access_Token;
		/// <summary>
		/// 超时时间(单位秒)
		/// </summary>
		private long Expires_In;
		/// <summary>
		/// 授权用户的uid
		/// </summary>
		public long UID;

		/// <summary>
		/// 用户信息类
		/// </summary>
		public SinaUserInfo UserInfo; 
		#endregion

		#region 构造函数
		/// <summary>
		/// 构造函数
		/// </summary>
		public SinaOAuth()
		{
			State = Common.GetRandom().ToString();
		} 
		#endregion

        //1.生成请求URL        
        #region 生成请求URL
        /// <summary>
        /// 生成请求URL
		/// </summary>
		/// <param name="display">
		/// display说明：
		/// 参数取值	类型说明
		/// default	默认的授权页面，适用于web浏览器。
		/// mobile	移动终端的授权页面，适用于支持html5的手机。注：使用此版授权页请用 https://open.weibo.cn/oauth2/authorize 授权接口
		/// wap	wap版授权页面，适用于非智能手机。
		/// client	客户端版本授权页面，适用于PC桌面应用。
		/// apponweibo	默认的站内应用授权页，授权后不返回access_token，只刷新站内应用父框架。
		/// </param>
		/// <param name="forcelogin">是否强制用户重新登录，true：是，false：否。默认false。</param>
		/// <returns>授权请求URL</returns>
		public static string AuthorizeURL(string display, bool forcelogin)
        {
            try
			{
				if (string.IsNullOrEmpty(display))
				{
					display = "default";
				}
				string url = string.Format(AuthorizeUrl, Conf.Sina_Client_ID, Conf.Sina_Callback_URL, "all", State, display, forcelogin==true, null);
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
        /// <returns></returns>
        public string Token(string code,string state)
        {
			try
			{
				//判断client端的状态值，用于第三方应用防止CSRF攻击
				//if (State != state)
				//{
				//	return null;
				//}
				Dictionary<string, object> result = Conf.PostDictionary(TokenUrl, Conf.Sina_Client_ID, Conf.Sina_Client_Secret, Conf.Sina_Callback_URL, code);
				if (result != null && result.Count > 0)
				{
					this.Access_Token = result["access_token"].ToString();//接口获取授权后的access token
					this.Expires_In = long.Parse(result["expires_in"].ToString());//access_token的生命周期，单位是秒数
					this.UID = long.Parse(result["uid"].ToString());//当前授权用户的UID
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
		/// <returns>用户基本信息类</returns>
		public SinaUserInfo GetUserInfo()
        {
            try
			{
				SinaUserInfo model = null;
                Dictionary<string, object> result = Conf.GetDictionary(UserBaseInfoUrl, Access_Token, this.UID);
                if (result != null && result.Count > 0)
                {
					model = new SinaUserInfo()
                    {
						id = long.Parse(result["id"].ToString()),					//用户UID
						idstr = result["idstr"].ToString(),							//字符串型的用户UID
						screen_name = result["screen_name"].ToString(),				//用户昵称
						name = result["name"].ToString(),							//友好显示名称
						province = int.Parse(result["province"].ToString()),		//用户所在省级ID
						city = int.Parse(result["city"].ToString()),				//用户所在城市ID
						location = result["location"].ToString(),					//用户所在地
						description = result["description"].ToString(),				//用户个人描述
						url = result["url"].ToString(),								//用户博客地址
						profile_image_url = result["profile_image_url"].ToString(),	//用户头像地址（中图），50×50像素
						profile_url = result["profile_url"].ToString(),				//用户的微博统一URL地址
						domain = result["domain"].ToString(),						//用户的个性化域名
						weihao = result["weihao"].ToString(),						//用户的微号
						gender = result["gender"].ToString(),						//性别，m：男、f：女、n：未知
						followers_count = int.Parse(result["followers_count"].ToString()),		//粉丝数
						friends_count = int.Parse(result["friends_count"].ToString()),			//关注数
						statuses_count = int.Parse(result["statuses_count"].ToString()),		//微博数
						favourites_count = int.Parse(result["favourites_count"].ToString()),	//收藏数
						created_at = result["created_at"].ToString(),							//用户创建（注册）时间
						following = bool.Parse(result["following"].ToString()),					//暂未支持
						allow_all_act_msg = bool.Parse(result["allow_all_act_msg"].ToString()),	//是否允许所有人给我发私信，true：是，false：否
						geo_enabled = bool.Parse(result["geo_enabled"].ToString()),				//是否允许标识用户的地理位置，true：是，false：否
						verified = bool.Parse(result["verified"].ToString()),					//是否是微博认证用户，即加V用户，true：是，false：否
						verified_type = int.Parse(result["verified_type"].ToString()),			//暂未支持
						remark = result["remark"].ToString(),									//用户备注信息，只有在查询用户关系时才返回此字段
						status = result["status"].ToString(),									//用户的最近一条微博信息字段 详细
						allow_all_comment = bool.Parse(result["allow_all_comment"].ToString()),	//是否允许所有人对我的微博进行评论，true：是，false：否
						avatar_large = result["avatar_large"].ToString(),						//用户头像地址（大图），180×180像素
						avatar_hd = result["avatar_hd"].ToString(),								//用户头像地址（高清），高清头像原图
						verified_reason = result["verified_reason"].ToString(),					//认证原因
						follow_me = bool.Parse(result["follow_me"].ToString()),					//该用户是否关注当前登录用户，true：是，false：否
						online_status = int.Parse(result["online_status"].ToString()),			//用户的在线状态，0：不在线、1：在线
						bi_followers_count = int.Parse(result["bi_followers_count"].ToString()),//用户的互粉数
						lang = result["lang"].ToString()										//用户当前的语言版本，zh-cn：简体中文，zh-tw：繁体中文，en：英语
                    };
					UserInfo = model;
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