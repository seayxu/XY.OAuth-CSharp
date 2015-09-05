using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XY.OAuth.Web.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }

		public ActionResult Sina()
		{
			string url = SinaOAuth.AuthorizeURL("",false);
			return RedirectPermanent(url);
		}

		public ActionResult QQ(string t)
		{
			string url = QQOAuth.AuthorizeURL(t);
			return RedirectPermanent(url);
		}

		public ActionResult Baidu(string t)
		{
			string url = BaiduOAuth.AuthorizeURL(1);
			return RedirectPermanent(url);
		}
    }
}
