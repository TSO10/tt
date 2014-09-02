using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Data.Layer;
using System.Web.Security;

namespace VEDK_Dashboard
{
    public partial class Users : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUsers();
            }
        }
        private void BindUsers()
        {
            List<VEDK_User> users = UsersRepository.GetAll();
            gvUsers.DataSource = users;
            gvUsers.DataBind();
        }
        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                VEDK_User user = (VEDK_User)e.Row.DataItem;

                Label lblUserName = e.Row.FindControl("lblUserName") as Label;
                Label lblCreatedOn = e.Row.FindControl("lblCreatedOn") as Label;
                Label lblUpdateOn = e.Row.FindControl("lblUpdateOn") as Label;
                Label lblComments = e.Row.FindControl("lblComments") as Label;

                lblUserName.Text = user.UserName;
                lblComments.Text = user.Comments;
                lblCreatedOn.Text = user.DateCreated.ToString();
                lblUpdateOn.Text = user.DataUpdated.ToString();

                ImageButton btnEnable = e.Row.FindControl("btnAdmin") as ImageButton;
                if (user.Administrator)
                {
                    btnEnable.ImageUrl = "~/images/true.png";
                    btnEnable.AlternateText = "Click to disable";
                }
                else
                {
                    btnEnable.ImageUrl = "~/images/false.png";
                    btnEnable.AlternateText = "Click to enable";
                }
            }
        }
        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsers.PageIndex = e.NewPageIndex;
            BindUsers();
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            WebControl p = (e.CommandSource as WebControl).Parent as WebControl;
            int idx = 0;
            if (p != null)
            {
                while (!(p is GridViewRow))
                {
                    p = p.Parent as WebControl;
                }
                idx = (p as GridViewRow).RowIndex;

            }
            if (e.CommandName == "Editing")
            {
                Label lblUserName = gvUsers.Rows[idx].FindControl("lblUserName") as Label;
                BindDetails(lblUserName.Text);
                mvUsers.ActiveViewIndex = 1;
            }
            if (e.CommandName == "Deleting")
            {
                Label lblUserName = gvUsers.Rows[idx].FindControl("lblUserName") as Label;
                ((CustomMembershipProvider)Membership.Provider).DeleteUser(lblUserName.Text);
                BindUsers();
            }
            if (e.CommandName == "AdminDisable")
            {
                Label lblUserName = gvUsers.Rows[idx].FindControl("lblUserName") as Label;

                ((CustomRoleProvider)Roles.Provider).ChangeRole(lblUserName.Text);
                BindUsers();
            }
        }

        private void BindDetails(string email)
        {
            txtUsername.Text = email;
            txtUsername.Enabled = false;
            btnAdd.Visible = false;
            btnUpdate.Visible = true;
            VEDK_User user = UsersRepository.GetByEmail(email);
            chbAdmin.Checked = user.Administrator;
            txtUsername.Text = user.UserName;
            txtComments.Text = user.Comments;
        }

        private void ClearDetails()
        {
            txtUsername.Text = string.Empty;
            txtUsername.Enabled = true;
            txtConfPwd.Text = string.Empty;
            txtComments.Text = string.Empty;
            btnAdd.Visible = true;
            btnUpdate.Visible = false;
            chbAdmin.Checked = false;
        }
        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            ClearDetails();
            mvUsers.ActiveViewIndex = 1;
        }
        protected void mvUsers_ActiveViewChanged(object sender, EventArgs e)
        {
            txtPwd.Text = string.Empty;
            txtConfPwd.Text = string.Empty;
            lblError.Text = string.Empty;
            trError.Visible = false;
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPwd.Text) || !string.IsNullOrEmpty(txtConfPwd.Text))
            {
                if (txtPwd.Text == txtConfPwd.Text)
                {
                    if (Membership.GetUser(txtUsername.Text.Trim()) == null)
                    {
                        if (txtPwd.Text.Length >= Membership.Provider.MinRequiredPasswordLength)
                        {
                            ((CustomMembershipProvider)Membership.Provider).CreateUser(txtUsername.Text, txtPwd.Text, txtComments.Text, chbAdmin.Checked);
                            mvUsers.ActiveViewIndex = 0;
                            BindUsers();
                        }
                        else
                        {
                            trError.Visible = true;
                            lblError.Text = string.Format("Password should be at least {0} characters long.", Membership.Provider.MinRequiredPasswordLength);
                        }
                    }
                    else
                    {
                        trError.Visible = true;
                        lblError.Text = "The username is already in use.";
                    }
                }
                else
                {
                    trError.Visible = true;
                    lblError.Text = "The passwords you typed do not match.<br />Please retype the new password in both boxes.";
                }
            }
            else
            {
                trError.Visible = true;
                lblError.Text = "The password is required.<br />Please enter password in both boxes.";
            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtPwd.Text == txtConfPwd.Text)
            {
                if (txtPwd.Text.Length >= Membership.Provider.MinRequiredPasswordLength || string.IsNullOrEmpty(txtPwd.Text))
                {
                    if (!string.IsNullOrEmpty(txtPwd.Text))
                    {
                        MembershipUser user = Membership.GetUser(txtUsername.Text);
                        string tempPwd = user.ResetPassword();
                        user.ChangePassword(tempPwd, txtPwd.Text);
                    }
                    ((CustomMembershipProvider)Membership.Provider).UpdateUser(txtUsername.Text, txtComments.Text, chbAdmin.Checked);

                    mvUsers.ActiveViewIndex = 0;
                    BindUsers();
                }
                else
                {
                    trError.Visible = true;
                    lblError.Text = string.Format("Password should be at least {0} characters long.", Membership.Provider.MinRequiredPasswordLength);
                }
            }
            else
            {
                trError.Visible = true;
                lblError.Text = "The passwords you typed do not match.<br />Please retype the new password in both boxes.";
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mvUsers.ActiveViewIndex = 0;
        }
    }
}