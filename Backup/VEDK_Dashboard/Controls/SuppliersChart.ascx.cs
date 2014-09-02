using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using Data.Layer;
using VEDK_Dashboard.pricing;

namespace VEDK_Dashboard.Controls
{
    public partial class SuppliersChart : System.Web.UI.UserControl
    {
        private const int SliderMax = 1000;
        private const string Specifier = "0.00";
        private const int MaxLineHeight = 5;
        private const int CountShowPoint = 10;
        private const int CountTotalShowPoint = 200;
        private bool _defaultInit = false;
        private decimal? _maxPrice = 0;
        private List<CustomSupplier> _prices;

        #region Properties

        public DateTime? StartTime
        {
            get
            {
                return ViewState["TimeStart"] == null ? null : (DateTime?)ViewState["TimeStart"];
            }
            set
            {
                ViewState["TimeStart"] = value;
            }
        }

        public DateTime? EndTime
        {
            get
            {
                return ViewState["TimeEnd"] == null ? null : (DateTime?)ViewState["TimeEnd"];
            }
            set
            {
                ViewState["TimeEnd"] = value;
            }
        }

        public double TotalMilliseconds
        {
            get
            {
                return ViewState["TotalMilliseconds"] == null ? 0 : (double)ViewState["TotalMilliseconds"];
            }
            set
            {
                ViewState["TotalMilliseconds"] = value;
            }
        }

        public int ProductId
        {
            get
            {
                return ViewState["TchartProductId"] == null ? 0 :
                    (int)ViewState["TchartProductId"];
            }
            set
            {
                ViewState["TchartProductId"] = value;
            }
        }

