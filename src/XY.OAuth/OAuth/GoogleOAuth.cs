using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XY.OAuth.Model;

namespace XY.OAuth
{
	public class GoogleOAuth
    {
        //https://accounts.google.com/o/oauth2/auth
        /// <summary>
        /// 基本的URL
        /// </summary>
        private static readonly string BasetUrl = "https://accounts.google.com/";
        /// <summary>
        /// 请求的URL
        /// </summary>
        private static string AuthorizeUrl = BasetUrl + "o/oauth2/auth?response_type=code&client_id={0}&redirect_uri={1}&scope={2}&state={3}&access_type={4}&approval_prompt={5}&login_hint={6}&include_granted_scopes={7}";
        //scope=email,profile&state=security_token%3D138r5719ru3e1%26url%3Dhttps://oa2cb.example.com/myHome&redirect_uri=https%3A%2F%2Foauth2-login-demo.appspot.com%2Fcode&,response_type=code&client_id=812741506391.apps.googleusercontent.com&approval_prompt=force
        /// <summary>
        /// 验证口令的接口
        /// </summary>
        private static string TokenUrl = BasetUrl + "oauth2/access_token?client_id={0}&client_secret={1}&grant_type=authorization_code&redirect_uri={2}&code={3}";
        /// <summary>
        /// 获取用户基本信息接口
        /// </summary>
        private static string UserBaseInfoUrl = BasetUrl + "2/users/show.json?access_token={0}&uid={1}";
        /// <summary>
        /// 获取用户详细信息接口
        /// </summary>
        private static string UserDetailInfoUrl = BasetUrl + "/action/openapi/my_information?access_token={0}&dataType={1}";

        /// <summary>
        /// access_token值
        /// </summary>
        private string Access_Token;
        /// <summary>
        /// 超时时间(单位秒)
        /// </summary>
        private long Expires_In;
        /// <summary>
        /// token_type值
        /// </summary>
        private string Token_Type;
        /// <summary>
        /// refresh_token值
        /// </summary>
        private string Refresh_Token;
        /// <summary>
        /// 授权用户的uid
        /// </summary>
        public long UID;

        /// <summary>
        /// 用户信息类
        /// </summary>
        public SinaUserInfo UserBaseInfo;

        //1.生成请求URL
        #region 请求参数
        /*
         请求参数
 	     The set of query string parameters supported by the Google Authorization Server for web server applications are:

        Parameter	Values	Description
        response_type	code	Determines whether the Google OAuth 2.0 endpoint returns an authorization code. Web server applications should use code.
        client_id	The client ID you obtain from the Developers Console.	Identifies the client that is making the request. The value passed in this parameter must exactly match the value shown in the Google Developers Console.
        redirect_uri	One of the redirect_uri values listed for this project in the Developers Console.	Determines where the response is sent. The value of this parameter must exactly match one of the values listed for this project in the Google Developers Console (including the http or https scheme, case, and trailing '/').
        scope	Space-delimited set of permissions that the application requests.	Identifies the Google API access that your application is requesting. The values passed in this parameter inform the consent screen that is shown to the user. There is an inverse relationship between the number of permissions requested and the likelihood of obtaining user consent. For information about available login scopes, see Login scopes. To see the available scopes for all Google APIs, visit the APIs Explorer. It is generally a best practice to request scopes incrementally, at the time access is required, rather than up front. For example, an app that wants to support purchases should not request Google Wallet access until the user presses the “buy” button; see Incremental authorization.
        state	Any string	Provides any state that might be useful to your application upon receipt of the response. The Google Authorization Server roundtrips this parameter, so your application receives the same value it sent. To mitigate against cross-site request forgery (CSRF), it is strongly recommended to include an anti-forgery token in the state, and confirm it in the response. See OpenID Connect for an example of how to do this.
        access_type	online or offline	Indicates whether your application needs to access a Google API when the user is not present at the browser. This parameter defaults to online. If your application needs to refresh access tokens when the user is not present at the browser, then use offline. This will result in your application obtaining a refresh token the first time your application exchanges an authorization code for a user.
        approval_prompt	force or auto	Indicates whether the user should be re-prompted for consent. The default is auto, so a given user should only see the consent page for a given set of scopes the first time through the sequence. If the value is force, then the user sees a consent page even if they previously gave consent to your application for a given set of scopes.
        login_hint	email address or sub identifier	When your application knows which user it is trying to authenticate, it can provide this parameter as a hint to the Authentication Server. Passing this hint will either pre-fill the email box on the sign-in form or select the proper multi-login session, thereby simplifying the login flow.
        include_granted_scopes	true or false	 If this is provided with the value true, and the authorization request is granted, the authorization will include any previous authorizations granted to this user/application combination for other scopes; see Incremental Authorization.
         * ****************************************
         返回数据
        返回值字段	字段类型	字段说明
        code	string	用于调用access_token，接口获取授权后的access token。
        state	string	如果传递参数，会回传该参数。
         */
        #endregion

