﻿using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;

namespace RoomPlusWebAPI
{
	public class Global : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}

		protected void Application_PostAuthorizeRequest()
		{
			System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
		}
	}
}
