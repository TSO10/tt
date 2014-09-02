using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VEDK_Dashboard
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                if (Page.User.IsInRole("admin"))
                {
                    NavigationMenu.Items.Add(new MenuItem { Text = "Products", NavigateUrl = "~/VEDK_Product/ListDetails.aspx" });
                    NavigationMenu.Items.Add(new MenuItem { Text = "Users", NavigateUrl = "~/users.aspx" });
                }
                NavigationMenu.Items.Add(new MenuItem { Text = "Passwords", NavigateUrl = "~/VEDK_Password/ListDetails.aspx" });
            }
            else
            {
                NavigationMenu.Visible=false;
            }

        }
    }
}
