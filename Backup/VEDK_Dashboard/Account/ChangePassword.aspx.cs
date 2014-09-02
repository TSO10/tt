using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace VEDK_Dashboard.Account
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnChange_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                if (!string.IsNullOrEmpty(NewPassword.Text) && NewPassword.Text.Length < Membership.MinRequiredPasswordLength)
                {
                    FailureText.Text = "Passwords are required to be a minimum of " + Membership.MinRequiredPasswordLength + " characters in length.";
                    FailureText.Visible = true;
                }
                else
                {
                    MembershipUser muser = Membership.GetUser(User.Identity.Name);
                    bool success = muser.ChangePassword(CurrentPassword.Text, NewPassword.Text);
                    if (success)
                    {
                        Response.Redirect("~/default.aspx");
                    }
                    else
                    {
                        FailureText.Text = "Old password is not right.";
                        FailureText.Visible = true;
                    }
                }
            }
        }
    }
}
