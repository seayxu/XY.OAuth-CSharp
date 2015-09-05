using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XY.OAuth.Web.Controllers
{
    public class CallbackController : Controller
    {
        //
        // GET: /CallbackController/

        public ActionResult Index()
        {
            return View();
        }

		public JsonResult Sina(string code,string state)
		{
			SinaOAuth m = new SinaOAuth();
			JsonResult json = new JsonResult();
			json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
			string token = m.Token(code, state);
			if (!string.IsNullOrEmpty(token))
			{
				var user = m.GetUserInfo();
				json.Data = new object[] { user };
			}
			return json;
		}

		public JsonResult QQ(string code, string state)
		{
			QQOAuth m = new QQOAuth();
			JsonResult json = new JsonResult();
			json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
			string token = m.Token(code, state);
			if (!string.IsNullOrEmpty(token))
			{
				string openid = m.OpenID();
				if (!string.IsNullOrEmpty(openid))
				{
					var user = m.GetUserInfo();
					json.Data = new object[] { user }; 
				}
			}
			return json;
		}

		public JsonResult Baidu(string code, string state)
		{
			BaiduOAuth m = new BaiduOAuth();
			JsonResult json = new JsonResult();
			json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
			string token = m.Token(code, state);
			if (!string.IsNullOrEmpty(token))
			{
				var user = m.GetUserInfo();
				json.Data = new object[] { user };
			}
			return json;
		}
    }
}
