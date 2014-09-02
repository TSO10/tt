using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.DynamicData;
using System.Web.Routing;
using Data.Layer;
using System.Data.Entity;
using System.Web.UI;
using VEDK_Dashboard.Administrators.DynamicData.Extenstions;

namespace VEDK_Dashboard
{
    public class Global : System.Web.HttpApplication
    {
        private static CustomMetaModel _dynamicDataModel = new CustomMetaModel("~/Administrators/DynamicData");
        public static CustomMetaModel DynamicDataModel
        {
            get
            {
                return _dynamicDataModel;
            }
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            _dynamicDataModel.RegisterContext(ObjectContextFactory.GetContext, new ContextConfiguration() { ScaffoldAllTables = false });
            routes.Add(new DynamicDataRoute("{table}/{action}.aspx")
            {
                Constraints = new RouteValueDictionary(new { action = "ListDetails" }),
                Model = DynamicDataModel
            });
        }

        void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
