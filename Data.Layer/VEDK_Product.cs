using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Layer
{
    public partial class VEDK_Product
    {
        public string RegionName
        {
            get
            {
                return VEDK_Region.RegionName;
            }
        }
    }
}

