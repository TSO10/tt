using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Layer
{
    public class ProductsRepository
    {
        public static List<VEDK_Product> GetByCategoryId(string id, string regionId, DateTime date)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                return (from product in entities.VEDK_Product.Include("VEDK_Region")
                        where product.VEDK_AgreementIDTbl.ProductCategory == id && (string.IsNullOrEmpty(regionId) ||
                        product.regionId == regionId) && product.Active &&
                        product.VEDK_Price.Count(p => p.CreateDate.HasValue && p.CreateDate.Value.Year == date.Year && p.CreateDate.Value.Month == date.Month && p.CreateDate.Value.Day == date.Day) > 0
                        orderby product.productName
                        select product).ToList();
            }
        }

        public static List<VEDK_Product> GetAll()
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                return entities.VEDK_Product.Include("VEDK_Region").Include("VEDK_AgreementIDTbl").ToList();
            }
        }

        public static VEDK_Product Get(int id)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                return entities.VEDK_Product.FirstOrDefault(p => p.VEDK_ProductID == id);
            }
        }
    }
}
