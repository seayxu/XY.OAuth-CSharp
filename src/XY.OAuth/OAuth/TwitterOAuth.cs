using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XY.OAuth.Model;
using XY.Utility;

namespace XY.OAuth
{
	public class TwitterOAuth
    {
        #region 变量属性
        /// <summary>
        /// 基本的URL
        /// </summary>
        private static readonly string BasetUrl = "https://github.com/";
        /// <summary>
        /// API的URL
        /// </summary>
        private static readonly string APIUrl = "https://api.github.com/";

        /// <summary>
        /// 请求的URL
        /// </summary>
        private static string AuthorizeUrl = BasetUrl + "login/oauth/authorize?response_type=code&client_id={0}&redirect_uri={1}&scope={2}&state=1216";
        /// <summary>
        /// 验证口令的接口
        /// </summary>
        private static string TokenUrl = BasetUrl + "login/oauth/access_token?client_id={0}&client_secret={1}&redirect_uri={2}&code={3}";
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
        /// 当前登录用户的数字ID
        /// </summary>
        public string UID;
        /// <summary>
        /// scope,权限列表
        /// </summary>
        public string Scope;
        /// <summary>
        /// 当前登录用户的头像
        /// small image: http://tb.himg.baidu.com/sys/portraitn/item/{$portrait} 
        /// large image: http://tb.himg.baidu.com/sys/portrait/item/{$portrait}
        /// </summary>
        public string Portrait;
        /// <summary>
        /// 百度用户基本信息类
        /// </summary>
        public GitHubUserInfo UserBaseInfo;
        #endregion

        //1.OpenAPI 授权登录页面
        #region 请求参数
        /*
         * 请求参数
         Parameters
            force_login
            Forces the user to enter their credentials to ensure the correct users account is authorized.

            screen_name
            Prefills the username input box of the OAuth login screen with the given value.
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
                string url = string.Format(AuthorizeUrl, Conf.GitHub_Client_ID, Conf.GitHub_Callback_URL, "");
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
                string token = Http.Post(string.Format(TokenUrl, Conf.GitHub_Client_ID, Conf.GitHub_Client_Secret, Conf.GitHub_Callback_URL, code), null);
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
                            case "scope":
                                this.Scope = kv[1];
                                break;
                            case "token_type":
                                this.Token_Type = kv[1];
                                break;
                        }
                    }
                }
                Dictionary<string, object> result = new Dictionary<string, object>();
                //Dictionary<string, object> result = R.PostDictionary(TokenUrl, R.GitHub_Client_ID, R.GitHub_Client_Secret, R.GitHub_Callback_URL, code,"json");
                if (!string.IsNullOrEmpty(this.Access_Token))
                {
                    result.Add("access_token", this.Access_Token);
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
        {
          "login": "octocat",
          "id": 1,
          "avatar_url": "https://github.com/images/error/octocat_happy.gif",
          "gravatar_id": "",
          "url": "https://api.github.com/users/octocat",
          "html_url": "https://github.com/octocat",
          "followers_url": "https://api.github.com/users/octocat/followers",
          "following_url": "https://api.github.com/users/octocat/following{/other_user}",
          "gists_url": "https://api.github.com/users/octocat/gists{/gist_id}",
          "starred_url": "https://api.github.com/users/octocat/starred{/owner}{/repo}",
          "subscriptions_url": "https://api.github.com/users/octocat/subscriptions",
          "organizations_url": "https://api.github.com/users/octocat/orgs",
          "repos_url": "https://api.github.com/users/octocat/repos",
          "events_url": "https://api.github.com/users/octocat/events{/privacy}",
          "received_events_url": "https://api.github.com/users/octocat/received_events",
          "type": "User",
          "site_admin": false,
          "name": "monalisa octocat",
          "company": "GitHub",
          "blog": "https://github.com/blog",
          "location": "San Francisco",
          "email": "octocat@github.com",
          "hireable": false,
          "bio": "There once was...",
          "public_repos": 2,
          "public_gists": 1,
          "followers": 20,
          "following": 0,
          "created_at": "2008-01-14T04:33:35Z",
          "updated_at": "2008-01-14T04:33:35Z",
          "total_private_repos": 100,
          "owned_private_repos": 100,
          "private_gists": 81,
          "disk_usage": 10000,
          "collaborators": 8,
          "plan": {
            "name": "Medium",
            "space": 400,
            "private_repos": 20,
            "collaborators": 0
          }
        }
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
                //string json = result["plan"].ToString();
                //Dictionary<string, object> plan = JsonHelper.Deserialize<Dictionary<string, object>>(json); 
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
                        updated_at = result["updated_at"].ToString(),
                        //total_private_repos = result["total_private_repos"].ToString(),
                        //owned_private_repos = result["owned_private_repos"].ToString(),
                        //private_gists = result["private_gists"].ToString(),
                        //disk_usage = result["disk_usage"].ToString(),
                        //collaborators = result["collaborators"].ToString(),
                        //plan_name = plan["name"].ToString(),
                        //plan_space = plan["space"].ToString(),
                        //plan_private_repos = plan["private_repos"].ToString(),
                        //plan_collaborators = plan["collaborators"].ToString()
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