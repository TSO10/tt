using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Data.Layer
{
    public class CustomSupplier
    {
        public VEDK_Price PreviousPrice { get; set; }
        public VEDK_Price LastPrice { get; set; }
    }
    public class SupplierOpeningPrice
    {
        public string SupplierName { get; set; }
        public decimal? Price { get; set; }
        public DateTime? Date { get; set; }
        public string SupplierId { get; set; }
    }

    public class SuppliersRepository
    {
        public static List<VEDK_SupplierTbl> GetList(bool? active=null)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                return (from supplier in entities.VEDK_SupplierTbl
                        where (!active.HasValue && supplier.Active.HasValue && supplier.Active.Value == 1) || active.HasValue
                        orderby supplier.id
                        select supplier).ToList();
            }
        }


        public static Dictionary<string, VEDK_SupplierTbl> GetDictionary()
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                return (from supplier in entities.VEDK_SupplierTbl
                        where supplier.Active.HasValue && supplier.Active.Value == 1
                        orderby supplier.id
                        select supplier).ToDictionary(k => k.id, v => v);
            }
        }

        public static DateTime? LastPrice(string supplierId, DateTime date)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                return (from price in entities.VEDK_Price
                        where price.SupplierID == supplierId && price.CreateDate.HasValue && price.CreateDate.Value.Year == date.Year && price.CreateDate.Value.Month == date.Month && price.CreateDate.Value.Day == date.Day
                        orderby price.RecordID descending
                        select price.CreateDate).FirstOrDefault();
            }
        }

        public static int TimesInfoReceived(string supplierId, DateTime date)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                return (from price in entities.VEDK_Price
                        where price.SupplierID == supplierId && price.CreateDate.HasValue && price.CreateDate.Value.Year == date.Year && price.CreateDate.Value.Month == date.Month && price.CreateDate.Value.Day == date.Day
                        orderby price.RecordID descending
                        select price.transactionGUID).Distinct().Count();
            }
        }

        public static List<CustomSupplier> GetByProductId(int productId, List<VEDK_SupplierTbl> suppliers, DateTime date)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                var resPrices = new List<CustomSupplier>();
                foreach (var item in suppliers)
                {
                    var prices = (from price in entities.VEDK_Price
                                  where price.SupplierID == item.id && price.VEDK_ProductID == productId &&
                                  price.CreateDate.HasValue && price.CreateDate.Value.Year == date.Year && price.CreateDate.Value.Month == date.Month && price.CreateDate.Value.Day == date.Day
                                  orderby price.RecordID descending
                                  select price).Take(2).ToList();
                    var customSupplier = new CustomSupplier();
                    if (prices.Count > 0)
                    {
                        customSupplier.LastPrice = prices[0];
                        if (prices.Count > 1)
                        {
                            customSupplier.PreviousPrice = prices[1];
                        }
                    }
                    resPrices.Add(customSupplier);
                }
                return resPrices;
            }
        }

        public static List<SupplierOpeningPrice> GetOpening(int productId, DateTime data)
        {
            TimeSpan timeStart = TimeSpan.Parse(ConfigurationManager.AppSettings["OpeningStartTime"]);
            TimeSpan timeEnd = TimeSpan.Parse(ConfigurationManager.AppSettings["OpeningEndTime"]);
            DateTime start = new DateTime(data.Year, data.Month, data.Day, timeStart.Hours, timeStart.Minutes, timeStart.Seconds);
            DateTime end = new DateTime(data.Year, data.Month, data.Day, timeEnd.Hours, timeEnd.Minutes, timeEnd.Seconds);
            using (VEDKEntities entities = new VEDKEntities())
            {
                var suppliers = (from p in
                                     (
                                         (from price in entities.VEDK_Price
                                          join product in entities.VEDK_Product on price.VEDK_ProductID equals product.VEDK_ProductID
                                          where
                                            product.VEDK_ProductID == productId && price.CreateDate >= start && price.CreateDate <= end
                                          group new { price } by new
                                          {
                                              price.SupplierID
                                          } into g
                                          select new
                                          {
                                              g.Key.SupplierID,
                                              CreateDate = (DateTime?)g.Max(p => p.price.CreateDate)
                                          }))
                                 join pr in entities.VEDK_Price
                                       on new { VEDK_ProductID = productId, p.SupplierID, p.CreateDate }
                                   equals new { pr.VEDK_ProductID, pr.SupplierID, pr.CreateDate }
                                 join s in entities.VEDK_SupplierTbl on new { id = p.SupplierID } equals new { id = s.id }
                                 orderby
                                   s.Name
                                 select new SupplierOpeningPrice
                                 {
                                     SupplierName = s.Name,
                                     Date = (DateTime?)pr.CreateDate,
                                     Price = pr.Price,
                                     SupplierId = s.id
                                 }).ToList();
                return suppliers;
            }

        }
    }
}
