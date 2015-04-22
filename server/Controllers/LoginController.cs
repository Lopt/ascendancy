using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Collections.Concurrent;
using System.Net.Http;

namespace server.Controllers
{
	public class LoginController : Controller
	{
        [HttpPost]
		public string LoginID (string _ID)
		{
			var mvcName = typeof(Controller).Assembly.GetName ();
			var isMono = Type.GetType ("Mono.Runtime") != null;

			ViewData ["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
			ViewData ["Runtime"] = isMono ? "Mono" : ".NET";
           
			return HttpUtility.HtmlEncode("Hello  NumTimes is: " + _ID);;
		}
        
	}
}

