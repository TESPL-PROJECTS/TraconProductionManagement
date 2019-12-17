using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ManageRoles.MapperConfig;

namespace ManageRoles
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Application["TotalOnlineUsers"] = 0;
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfiguration.Config();
        }

        void Session_Start(object sender, EventArgs e)
        {
            Application.Lock();
            Application["TotalOnlineUsers"] = (int)Application["TotalOnlineUsers"] + 1;
            Application.UnLock();
        }

        void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["TotalOnlineUsers"] = (int)Application["TotalOnlineUsers"] - 1;
            if (Session["UserID"] != null)
            {
                SetOnlineUser(Session["UserID"].ToString());
            }
            Application.UnLock();
        }

        void SetOnlineUser(string userId)
        {
            Application.Lock();
            string user = "";
            if (Application["OnlineUsers"] != null)
            {
                user = (string)Application["OnlineUsers"];
                List<string> lstUser = user.Split(',').ToList();
                lstUser.Remove(userId);
                user = string.Join(",", lstUser);
            }
            Application["OnlineUsers"] = user;
            Application.UnLock();
        }
    }
}