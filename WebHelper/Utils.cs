using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebHelper
{
    public class Utils
    {
        public static string AppUri
        {
            get
            {
                return System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
            }
        }


        /// <summary>
        ///	Gets current application absolute Uri
        /// </summary>
        public static string AbsUri
        {
            get
            {
                return System.Web.HttpContext.Current.Request.Url.GetLeftPart(System.UriPartial.Authority) + AppUri;
            }
        }


        /// <summary>
        ///	Gets current application path
        /// </summary>
        public static string AppPath
        {
            get
            {
                string res = string.Empty;
                string uri = AppUri;
                if (uri == string.Empty)
                    uri = "/";
                try { res = System.Web.HttpContext.Current.Server.MapPath(uri); }
                catch { }
                return res;
            }
        }
    }
}
