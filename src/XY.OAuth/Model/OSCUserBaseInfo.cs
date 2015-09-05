using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XY.OAuth.Model
{
    /// <summary>
    /// 新浪微博用户基本信息类
    /// </summary>
    public class OSCUserBaseInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string id { get; set;}
        /// <summary>
        /// email
        /// </summary>
        public string email { get; set;}
        /// <summary>
        /// 用户名
        /// </summary>
        public string name { get; set;}
        /// <summary>
        /// 性别
        /// </summary>
        public string gender { get; set;}
        /// <summary>
        /// 头像
        /// </summary>
        public string avatar { get; set;}
        /// <summary>
        /// 地点
        /// </summary>
        public string location { get; set;}
        /// <summary>
        /// 主页
        /// </summary>
        public string url { get; set;}
    }
}