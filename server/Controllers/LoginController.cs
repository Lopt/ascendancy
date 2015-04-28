using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;

using @base.model;
using server.Models;

namespace server.Controllers
{
	public class LoginController : Controller
	{

        // Schnittstelle ansprechen mit http://localhost:23287/Login?_ID=4
        //[HttpPost]
        //public string LoginID(string _ID)
        //{
        //    var mvcName = typeof(Controller).Assembly.GetName();
        //    var isMono = Type.GetType("Mono.Runtime") != null;

        //    ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
        //    ViewData["Runtime"] = isMono ? "Mono" : ".NET";

        //    return HttpUtility.HtmlEncode("Hello  NumTimes is: " + _ID); ;
        //}
             
      //  [HttpPost]
        public async Task<List<User>> LoginID(string ID)
        {
			return null;
/*            DBLogin LoginQuery = new DBLogin();
            List<User> UserGuid =  await LoginQuery.GetUserLoginID(ID);
            return UserGuid;*/
        }

	}
}

