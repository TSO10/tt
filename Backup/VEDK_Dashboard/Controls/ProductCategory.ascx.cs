using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Data.Layer;
using VEDK_Dashboard.pricing;
using System.Web.UI.HtmlControls;

namespace VEDK_Dashboard.Controls
{
    public partial class ProductCategory : System.Web.UI.UserControl
    {
        private int _selectedProductId;

        #region Properties

        public VEDK_ProductCategory Category
        {
            get
            {
                return ViewState["Category"] == null ? null : (VEDK_ProductCategory)ViewState["Category"];
            }
            set
            {
                ViewState["Category"] = value;
                CategoryId = value.id;

            }
        }

        public bool ProductsOpen
        {
            get
            {
                return ViewState["ProductsOpen"] == null ? false : (bool)ViewState["ProductsOpen"];
            }
            set
            {
                ViewState["ProductsOpen"] = value;
            }
        }

        public string CategoryId
        {
            get
            {
                return ViewState["CategoryId"] == null ? string.Empty : (string)ViewState["CategoryId"];
            }
            set
            {
                ViewState["CategoryId"] = value;
            }
        }

        public string RegionId
        {
            get
            {
                return ViewState["RegionId"] == null ? string.Empty : (string)ViewState["RegionId"];
            }
            set
            {
                ViewState["RegionId"] = value;
            }
        }

        public DateTime SelectedDate
        {
            get
            {
                return ViewState["SelectedDate"] == null ? DateTime.Now : (DateTime)ViewState["SelectedDate"];
            }
            set
            {
                ViewState["SelectedDate"] = value;
            }
        }

