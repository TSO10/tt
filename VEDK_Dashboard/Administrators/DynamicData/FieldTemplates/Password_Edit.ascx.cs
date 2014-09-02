using System;
using System.Collections.Specialized;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Marathon
{
    public partial class Password_EditField : FieldTemplateUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Column.MaxLength < 20)
            {
                TextBox1.Columns = Column.MaxLength;
            }
            TextBox1.ToolTip = Column.Description;

            if (this.Mode.ToString() != "Edit")
            {
                SetUpValidator(RequiredFieldValidator1);
                SetUpValidator(RegularExpressionValidator1);
                SetUpValidator(DynamicValidator1);
            }
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            TextBox1.MaxLength = Math.Max(FieldValueEditString.Length, Column.MaxLength);
        }

        protected override void ExtractValues(IOrderedDictionary dictionary)
        {
                if (TextBox1.Text.Length > 0)
                    dictionary[Column.Name] = ConvertEditedValue(TextBox1.Text);
        }
        public override Control DataControl
        {
            get
            {
                return TextBox1;
            }
        }
    }
}