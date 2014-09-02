using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VEDK_Dashboard.GetParker;
using System.Reflection;
using System.Text;

namespace VEDK_Dashboard
{
    /// <summary>
    /// Summary description for parkerCsv
    /// </summary>
    public class parkerCsv : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Clear();
            context.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}.csv", context.Request["date"]));
            context.Response.Charset = "";
            // If you want the option to open the Excel file without saving than
            // comment out the line below
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.ContentType = "application/vnd.xls";
            GetParkerSoapClient client = new GetParkerSoapClient();
            client.Open();
            Park[] res = client.GetParker(DateTime.Parse(context.Request["date"]), Guid.Parse(context.Request["supplier"])).Parker;
            client.Close();
            context.Response.Write(ToCsv<Park>(",", res));
            context.Response.End();
        }

        public static string ToCsv<T>(string separator, IEnumerable<T> objectlist)
        {
            Type t = typeof(T);
            PropertyInfo[] fields = t.GetProperties().Where((x, i) => i > 0 && i < 6).ToArray();

            string header = String.Join(separator, fields.Select(f => f.Name).ToArray());

            StringBuilder csvdata = new StringBuilder();
            csvdata.AppendLine(header);

            foreach (var o in objectlist)
                csvdata.AppendLine(ToCsvFields(separator, fields, o));

            return csvdata.ToString();
        }

        public static string ToCsvFields(string separator, PropertyInfo[] props, object o)
        {
            StringBuilder linie = new StringBuilder();

            for (int i = 0; i < props.Length; i++)
            {
                if (linie.Length > 0)
                    linie.Append(separator);

                var x = props[i].GetValue(o, null);

                if (x != null)
                {
                    if (x is decimal)
                    {
                        x = linie.Append(x.ToString().Replace(',', '.'));
                    }
                    else
                    {
                        linie.Append(x.ToString());
                    }
                }
            }

            return linie.ToString();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}