using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RoomPlustWebAPI.App_Code
{
    public class ConstantValues
    {
        public static string ClientId
        {
            get
            {
                if (HttpContext.Current.Session["_ClientId"] != null)
                    return HttpContext.Current.Session["_ClientId"].ToString();
                else
                    return null;               
            }
            set
            {
                HttpContext.Current.Session["_ClientId"] = value;
            }
        }

        public static string SecrectKey
        {
            get
            {
                if (HttpContext.Current.Session["_SecrectKey"] != null)
                    return HttpContext.Current.Session["_SecrectKey"].ToString();
                else
                    return null;
            }
            set
            {
                HttpContext.Current.Session["_SecrectKey"] = value;
            }
        }

		public static string TIME_REMARK_NOW = "Now";
    }
}