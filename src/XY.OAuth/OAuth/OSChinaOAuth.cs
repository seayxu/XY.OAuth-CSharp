using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using XY.OAuth.Model;
using XY.Utility;

namespace XY.OAuth
{
	public class OSChinaOAuth
    {
        #region 变量属性
        /// <summary>
        /// 基本的URL
        /// </summary>
        private static readonly string BasetUrl = "https://www.oschina.net/";
        /// <summary>
        /// 请求的URL
        /// </summary>
        private static string AuthorizeUrl = BasetUrl + "action/oauth2/authorize?response_type=code&client_id={0}&redirect_uri={1}&state=1";
        /// <summary>
        /// 验证口令的接口
        /// </summary>
        private static string TokenUrl = BasetUrl + "action/openapi/token?response_type=code&client_id={0}&client_secret={1}&grant_type=authorization_code&redirect_uri={2}&code={3}&dataType={4}";
        /// <summary>
        /// 获取用户基本信息接口
        /// </summary>
        private static string UserBaseInfoUrl = BasetUrl + "action/openapi/user?access_token={0}&dataType={1}";
        /// <summary>
        /// 获取用户详细信息接口
        /// </summary>
        private static string UserDetailInfoUrl = BasetUrl + "/action/openapi/my_information?access_token={0}&dataType={1}";

        /// <summary>
        /// access_token值
        /// </summary>
        private string Access_Token;
        /// <summary>
        /// refresh_token值
        /// </summary>
        private string Refresh_Token;
        /// <summary>
        /// access_token类型
        /// </summary>
        private string Token_Type;
        /// <summary>
        /// 超时时间(单位秒)
        /// </summary>
        private int Expires_In;
        /// <summary>
        /// 授权用户的uid
        /// </summary>
        public int UID;
        /// <summary>
        /// 开源中国用户基本信息类
        /// </summary>
        public OSCUserBaseInfo UserBaseInfo;
        /// <summary>
        /// 开源中国用户详细信息类
        /// </summary>
        public OSCUserDetailInfo UserDetailInfo; 
        #endregion

