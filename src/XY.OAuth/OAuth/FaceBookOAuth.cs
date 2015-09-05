using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XY.OAuth.Model;

namespace XY.OAuth
{
	public class FaceBookOAuth
    {
        #region 变量属性
        /// <summary>
        /// 基本的URL
        /// </summary>
        private static readonly string BasetUrl = "https://www.facebook.com/";
        /// <summary>
        /// API的URL
        /// </summary>
        private static readonly string APIUrl = "https://graph.facebook.com/v2.3/";

        /// <summary>
        /// 请求的URL
        /// </summary>
        private static string AuthorizeUrl = BasetUrl + "dialog/oauth?client_id={0}&redirect_uri={1}&state={2}&response_type=code&scope={3}";
        /// <summary>
        /// 验证口令的接口
        /// </summary>
        private static string TokenUrl = APIUrl + "oauth/access_token?client_id={0}&client_secret={1}&redirect_uri={2}&code={3}";
        /// <summary>
        /// 获取登录用户基本信息接口
        /// </summary>
        private static string UserBaseInfoUrl = APIUrl + "user?access_token={0}";
        /// <summary>
        /// 获取指定用户基本信息接口
        /// </summary>
        private static string UserDetailInfoUrl = BasetUrl + "rest/2.0/passport/users/getInfo?access_token={0}&format={1}";

        /// <summary>
        /// access_token值
        /// </summary>
        private string Access_Token;
        /// <summary>
        /// Token_Type
        /// </summary>
        private string Token_Type;
        /// <summary>
        /// expires_in
        /// </summary>
        private string Expires_In;
        /// <summary>
        /// scope,权限列表
        /// </summary>
        public string Scope;
        /// <summary>
        /// 百度用户基本信息类
        /// </summary>
        public GitHubUserInfo UserBaseInfo;
        #endregion

        //1.OpenAPI 授权登录页面
        #region 请求参数
        /*
         * 请求参数
         This endpoint has the following required parameters:

        client_id. The ID of your app, found in your app's dashboard.
        redirect_uri. The URL that you want to redirect the person logging in back to. This URL will capture the response from the Login Dialog. If you are using this in a webview within a desktop app, this must be set to https://www.facebook.com/connect/login_success.html.
        It also has the following optional parameters:

        state. An arbitrary unique string created by your app to guard against Cross-site Request Forgery.
        response_type. Determines whether the response data included when the redirect back to the app occurs is in URL parameters or fragments. See the Confirming Identity section to choose which type your app should use. This can be one of:
        code. Response data is included as URL parameters and contains code parameter (an encrypted string unique to each login request). This is the default behaviour if this parameter is not specified. It's most useful when your server will be handling the token.
        token. Response data is included as a URL fragment and contains an access token. Desktop apps must use this setting for response_type. This is most useful when the client will be handling the token.
        code%20token. Response data is included as a URL fragment and contains both an access token and the code parameter.
        granted_scopes. Returns a comma-separated list of all Permissions granted to the app by the user at the time of login. Can be combined with other response_type values. When combined with token, response data is included as a URL fragment, otherwise included as a URL parameter.
        scope. A comma separated list of Permissions to request from the person using your app.
         * ************************************************************
         * 返回字段说明
        字段	    类型及范围	说明
        code	string	    授权码
        state	string	    应用传递的可选参数
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
                string url = string.Format(AuthorizeUrl, Conf.FaceBook_Client_ID, Conf.FaceBook_Callback_URL, "1216", "public_profile,user_about_me,email,read_stream");
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
         * 请求参数
        Name	        Type	Description
        client_id	    string	Required. The client ID you received from GitHub when you registered.
        client_secret	string	Required. The client secret you received from GitHub when you registered.
        code	        string	Required. The code you received as a response to Step 1.
        redirect_uri	string	The URL in your app where users will be sent after authorization. See details below about redirect urls.
         * ****************************************************************************
         * 返回字段说明
        access_token=e72e16c7e42f292c6912e7710c838347ae178b4a&scope=user%2Cgist&token_type=bearer
         * 
         * You can also receive the content in different formats depending on the Accept header:
        Accept: application/json
        {"access_token":"e72e16c7e42f292c6912e7710c838347ae178b4a", "scope":"repo,gist", "token_type":"bearer"}

        Accept: application/xml
        <OAuth>
          <token_type>bearer</token_type>
          <scope>repo,gist</scope>
          <access_token>e72e16c7e42f292c6912e7710c838347ae178b4a</access_token>
        </OAuth>
         */

        #endregion

        #region 获取Token
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public Dictionary<string, object> Token(string code, string state)
        {
            try
            {
                Dictionary<string, object> result = Conf.GetDictionary(string.Format(TokenUrl, Conf.FaceBook_Client_ID, Conf.FaceBook_Client_Secret, Conf.FaceBook_Callback_URL, code));
                if (result!=null)
                {
                    this.Access_Token = result["access_token"].ToString();
                    this.Expires_In = result["expires_in"].ToString();
                    this.Token_Type = result["token_type"].ToString();
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
        #region 请求参数
        /*
         * 请求参数
        基于https调用Open API时需要传递的系统级输入参数：

        参数名	参数类型	是否必需	描述
        access_token	string	是	授权码，应用访问百度的任何Restful Open API都必须出具授权码以表明其是一个合法第三方。其值必须是通过OAuth2.0协议换取access token时所拿到的access_token参数值。
         * ************************************
         *返回字段说明
        "id": "1462702590710348",
        "first_name": "Seay",
        "gender": "male",
        "last_name": "Xu",
        "link": "https://www.facebook.com/app_scoped_user_id/1462702590710348/",
        "locale": "zh_CN",
        "name": "Seay Xu",
        "timezone": 8,
        "updated_time": "2015-06-30T05:50:43+0000",
        "verified": true
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
                Dictionary<string, object> result = Conf.GetDictionary(UserBaseInfoUrl, Access_Token);
                if (result != null && result.Count > 0)
                {
                    this.UserBaseInfo = new GitHubUserInfo()
                    {
                        login = result["login"].ToString(),
                        id = result["id"].ToString(),
                        avatar_url = result["avatar_url"].ToString(),
                        gravatar_id = result["gravatar_id"].ToString(),
                        url = result["url"].ToString(),
                        html_url = result["html_url"].ToString(),
                        followers_url = result["followers_url"].ToString(),
                        following_url = result["following_url"].ToString(),
                        gists_url = result["gists_url"].ToString(),
                        starred_url = result["starred_url"].ToString(),
                        subscriptions_url = result["subscriptions_url"].ToString(),
                        organizations_url = result["organizations_url"].ToString(),
                        repos_url = result["repos_url"].ToString(),
                        events_url = result["events_url"].ToString(),
                        received_events_url = result["received_events_url"].ToString(),
                        type = result["type"].ToString(),
                        site_admin = result["site_admin"].ToString(),
                        name = result["name"].ToString(),
                        company = result["company"].ToString(),
                        blog = result["blog"].ToString(),
                        location = result["location"].ToString(),
                        email = result["email"].ToString(),
                        hireable = result["hireable"].ToString(),
                        bio = result["bio"] != null ? result["bio"].ToString() : null,
                        public_repos = result["public_repos"].ToString(),
                        public_gists = result["public_gists"].ToString(),
                        followers = result["followers"].ToString(),
                        following = result["following"].ToString(),
                        created_at = result["created_at"].ToString(),
                        updated_at = result["updated_at"].ToString()
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