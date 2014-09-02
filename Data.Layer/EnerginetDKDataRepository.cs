using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Objects.SqlClient;

namespace Data.Layer
{
    [DataContract]
    public class CustomEnerginetData
    {
        [DataMember]
        public int Date { get; set; }
        public string StrDateTime { get; set; }
        [DataMember]
        public string CentralPowerDK1 { get; set; }
        [DataMember]
        public string LocalCHPPowerDK1 { get; set; }
        [DataMember]
        public string WindTurbinesDK1 { get; set; }
    }

    public class EnerginetDKDataRepository
    {
        public static List<CustomEnerginetData> GetListByDate(int year, int month, int day)
        {
            string date = year + "-" + GetFullInt(month) + "-" + GetFullInt(day);
            using (VEDKEntities entities = new VEDKEntities())
            {
                var res = (from energinet in entities.EnerginetDKDatas
                           where date == energinet.Datetime.Trim().Substring(0, date.Length)
                           select new CustomEnerginetData
                           {
                               StrDateTime = energinet.Datetime,
                               CentralPowerDK1 = energinet.CentralPowerDK1,
                               LocalCHPPowerDK1 = energinet.LocaCHPDK1,
                               WindTurbinesDK1 = energinet.WindTurbinesDK1
                           }).ToList();
                res.ForEach(delegate(CustomEnerginetData energinet)
                {
                    DateTime d = DateTime.Now;
                    DateTime.TryParse(energinet.StrDateTime, out d);
                    energinet.Date = d.Hour;
                });
                return res.OrderBy(e => e.Date).ToList();
            }
        }

        private static string GetFullInt(int d)
        {
            return d.ToString().Length == 1 ? ("0" + d) : d.ToString();
        }
    }
}