        private List<VEDK_SupplierTbl> _suppliers = null;
        public List<VEDK_SupplierTbl> Suppliers
        {
            get
            {
                if (_suppliers == null)
                {
                    _suppliers = SuppliersRepository.GetList();
                }
                return _suppliers;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void DataBindCategory(bool showproducts, int productId)
        {
            base.DataBind();
            this._selectedProductId = productId;
            if (Category != null)
            {
                lnkCategoryName.Text = "+ " + Category.Name;
                if (showproducts)
                {
                    lnkCategoryName_Click(lnkCategoryName, null);
                }
            }
        }

        protected void lnkCategoryName_Click(object sender, EventArgs e)
        {
            if (!ProductsOpen)
            {
                lnkCategoryName.Text = lnkCategoryName.Text.Replace("+", "-");
                ProductsOpen = true;
                var products = ProductsRepository.GetByCategoryId(CategoryId, RegionId, SelectedDate);
                spInfoNoProducts.Visible = products.Count == 0;
                pnlProducts.Visible = !spInfoNoProducts.Visible;
                lSuppliersHeaders.Text = string.Empty;
                foreach (var supplier in Suppliers)
                {
                    lSuppliersHeaders.Text += string.Format("<th scope=\"col\">{0}</th>", supplier.Name);
                }
                rProducts.DataSource = products;
                rProducts.DataBind();
            }
            else
            {
                pnlProducts.Visible = false;
                spInfoNoProducts.Visible = false;
                lnkCategoryName.Text = lnkCategoryName.Text.Replace("-", "+");
                ProductsOpen = pnlProducts.Visible = false;
            }
            (this.Page as price).ShowCurrentChart();
        }

        protected void rProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var product = e.Item.DataItem as VEDK_Product;
            decimal? bestPrice = null;
            bool isinclude = false;
            var prices = SuppliersRepository.GetByProductId(product.VEDK_ProductID, Suppliers, SelectedDate);
            if (Category.ProductGroupMember.HasValue)
            {
                switch (Category.ProductGroupMember.Value)
                {
                    case 1:
                        bestPrice = prices.Where(p => p.LastPrice != null && p.LastPrice.Price.HasValue).Max(p => p.LastPrice.Price);
                        break;
                    case 2:
                        bestPrice = prices.Where(p => p.LastPrice != null && p.LastPrice.Price.HasValue).Min(p => p.LastPrice.Price);
                        break;
                }
            }

            if (_selectedProductId != 0 && product.VEDK_ProductID == _selectedProductId)
            {
                var chart = e.Item.FindControl("sCart") as SuppliersChart;
                chart.ProductId = product.VEDK_ProductID;
                chart.BindChart();
                chart.Visible = true;
                var lnkShowChart = e.Item.FindControl("lnkShowChart") as LinkButton;
                lnkShowChart.Text = "-";
            }
            Literal ltSuppliersInfo = e.Item.FindControl("ltSuppliersInfo") as Literal;
            ltSuppliersInfo.Text = string.Empty;
            foreach (var item in prices)
            {
                ltSuppliersInfo.Text += "<td>";
                if (item.LastPrice != null)
                {
                    string cssClass = string.Empty;
                    if (!isinclude && item.LastPrice.Price == bestPrice)
                    {
                        cssClass = " class=\"best-price\"";
                        isinclude = true;
                    }
                    ltSuppliersInfo.Text += string.Format("<span" + cssClass + ">{0} øre</span>", item.LastPrice.Price.HasValue ?
                        item.LastPrice.Price.Value.ToString("N2") : string.Empty);

                    if (item.PreviousPrice != null && item.LastPrice.Price != item.PreviousPrice.Price)
                    {
                        ltSuppliersInfo.Text += string.Format("<img src=\"/Images/{0}\"/>", (item.LastPrice.Price < item.PreviousPrice.Price ?
                           "arrow_down.png" : "arrow_up.png"));

                        //set #1 image
                        if (Math.Abs(item.LastPrice.Price.Value - item.PreviousPrice.Price.Value) > 1.5M)
                        {
                            ltSuppliersInfo.Text += "<img src=\"/Images/exmark.png\" width=\"20\" />";
                        }
                    }

                    //set #2 image
                    foreach (var subitem in prices)
                    {
                        if (subitem.LastPrice != null && subitem.LastPrice.Price.HasValue &&
                            (item.LastPrice.Price.Value - subitem.LastPrice.Price.Value) > 2M)
                        {
                            ltSuppliersInfo.Text += "<img src=\"/Images/stop.png\" width=\"20\"/>";
                            break;
                        }
                    }
                }
                ltSuppliersInfo.Text += "</td>";
            }
        }

        protected void rProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ShowChart")
            {
                var lnkShowChart = e.Item.FindControl("lnkShowChart") as LinkButton;
                var page = this.Page as price;
                var chart = e.Item.FindControl("sCart") as SuppliersChart;
                (this.Page as price).ChartStartDate = null;
                (this.Page as price).ChartEndDate = null;
                if (lnkShowChart.Text == "+")
                {
                    HideCharts();
                    page.SelectedProductId = chart.ProductId = int.Parse((string)e.CommandArgument);
                    chart.BindChart();
                    chart.Visible = true;
                    page.SelectedCategoryProductId = this.CategoryId;
                    lnkShowChart.Text = "-";
                    var spChart = e.Item.FindControl("spChart") as HtmlControl;
                    spChart.Attributes.Add("class", "display-slider");
                }
                else
                {
                    chart.Visible = false;
                    page.SelectedCategoryProductId = string.Empty;
                    page.SelectedProductId = 0;
                    lnkShowChart.Text = "+";
                }
            }
        }

        public void ShowChart()
        {
            foreach (RepeaterItem row in rProducts.Items)
            {
                var chart = row.FindControl("sCart") as SuppliersChart;
                var lnkShowChart = row.FindControl("lnkShowChart") as LinkButton;
                if (chart != null && chart.Visible)
                {
                    lnkShowChart.Text = "-";
                    chart.BindChart();
                }
            }
        }

        public void HideCharts()
        {
            foreach (RepeaterItem row in rProducts.Items)
            {
                var chart = row.FindControl("sCart") as SuppliersChart;
                if (chart != null && chart.Visible)
                {
                    chart.Visible = false;
                    var lnkShowChart = row.FindControl("lnkShowChart") as LinkButton;
                    lnkShowChart.Text = "+";
                }
            }
        }

    }
}