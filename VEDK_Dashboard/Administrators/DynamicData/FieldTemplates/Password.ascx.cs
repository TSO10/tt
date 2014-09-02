using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Marathon
{
    public partial class PasswordField : System.Web.DynamicData.FieldTemplateUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // add a string of '*' the max length of the field
            var length = Column.MaxLength > 20 ? 20 : Column.MaxLength;
            Literal1.Text = new String('*', length);
        }

        public override Control DataControl
        {
            get
            {
                return Literal1;
            }
        }
    }
}