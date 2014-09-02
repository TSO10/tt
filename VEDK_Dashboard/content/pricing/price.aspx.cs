using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Data.Layer;
using VEDK_Dashboard.Controls;

namespace VEDK_Dashboard.pricing
{

    public partial class price : System.Web.UI.Page
    {
        public const string DK1 = "DK1";
        public const string DK2 = "DK2";
        public string RegionId
        {
            get
            {
                return ViewState["MainRegionID"] == null ? string.Empty : (string)ViewState["MainRegionID"];
            }
            set
            {
                ViewState["MainRegionID"] = value;
            }
        }

        public int SelectedProductId
        {
            get
            {
                return ViewState["SelectedProducId"] == null ? 0 : (int)ViewState["SelectedProducId"];
            }
            set
            {
                ViewState["SelectedProducId"] = value;
            }
        }

        public string SelectedCategoryProductId
        {
            get
            {
                return ViewState["SelectedCategoryProducId"] == null ? string.Empty : (string)ViewState["SelectedCategoryProducId"];
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && ViewState["SelectedCategoryProducId"] != null &&
                    !((string)ViewState["SelectedCategoryProducId"]).Equals(value))
                {
                    ChangeChart((string)ViewState["SelectedCategoryProducId"]);
                }
                ViewState["SelectedCategoryProducId"] = value;
            }
        }

        public DateTime SelectedDate
        {
            get
            {
                return cSelectedDate.SelectedDate;
            }
        }

        public DateTime? ChartStartDate
        {
            get
            {
                return ViewState["ChartStartDate"] == null ? null : (DateTime?)ViewState["ChartStartDate"];
            }
            set
            {
                ViewState["ChartStartDate"] = value;
            }
        }

        public DateTime? ChartEndDate
        {
            get
            {
                return ViewState["ChartEndDate"] == null ? null : (DateTime?)ViewState["ChartEndDate"];
            }
            set
            {
                ViewState["ChartEndDate"] = value;
            }
        }

        Dictionary<string, bool> _openCategories;
        Dictionary<string, bool> OpenCategories
        {
            get
            {
                if (_openCategories == null)
                {
                    _openCategories = new Dictionary<string, bool>();
                }
                return _openCategories;
            }
        }

        public int IdOpenProduct
        {
            get
            {
                return ViewState["IdOpenProduct"] == null ? -1 : (int)ViewState["IdOpenProduct"];
            }
            set
            {
                ViewState["IdOpenProduct"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var categories = ProductCategoryRepository.GetList();
                chblCategories.DataSource = categories;
                chblCategories.DataBind();
                foreach (ListItem item in chblCategories.Items)
                {
                    item.Selected = true;
                }

                litSelectedDate.Text = DateTime.Now.ToString("d");
                cSelectedDate.SelectedDate = DateTime.Now;

                InitData();
            }
        }

        public void InitData()
        {
            DateTime openFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 0, 0);
            DateTime openTill = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 15, 0);
            imgStoreStatus.ImageUrl = (DateTime.Now >= openFrom && DateTime.Now <= openTill) ? "~/images/shop_open.gif" : "~/images/shop_closed.gif";
            litSelectedRegion.Text = RegionsRepository.GetById(this.RegionId);
            foreach (RepeaterItem item in rCategories.Items)
            {
                ProductCategory pc = item.FindControl("pcCategory") as ProductCategory;
                if (pc.ProductsOpen)
                {
                    OpenCategories.Add(pc.CategoryId, pc.ProductsOpen);
                }
            }
            var suppliers = SuppliersRepository.GetList();
            rSuppliers.DataSource = suppliers;
            rSuppliers.DataBind();

            var selectedIDs = chblCategories.Items.Cast<ListItem>()
                   .Where(item => item.Selected).Select(item => item.Value).ToList();

            rCategories.DataSource = ProductCategoryRepository.GetList().Where(c => selectedIDs.Contains(c.id)).ToList();
            rCategories.DataBind();
        }

        protected void rCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ProductCategory pc = e.Item.FindControl("pcCategory") as ProductCategory;
            pc.Category = e.Item.DataItem as VEDK_ProductCategory;
            pc.RegionId = RegionId;
            pc.SelectedDate = cSelectedDate.SelectedDate;
            int spid = !string.IsNullOrEmpty(SelectedCategoryProductId) && SelectedCategoryProductId == pc.Category.id ? SelectedProductId : 0;
            pc.DataBindCategory(OpenCategories.ContainsKey(pc.Category.id), spid);
        }

        protected void tUpdateLayout_Tick(object sender, EventArgs e)
        {
            InitData();
        }

        protected void rSuppliersRecieved_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var item = e.Item.DataItem as VEDK_SupplierTbl;
            Literal lblSupplier = e.Item.FindControl("lblSupplier") as Literal;
            lblSupplier.Text += item.Name + " - ";
            DateTime? p = SuppliersRepository.LastPrice(item.id, cSelectedDate.SelectedDate);
            if (p != null && p.HasValue)
            {
                lblSupplier.Text = "<b>" + lblSupplier.Text;
                lblSupplier.Text += p.Value.ToString("HH:mm") + " - # " + SuppliersRepository.TimesInfoReceived(item.id, cSelectedDate.SelectedDate);
                lblSupplier.Text += "</b>";
            }
            else
            {
                lblSupplier.Text += "No prices";
            }
        }

        public void ChangeChart(string categoryId)
        {
            foreach (RepeaterItem item in rCategories.Items)
            {
                ProductCategory pc = item.FindControl("pcCategory") as ProductCategory;
                if (pc.CategoryId == categoryId)
                {
                    pc.HideCharts();
                }
            }
        }

        public void ShowCurrentChart()
        {
            foreach (RepeaterItem item in rCategories.Items)
            {
                ProductCategory pc = item.FindControl("pcCategory") as ProductCategory;
                if (pc.CategoryId == SelectedCategoryProductId)
                {
                    pc.ShowChart();
                }
            }
        }

        protected void imgDK1_Click(object sender, ImageClickEventArgs e)
        {
            RegionId = RegionsRepository.GetIdByName(DK1);
            InitData();
        }

        protected void imgDK2_Click(object sender, ImageClickEventArgs e)
        {
            RegionId = RegionsRepository.GetIdByName(DK2);
            InitData();
        }

        protected void imgNone_Click(object sender, ImageClickEventArgs e)
        {
            RegionId = string.Empty;
            InitData();
        }

        protected void chblCategories_TextChanged(object sender, EventArgs e)
        {
            InitData();
        }

        protected void cSelectedDate_SelectionChanged(object sender, EventArgs e)
        {
            litSelectedDate.Text = cSelectedDate.SelectedDate.ToString("d");
            InitData();
        }

    }
}