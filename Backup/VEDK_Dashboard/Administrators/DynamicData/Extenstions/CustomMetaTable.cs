using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.DynamicData.ModelProviders;
using System.Web.UI.WebControls;
using Data.Layer;

namespace VEDK_Dashboard.Administrators.DynamicData.Extenstions
{
    public class CustomMetaTable : MetaTable
    {
        public CustomMetaTable(CustomMetaModel model, TableProvider provider)
            : base(model, provider)
        {
        }
    }
}