        #region 生成请求URL
        /// <summary>
        /// 生成请求URL
        /// </summary>
        /// <returns></returns>
        public static string Authorize()
        {
            try
            {
                string url = string.Format(AuthorizeUrl, Conf.Sina_Client_ID, Conf.Sina_Callback_URL, "", "1216", "online", "force", "email", "true");
                return url;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        //2.获取Token
        #region 请求参数
        /*
         HTTP请求方式:POST
        请求参数
 	    参数          必选	类型及范围	说明
        client_id	    true	string	申请应用时分配的AppKey。
        client_secret	true	string	申请应用时分配的AppSecret。
        grant_type	    true	string	请求的类型，填写authorization_code
         * 
        grant_type为authorization_code时
 	    参数            必选	    类型及范围	说明
        code	        true	string	    调用authorize获得的code值。
        redirect_uri	true	string	    回调地址，需需与注册应用里的回调地址一致。
         * 返回值字段	
        参数            字段类型     字段说明
        access_token	string	    用于调用access_token，接口获取授权后的access token。
        expires_in	    string	    access_token的生命周期，单位是秒数。
        remind_in	    string	    access_token的生命周期（该参数即将废弃，开发者请使用expires_in）。
        uid	            string	    当前授权用户的UID。
         */
        #endregion

        #region 获取Token
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Dictionary<string, object> Token(string code)
        {
            try
            {
                Dictionary<string, object> result = Conf.PostDictionary(TokenUrl, Conf.Sina_Client_ID, Conf.Sina_Client_Secret, Conf.Sina_Callback_URL, code);
                if (result != null && result.Count > 0)
                {
                    this.Access_Token = result["access_token"].ToString();
                    this.Expires_In = long.Parse(result["expires_in"].ToString());
                    this.Token_Type = result["token_type"].ToString();
                    this.Refresh_Token = result["refresh_token"].ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        //3.获取用户信息
        #region 参数
        /*
         返回字段说明
        字段	                字段类型	    字段说明
        id	                int64	    用户UID
        idstr	            string	    字符串型的用户UID
        screen_name	        string	    用户昵称
        name	            string	    友好显示名称
        province	        int	        用户所在省级ID
        city	            int	        用户所在城市ID
        location	        string	    用户所在地
        description	        string	    用户个人描述
        url     	        string	    用户博客地址
        profile_image_url	string	    用户头像地址（中图），50×50像素
        profile_url	        string	    用户的微博统一URL地址
        domain	            string	    用户的个性化域名
        weihao	            string	    用户的微号
        gender	            string	    性别，m：男、f：女、n：未知
        followers_count	    int	        粉丝数
        friends_count	    int	        关注数
        statuses_count	    int	        微博数
        favourites_count	int	        收藏数
        created_at	        string	    用户创建（注册）时间
        following	        boolean	    暂未支持
        allow_all_act_msg	boolean	    是否允许所有人给我发私信，true：是，false：否
        geo_enabled	        boolean	    是否允许标识用户的地理位置，true：是，false：否
        verified	        boolean	    是否是微博认证用户，即加V用户，true：是，false：否
        verified_type	    int	        暂未支持
        remark	            string	    用户备注信息，只有在查询用户关系时才返回此字段
        status	            object	    用户的最近一条微博信息字段 详细
        allow_all_comment	boolean	    是否允许所有人对我的微博进行评论，true：是，false：否
        avatar_large	    string	    用户头像地址（大图），180×180像素
        avatar_hd	        string	    用户头像地址（高清），高清头像原图
        verified_reason	    string	    认证原因
        follow_me	        boolean	    该用户是否关注当前登录用户，true：是，false：否
        online_status	    int	        用户的在线状态，0：不在线、1：在线
        bi_followers_count	int	        用户的互粉数
        lang	            string	    用户当前的语言版本，zh-cn：简体中文，zh-tw：繁体中文，en：英语
         */
        #endregion

        #region 获取用户基本信息
        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetUserBaseInfo()
        {
            try
            {
                Dictionary<string, object> result = Conf.GetDictionary(UserBaseInfoUrl, Access_Token, this.UID);
                if (result != null && result.Count > 0)
                {
                    //string error = result["error"].ToString();
                    //string error_description = result["error_description"].ToString();
                    UserBaseInfo = new SinaUserInfo()
                    {
                        id = long.Parse(result["id"].ToString()),
                        idstr = result["idstr"].ToString(),
                        screen_name = result["screen_name"].ToString(),
                        name = result["name"].ToString(),
                        province = int.Parse(result["province"].ToString()),
                        city = int.Parse(result["city"].ToString()),
                        location = result["location"].ToString(),
                        description = result["description"].ToString(),
                        url = result["url"].ToString(),
                        profile_image_url = result["profile_image_url"].ToString(),
                        profile_url = result["profile_url"].ToString(),
                        domain = result["domain"].ToString(),
                        weihao = result["weihao"].ToString(),
                        gender = result["gender"].ToString(),
                        followers_count = int.Parse(result["followers_count"].ToString()),
                        friends_count = int.Parse(result["friends_count"].ToString()),
                        statuses_count = int.Parse(result["statuses_count"].ToString()),
                        favourites_count = int.Parse(result["favourites_count"].ToString()),
                        created_at = result["created_at"].ToString(),
                        following = bool.Parse(result["following"].ToString()),
                        allow_all_act_msg = bool.Parse(result["allow_all_act_msg"].ToString()),
                        geo_enabled = bool.Parse(result["geo_enabled"].ToString()),
                        verified = bool.Parse(result["verified"].ToString()),
                        verified_type = int.Parse(result["verified_type"].ToString()),
                        remark = result["remark"].ToString(),
                        status = result["status"].ToString(),
                        allow_all_comment = bool.Parse(result["allow_all_comment"].ToString()),
                        avatar_large = result["avatar_large"].ToString(),
                        avatar_hd = result["avatar_hd"].ToString(),
                        verified_reason = result["verified_reason"].ToString(),
                        follow_me = bool.Parse(result["follow_me"].ToString()),
                        online_status = int.Parse(result["online_status"].ToString()),
                        bi_followers_count = int.Parse(result["bi_followers_count"].ToString()),
                        lang = result["lang"].ToString()
                    };
                }
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