using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XY.OAuth.Model
{
    public class GitHubUserInfo
    {
        public string login { get; set; } //octocat
        public string id { get; set; } // 1,
        public string avatar_url { get; set; } //https://github.com/images/error/octocat_happy.gif
        public string gravatar_id { get; set; } //
        public string url { get; set; } //https://api.github.com/users/octocat
        public string html_url { get; set; } //https://github.com/octocat
        public string followers_url { get; set; } //https://api.github.com/users/octocat/followers
        public string following_url { get; set; } //https://api.github.com/users/octocat/following{/other_user}
        public string gists_url { get; set; } //https://api.github.com/users/octocat/gists{/gist_id}
        public string starred_url { get; set; } //https://api.github.com/users/octocat/starred{/owner}{/repo}
        public string subscriptions_url { get; set; } //https://api.github.com/users/octocat/subscriptions
        public string organizations_url { get; set; } //https://api.github.com/users/octocat/orgs
        public string repos_url { get; set; } //https://api.github.com/users/octocat/repos
        public string events_url { get; set; } //https://api.github.com/users/octocat/events{/privacy}
        public string received_events_url { get; set; } //https://api.github.com/users/octocat/received_events
        public string type { get; set; } //User
        public string site_admin { get; set; } //false
        public string name { get; set; } //monalisa octocat
        public string company { get; set; } //GitHub
        public string blog { get; set; } //https://github.com/blog
        public string location { get; set; } //San Francisco
        public string email { get; set; } //octocat@github.com
        public string hireable { get; set; } //false
        public string bio { get; set; } //There once was...
        public string public_repos { get; set; } //2
        public string public_gists { get; set; } //1,
        public string followers { get; set; } //20,
        public string following { get; set; } //0,
        public string created_at { get; set; } //2008-01-14T04:33:35Z
        public string updated_at { get; set; } //2008-01-14T04:33:35Z
        public string total_private_repos { get; set; } //100,
        public string owned_private_repos { get; set; } //100,
        public string private_gists { get; set; } //81,
        public string disk_usage { get; set; } //10000,
        public string collaborators { get; set; } //8
        public string plan_name { get; set; } //Medium
        public string plan_space { get; set; } //400,
        public string plan_private_repos { get; set; } //20
        public string plan_collaborators { get; set; } //0
    }
}