using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Layer
{
    public class ProductCategoryRepository
    {
        public static List<VEDK_ProductCategory> GetList()
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                return (from category in entities.VEDK_ProductCategory
                        orderby category.id
                        select category).ToList();
            }
        }      
    }
}
