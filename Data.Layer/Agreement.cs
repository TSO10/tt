using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Data.Layer
{
    [DataContract]
    public class Agreement
    {
        [DataMember]
        public int Agreement_ID { get; set; }

        [DataMember]
        public string Produktnavn { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal Pris { get; set; }

        [DataMember]
        public DateTime Dato_prissikring { get; set; }
    }
}