        private Dictionary<string, VEDK_SupplierTbl> _suppliers = null;
        public Dictionary<string, VEDK_SupplierTbl> Suppliers
        {
            get
            {
                if (_suppliers == null)
                {
                    _suppliers = SuppliersRepository.GetDictionary();
                }
                return _suppliers;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindChart()
        {
            DateTime selectedTime = (this.Page as price).SelectedDate;
            DateTime selectedTimeStart = new DateTime(selectedTime.Year, selectedTime.Month, selectedTime.Day, 0, 0, 0);
            DateTime selectedTimeEnd = new DateTime(selectedTime.Year, selectedTime.Month, selectedTime.Day, 23, 59, 59);
            var firstPrice = PriceRepository.GetPriceFirstByProductId(ProductId, CountTotalShowPoint);
            var lastPrice = PriceRepository.GetPriceLastByProductId(ProductId);
            if (firstPrice != null && lastPrice != null)
            {
                StartTime = firstPrice.CreateDate;
                EndTime = lastPrice.CreateDate;

                SliderBeginHiddenValue.Value = "0";
                TotalMilliseconds = (EndTime.Value - StartTime.Value).TotalMilliseconds;
                SliderTotal.Value = SliderEndHiddenValue.Value = SliderMax.ToString();
                lblStartDate.Text = StartTime.Value.ToShortDateString();
                lblEndDate.Text = EndTime.Value.ToShortDateString();
                var ptimeStart = (this.Page as price).ChartStartDate;
                var ptimeEnd = (this.Page as price).ChartEndDate;

                if (ptimeStart.HasValue && ptimeEnd.HasValue)
                {
                    double c = TotalMilliseconds / SliderMax;
                    SliderBeginHiddenValue.Value = ((int)Math.Round(((ptimeStart.Value - StartTime.Value).TotalMilliseconds) / c, 2)).ToString();
                    SliderEndHiddenValue.Value = ((int)Math.Round(((ptimeEnd.Value - StartTime.Value).TotalMilliseconds) / c, 2)).ToString();
                }
                else
                {
                    _defaultInit = true;
                }

                ShowSelectedDatePriceValues(PriceRepository.GetMaxByProductId(ProductId, selectedTimeStart, selectedTimeEnd),
                PriceRepository.GetMinByProductId(ProductId, selectedTimeStart, selectedTimeEnd),
                PriceRepository.GetAverageByProductId(ProductId, selectedTimeStart, selectedTimeEnd));
                btnChangeChart_Click(null, null);
            }
        }

        private void CalculateData(decimal price, ref decimal max, ref decimal min, ref decimal sum, ref int count)
        {
            if (price > max)
            {
                max = price;
            }
            if (price < min)
            {
                min = price;
            }
            sum += price;
            count++;
        }

        private void ShowSelectedDatePriceValues(decimal? max, decimal? min, decimal? average)
        {
            if (max.HasValue)
            {
                lblSelectedDataMax.Text = max.Value.ToString(Specifier);
                lblSelectedDataMin.Text = min.Value.ToString(Specifier);
                lblSelectedDataAverage.Text = average.Value.ToString(Specifier);
            }
        }

        protected void btnChangeChart_Click(object sender, EventArgs e)
        {
            int index = 0;
            DateTime timeStart = DateTime.Now, timeend = DateTime.Now;
            List<VEDK_Price> prices = null;
            if (_defaultInit)
            {
                prices = PriceRepository.GetTopByProductId(ProductId, CountShowPoint);
                if (prices.Count > 0)
                {
                    timeStart = prices.First().CreateDate.Value;
                    timeend = prices.Last().CreateDate.Value;
                    double c = TotalMilliseconds / SliderMax;
                    SliderBeginHiddenValue.Value = ((int)Math.Round(((timeStart - StartTime.Value).TotalMilliseconds) / c, 2)).ToString();
                    SliderEndHiddenValue.Value = ((int)Math.Round(((timeend - StartTime.Value).TotalMilliseconds) / c, 2)).ToString();
                }
            }
            else
            {
                double c = TotalMilliseconds / SliderMax;
                timeStart = StartTime.Value.AddMilliseconds(int.Parse(SliderBeginHiddenValue.Value) * c);
                timeend = StartTime.Value.AddMilliseconds(int.Parse(SliderEndHiddenValue.Value) * c);
                prices = PriceRepository.GetPriceByProductId(ProductId, timeStart, timeend);
                (this.Page as price).ChartStartDate = timeStart;
                (this.Page as price).ChartEndDate = timeend;
            }

            BindOpeningTab((this.Page as price).SelectedDate);
            int countdata = 0;
            decimal max = 0, min = decimal.MaxValue, sum = 0;
            tChart.Series["sLine"].Points.Clear();
            tChart.Series["sPoint"].Points.Clear();
            double totalselectedtime = (timeend - timeStart).TotalMilliseconds;
            double totaltime = (EndTime.Value - StartTime.Value).TotalMilliseconds;
            int lineHeigth = (int)(Math.Round((1 - totalselectedtime / totaltime) * MaxLineHeight, 1));
            lineHeigth = lineHeigth == 0 ? 1 : lineHeigth;
            tChart.Series["sLine"].BorderWidth = lineHeigth;
            bool showPointLine = prices.Count <= CountShowPoint;
            foreach (var price in prices)
            {
                tChart.Series["sLine"].Points.AddXY(price.CreateDate.Value, price.Price.Value);
                if (showPointLine)
                {
                    tChart.Series["sPoint"].Points.AddXY(price.CreateDate.Value, price.Price.Value);
                    tChart.Series["sPoint"].Points[index].ToolTip = tChart.Series["sPoint"].Points[index].Label =
                        "#VALX{dd-MM-yy HH-mm}\n#VAL{N2} " + (Suppliers.ContainsKey(price.SupplierID) ? Suppliers[price.SupplierID].Name :
                        string.Empty);
                    index++;
                }
                CalculateData(price.Price.Value, ref max, ref min, ref sum, ref countdata);
            }

            if (countdata != 0)
            {
                double difference = (double)(max - min);
                tChart.ChartAreas["caChart"].AxisY.Interval = difference > 1.63 ? 0 : 0.5;
                tChart.ChartAreas["caChart"].AxisY.Maximum = Math.Ceiling((double)max + difference);
                tChart.ChartAreas["caChart"].AxisY.Minimum = Math.Ceiling((double)min - difference) - 1;
                if (tChart.ChartAreas["caChart"].AxisY.Maximum == tChart.ChartAreas["caChart"].AxisY.Minimum)
                {
                    tChart.ChartAreas["caChart"].AxisY.Minimum -= 1;
                    tChart.ChartAreas["caChart"].AxisY.Maximum += 1;
                }
                lblChartSelectedDateMax.Text = max.ToString(Specifier);
                lblChartSelectedDateMin.Text = min.ToString(Specifier);
                lblChartSelectedDateAverage.Text = (sum / countdata).ToString(Specifier);
                lblStartDate.Text = timeStart.ToShortDateString();
                lblEndDate.Text = timeend.ToShortDateString();
            }
            else
            {
                lblChartSelectedDateMax.Text = lblChartSelectedDateMin.Text = lblChartSelectedDateAverage.Text = "0.00";
            }
        }

        private void BindOpeningTab(DateTime date)
        {
            lblSelectedDate.Text = date.ToShortDateString();
            _prices = SuppliersRepository.GetByProductId(ProductId, Suppliers.Select(k => k.Value).ToList(), date);
            List<SupplierOpeningPrice> listOpeningPrice = SuppliersRepository.GetOpening(ProductId, date);
            var product = ProductsRepository.Get(ProductId);
            double totaldays = product != null && product.productStartDate.HasValue ?
                Math.Abs((product.productStartDate.Value - DateTime.Now).TotalDays) : 0;
            if (totaldays < 30)
            {
                _maxPrice = listOpeningPrice.Min(s => s.Price);
            }
            else
            {
                _maxPrice = listOpeningPrice.Max(s => s.Price);
            }
            grStatitics.DataSource = listOpeningPrice;
            grStatitics.DataBind();
        }

        protected void grStatitics_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var item = e.Row.DataItem as SupplierOpeningPrice;
                Literal ltPrice = e.Row.FindControl("ltPrice") as Literal;
                if (_maxPrice.Value == item.Price.Value)
                {
                    ltPrice.Text = "<b><span>" + item.Price.Value.ToString("N2") + "</span></b>" +
                        "<img src=\"/Images/customerprice.png\"/>";
                }
                else
                {
                    ltPrice.Text = item.Price.Value.ToString("N2");
                }

                decimal price = GetDifference(item);
                Literal ltDifference = e.Row.FindControl("ltDifference") as Literal;
                ltDifference.Text = string.Format("<span>{0}</span>", price.ToString("N2"));
                if (price != 0)
                {
                    ltDifference.Text += string.Format("<img src=\"/Images/{0}\"/>", (price < 0 ?
                            "arrow_down.png" : "arrow_up.png"));
                }
            }
        }

        private decimal GetDifference(SupplierOpeningPrice supplierOpeningPrice)
        {
            decimal cprice = 0;
            foreach (var item in _prices)
            {
                if (item.LastPrice != null && item.LastPrice.SupplierID == supplierOpeningPrice.SupplierId)
                {
                    if (item.LastPrice.Price.HasValue)
                    {
                        cprice = supplierOpeningPrice.Price.Value - item.LastPrice.Price.Value;
                    }
                    break;
                }
            }
            return cprice;
        }
    }
}