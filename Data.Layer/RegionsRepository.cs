using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Layer
{
    public class RegionsRepository
    {
        public static string GetIdByName(string name)
        {
            string res = string.Empty;
            using (VEDKEntities entities = new VEDKEntities())
            {
                var region = (from reg in entities.VEDK_Region
                              where reg.RegionName == name
                              select reg).FirstOrDefault();
                if (region != null)
                {
                    res = region.id;
                }
            }
            return res;
        }
        public static string GetById(string id)
        {
            string res = "All";
            using (VEDKEntities entities = new VEDKEntities())
            {
                var region = (from reg in entities.VEDK_Region
                              where reg.id == id
                              select reg.RegionName).FirstOrDefault();
                if (region != null)
                {
                    res = region;
                }
            }
            return res;
        }
    }
}
