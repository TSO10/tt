using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Configuration;

namespace Data.Layer
{
    [DataContract]
    public class CustomMillLog
    {
        [DataMember]
        public int MillLogID { get; set; }

        [DataMember]
        public DateTime DateTime { get; set; }

        [DataMember]
        public string GSRN { get; set; }

        [DataMember]
        public float EffectReal { get; set; }

        [DataMember]
        public float EffectNorm { get; set; }

        [DataMember]
        public float WindSpeed { get; set; }

        [DataMember]
        public short Status { get; set; }

        public static CustomMillLog[] GetList(string gsrn, int top)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {                
                return entities.MillLogs.Where(m => string.IsNullOrEmpty(gsrn) || gsrn == m.GSRN).
                    OrderByDescending(s => s.DateTime).Take(top).Select(m => new CustomMillLog
                {
                    MillLogID = m.MillLogID,
                    DateTime = m.DateTime.HasValue ? m.DateTime.Value : DateTime.MinValue,
                    GSRN = m.GSRN,
                    EffectNorm = m.EffectNorm.HasValue ? m.EffectNorm.Value : 0,
                    EffectReal = m.EffectReal.HasValue ? m.EffectReal.Value : 0,
                    WindSpeed = m.WindSpeed.HasValue ? m.WindSpeed.Value : 0,
                    Status = m.Status.HasValue ? m.Status.Value : short.MinValue

                }).ToArray();
            }
        }
    }
}
