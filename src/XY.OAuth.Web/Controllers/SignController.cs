using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace XY.OAuth.Web.Controllers
{
    public class SignController : Controller
    {
		public ActionResult On()//登录
		{
			return View();
		}

		public ActionResult Out()//登出
		{
			return View();
		}
    }
}