        //1.OpenAPI 授权登录页面
        #region 请求参数
        /*
         * 请求参数
        字段	            必选  	类型及范围	说明	            默认值
        client_id	    true	string	    OAuth2客户ID	
        response_type	true	string	    返回数据类型	    code
        redirect_uri	true	string	    回调地址	
        state	        false	string	    可选参数	
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
                string url = string.Format(AuthorizeUrl, Conf.OSC_Client_ID, Conf.OSC_Callback_URL);
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
        字段	            必选  类型及范围	说明	                        默认值
        client_id	    true	string	OAuth2客户ID	
        client_secret	true	string	OAuth2密钥	
        grant_type	    true	string	授权方式：authorization_code或者refresh_token	authorization_code
        redirect_uri	true	string	回调地址	
        code	        true	string	调用 /action/oauth2/authorize 接口返回的授权码(grant_type为authorization_code时必选)	
        refresh_token	false	string	上次调用 /action/oauth2/token 接口返回的refresh_token(grant_type为refresh_token时必选)	
        dataType	    true	string	返回数据类型['json'|'jsonp'|'xml']	json
        callback	    false	string	dataType为 jsonp 时用来指定回调函数	json
         * ****************************************************************************
         * 返回字段说明
        返回值字段	    类型及范围	说明
        access_token	string	    access_token值
        refresh_token	string	    refresh_token值
        token_type	    string	    access_token类型
        expires_in	    int	        超时时间(单位秒)
        uid	            int	        授权用户的uid
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
                string json = Http.Get(string.Format(TokenUrl, Conf.OSC_Client_ID, Conf.OSC_Client_Secret, Conf.OSC_Callback_URL, code, "json"));
                Dictionary<string, object> result = JsonHelper.Deserialize<Dictionary<string, object>>(json);
                //Dictionary<string, object> result = R.PostDictionary(TokenUrl, R.OSC_Client_ID, R.OSC_Client_Secret, R.OSC_Callback_URL, code,"json");
                if (result != null && result.Count > 0)
                {
                    this.Access_Token = result["access_token"].ToString();
                    this.Refresh_Token = result["refresh_token"].ToString();
                    this.Token_Type = result["token_type"].ToString();
                    this.Expires_In = int.Parse(result["expires_in"].ToString());
                    this.UID = int.Parse(result["uid"].ToString()); 
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
        字段            必选	    类型及范围	说明	                                默认值
        access_token    true	string	    oauth2_token获取的access_token	
        dataType	    true	string	    返回数据类型['json'|'jsonp'|'xml']	json
         * ************************************8888
         *返回字段说明
        字段	       类型及范围	说明
        error	    string	    错误
        error_description	string	错误描述
        id	        string	    用户ID
        email	    string	    用户email
        name	    string	    用户名
        gender	    string	    性别
        avatar	    string	    头像
        location	string	    地点
        url	        string	    主页 
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
                Dictionary<string, object> result = Conf.GetDictionary(UserBaseInfoUrl, Access_Token, "json");
                if (result!=null && result.Count>0)
                {
                    UserBaseInfo = new OSCUserBaseInfo()
                    {
                        id = result["id"].ToString(),
                        email = result["email"].ToString(),
                        name = result["name"].ToString(),
                        gender = result["gender"].ToString(),
                        avatar = result["avatar"].ToString(),
                        location = result["location"].ToString(),
                        url = result["url"].ToString()
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

        //4.获取用户详细信息
        #region 请求参数
        /*
         返回字段说明
        字段	            类型及范围	    说明
        uid	            long	        被查询用户id
        name	        string	        用户名称
        gender	        int	            性别：1-男，2-女
        province	    string	        省份
        city	        string	        城市
        platforms	    array	        开发平台
        expertise	    array	        专长领域
        joinTime	    datetime	    加入时间
        lastLoginTime	datetime	    最近登录时间
        portrait	    string	        头像
        fansCount	    string	        粉丝数
        favoriteCount	string	        收藏数
        followersCount	string	        关注数
        notice.replyCount	int	        未读评论数
        notice.msgCount	int	            未读留言数
        notice.fansCount	int	        新增粉丝数
        notice.referCount	int	        未读@我数
         */

        #endregion

        #region 获取用户详细信息
        /// <summary>
        /// 获取用户详细信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetUserDetailInfo()
        {
            try
            {
                Dictionary<string, object> result = Conf.GetDictionary(UserDetailInfoUrl, Access_Token, "json");
                string json = result["notice"].ToString();
                Dictionary<string, object> notice = JsonHelper.Deserialize<Dictionary<string, object>>(json);
                if (result != null && result.Count>0)
                {
                    UserDetailInfo = new OSCUserDetailInfo()
                    {
                        uid = long.Parse(result["uid"].ToString()),
                        name = result["name"].ToString(),
                        gender = int.Parse(result["gender"].ToString()),
                        province = result["province"].ToString(),
                        city = result["city"].ToString(),
                        platforms = ((string [])result["platforms"]).ToArray<string>(),
                        expertise = ((string[])result["expertise"]).ToArray<string>(),
                        joinTime = result["joinTime"].ToString(),
                        lastLoginTime = result["lastLoginTime"].ToString(),
                        portrait = result["portrait"].ToString(),
                        fansCount = result["fansCount"].ToString(),
                        favoriteCount = result["favoriteCount"].ToString(),
                        followersCount = result["followersCount"].ToString(),
                        replyCount = int.Parse(notice["replyCount"].ToString()),
                        msgCount = int.Parse(notice["msgCount"].ToString()),
                        newFansCount = int.Parse(notice["fansCount"].ToString()),
                        referCount = int.Parse(notice["referCount"].ToString())
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