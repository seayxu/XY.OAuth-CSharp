using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XY.OAuth.Model
{
    /// <summary>
    /// 新浪微博用户信息类
    /// </summary>
    public class SinaUserInfo
    {
        public long id { get; set; } //用户UID
        public string idstr { get; set; } //字符串型的用户UID
        public string screen_name { get; set; } //用户昵称
        public string name { get; set; } //友好显示名称
        public int province { get; set; } //用户所在省级ID
        public int city { get; set; } //用户所在城市ID
        public string location { get; set; } //用户所在地
        public string description { get; set; } //用户个人描述
        public string url { get; set; } //用户博客地址
        public string profile_image_url { get; set; } //用户头像地址（中图），50×50像素
        public string profile_url { get; set; } //用户的微博统一URL地址
        public string domain { get; set; } //用户的个性化域名
        public string weihao { get; set; } //用户的微号
        public string gender { get; set; } //性别，m：男、f：女、n：未知
        public int followers_count { get; set; } //粉丝数
        public int friends_count { get; set; } //关注数
        public int statuses_count { get; set; } //微博数
        public int favourites_count { get; set; } //收藏数
        public string created_at { get; set; } //用户创建（注册）时间
        public bool following { get; set; } //暂未支持
        public bool allow_all_act_msg { get; set; } //是否允许所有人给我发私信，true：是，false：否
        public bool geo_enabled { get; set; } //是否允许标识用户的地理位置，true：是，false：否
        public bool verified { get; set; } //是否是微博认证用户，即加V用户，true：是，false：否
        public int verified_type { get; set; } //暂未支持
        public string remark { get; set; } //用户备注信息，只有在查询用户关系时才返回此字段
        public object status { get; set; } //用户的最近一条微博信息字段 详细
        public bool allow_all_comment { get; set; } //是否允许所有人对我的微博进行评论，true：是，false：否
        public string avatar_large { get; set; } //用户头像地址（大图），180×180像素
        public string avatar_hd { get; set; } //用户头像地址（高清），高清头像原图
        public string verified_reason { get; set; } //认证原因
        public bool follow_me { get; set; } //该用户是否关注当前登录用户，true：是，false：否
        public int online_status { get; set; } //用户的在线状态，0：不在线、1：在线
        public int bi_followers_count { get; set; } //用户的互粉数
        public string lang { get; set; } //用户当前的语言版本，zh-cn：简体中文，zh-tw：繁体中文，en：英语
    }
}