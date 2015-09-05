using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XY.OAuth.Model
{
    /// <summary>
    /// 新浪微博用户详细信息类
    /// </summary>
    public class OSCUserDetailInfo
    {
        /// <summary>
        /// 被查询用户id
        /// </summary>
        public long uid { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 性别：1-男，2-女
        /// </summary>
        public int gender { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 开发平台
        /// </summary>
        public string[] platforms { get; set; }
        /// <summary>
        /// 专长领域
        /// </summary>
        public string[] expertise { get; set; }
        /// <summary>
        /// 加入时间
        /// </summary>
        public string joinTime { get; set; }
        /// <summary>
        /// 最近登录时间
        /// </summary>
        public string lastLoginTime { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string portrait { get; set; }
        /// <summary>
        /// 粉丝数
        /// </summary>
        public string fansCount { get; set; }
        /// <summary>
        /// 收藏数
        /// </summary>
        public string favoriteCount { get; set; }
        /// <summary>
        /// 关注数
        /// </summary>
        public string followersCount { get; set; }
        /// <summary>
        /// 未读评论数
        /// </summary>
        public int replyCount { get; set; }
        /// <summary>
        /// 未读留言数
        /// </summary>
        public int msgCount { get; set; }
        /// <summary>
        /// 新增粉丝数
        /// </summary>
        public int newFansCount { get; set; }
        /// <summary>
        /// 未读@我数
        /// </summary>
        public int referCount { get; set; }
    }
}