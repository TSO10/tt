using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Data.Layer;
using VEDK_Dashboard.GetParker;
using System.Reflection;
using System.Text;
using System.Data;
using System.Reflection;

namespace VEDK_Dashboard
{
    public partial class Parkers : System.Web.UI.Page
    {
        bool isContent = false;
        string rowsCount = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {            
                ddlSuppliers.DataSource = SuppliersRepository.GetList(true);
                ddlSuppliers.DataBind();
                btnExport.Visible = false;
                lblRowsCount.Visible = false;
                ViewState["sortColumn"] = "";
                ViewState["sortDirection"] = "";
            }            
        }
        public void BindParkers()
        {
            GetParkerSoapClient client = new GetParkerSoapClient();
            client.Open();
            Park[] res = client.GetParker(DateTime.Parse(tbDate.Text), Guid.Parse(ddlSuppliers.SelectedValue)).Parker;
            client.Close();
            if (isContent)
            {
                rowsCount = "Rows count: " + Convert.ToString(res.Length);
                if (chbAllowPagign.Checked)
                {
                    parkersList.PageSize = 20;
                }
                else
                {
                    parkersList.PageSize = res.Length;
                }
            }
            if (!string.IsNullOrEmpty(rowsCount))
            {
                lblRowsCount.Visible = true;
                lblRowsCount.Text = rowsCount;
            }
            parkersList.DataSource = res;         
            parkersList.DataBind();
            DecimalFormatting();
        }       

        protected void getContent_Click(object sender, EventArgs e)
        {
            isContent = true;
            BindParkers();
            btnExport.Visible = true;
            lblRowsCount.Visible = true;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            FunExportToExcel(parkersList);
        }

        protected void parkersList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            btnExport.Visible = true;
            lblRowsCount.Visible = true;
            parkersList.PageIndex = e.NewPageIndex;
            BindParkers();
        }

        private void FunExportToExcel(GridView GrdView)
        {
            Response.Redirect("~/parkerCsv.ashx?date=" + tbDate.Text + "&supplier=" + ddlSuppliers.SelectedValue);
        }
        protected void parkersList_Sorting(object sender, GridViewSortEventArgs e)
        {
            GetParkerSoapClient client = new GetParkerSoapClient();
            client.Open();
            Park[] res = client.GetParker(DateTime.Parse(tbDate.Text), Guid.Parse(ddlSuppliers.SelectedValue)).Parker;
            client.Close();
            Type t =  typeof(Park);
            PropertyInfo[] fields = t.GetProperties().Where((x, i) => i > 0 && i < 6).ToArray();

            DataTable dt = new DataTable("Parks");
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].PropertyType.Name == "String")
                {
                    dt.Columns.Add(fields[i].Name, typeof(string));
                }
                else if (fields[i].PropertyType.Name == "Decimal")
                {
                    dt.Columns.Add(fields[i].Name, typeof(decimal));
                }
            }           

            for (int i = 0; i < res.Length; i++)
            {
                DataRow row = dt.NewRow();
                for (int j = 0; j < fields.Length; j++)
                {
                    row[fields[j].Name] =  fields[j].GetValue(res[i], null);
                }
                dt.Rows.Add(row);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            DataView dv = ds.Tables["Parks"].DefaultView;
           
            if (ViewState["sortColumn"].ToString() == e.SortExpression.ToString())
            {
                if (" ASC" == ViewState["sortDirection"].ToString())
                {
                    ViewState["sortDirection"] = " DESC";
                    dv.Sort = e.SortExpression + " DESC";                    
                }
                else
                {
                    ViewState["sortDirection"] = " ASC";
                    dv.Sort = e.SortExpression + " ASC";
                }
            }
            else
            {
                ViewState["sortColumn"] = e.SortExpression.ToString();
                ViewState["sortDirection"] = " ASC";
                dv.Sort = e.SortExpression + " ASC";
            }
            parkersList.DataSource = dv.Table;
            parkersList.DataBind();
            DecimalFormatting();
        }
        private void  DecimalFormatting ()
        {
            decimal KWEffect;
            for (int i = 0; i < parkersList.Rows.Count; i++)
            {
                foreach (TableCell c in parkersList.Rows[i].Cells)
                {
                    if (decimal.TryParse(c.Text, out KWEffect))
                    {
                        c.Text = String.Format("{0:F0}", KWEffect);
                    }
                }
            }
        }

        
    }
}