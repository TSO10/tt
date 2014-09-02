using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Layer
{
    public class PriceRepository
    {
        public static List<VEDK_Price> GetPriceByProductId(int id)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                return (from price in entities.VEDK_Price
                        where price.VEDK_ProductID == id &&
                        price.CreateDate.HasValue && price.Price.HasValue
                        orderby price.CreateDate
                        select price).ToList();
            }
        }

        public static List<VEDK_Price> GetTopByProductId(int id, int top)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                return (from price in entities.VEDK_Price
                        where price.VEDK_ProductID == id &&
                        price.CreateDate.HasValue && price.Price.HasValue
                        orderby price.CreateDate descending
                        select price).Take(top).OrderBy(p => p.CreateDate).ToList();

            }
        }

        public static VEDK_Price GetPriceFirstByProductId(int id, int countrows)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                return (from price in entities.VEDK_Price
                        where price.VEDK_ProductID == id &&
                        price.CreateDate.HasValue && price.Price.HasValue
                        orderby price.CreateDate descending
                        select price).Take(countrows).OrderBy(p => p.CreateDate).FirstOrDefault();
            }
        }

        public static VEDK_Price GetPriceLastByProductId(int id)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                return (from price in entities.VEDK_Price
                        where price.VEDK_ProductID == id &&
                        price.CreateDate.HasValue && price.Price.HasValue
                        orderby price.CreateDate descending
                        select price).FirstOrDefault();
            }
        }

        public static decimal? GetMaxByProductId(int id, DateTime starttime, DateTime endtime)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                return (from price in entities.VEDK_Price
                        where price.VEDK_ProductID == id &&
                        price.CreateDate.HasValue && price.Price.HasValue &&
                        price.CreateDate >= starttime &&
                        price.CreateDate <= endtime
                        orderby price.CreateDate
                        select price).Max(p => p.Price);
            }
        }

        public static decimal? GetMinByProductId(int id, DateTime starttime, DateTime endtime)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                return (from price in entities.VEDK_Price
                        where price.VEDK_ProductID == id &&
                        price.CreateDate.HasValue && price.Price.HasValue &&
                        price.CreateDate >= starttime &&
                        price.CreateDate <= endtime
                        orderby price.CreateDate
                        select price).Min(p => p.Price);
            }
        }

        public static decimal? GetAverageByProductId(int id, DateTime starttime, DateTime endtime)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                return (from price in entities.VEDK_Price
                        where price.VEDK_ProductID == id &&
                        price.CreateDate.HasValue && price.Price.HasValue &&
                        price.CreateDate >= starttime &&
                        price.CreateDate <= endtime
                        orderby price.CreateDate
                        select price).Average(p => p.Price);
            }
        }


        public static List<VEDK_Price> GetPriceByProductId(int id, DateTime starttime, DateTime endtime)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                return (from price in entities.VEDK_Price
                        where price.VEDK_ProductID == id &&
                        price.CreateDate.HasValue && price.Price.HasValue &&
                        price.CreateDate >= starttime &&
                        price.CreateDate <= endtime
                        orderby price.CreateDate
                        select price).ToList();
            }
        }
    }
}
