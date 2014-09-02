using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.DynamicData.ModelProviders;

namespace VEDK_Dashboard.Administrators.DynamicData.Extenstions
{
    public class CustomMetaModel : MetaModel
    {
        public CustomMetaModel()
        {
        }

        public CustomMetaModel(string dynamicDataFolderVirtualPath)
        {
            this.DynamicDataFolderVirtualPath = dynamicDataFolderVirtualPath;
        }

        protected override MetaTable CreateTable(TableProvider provider)
        {
            return new CustomMetaTable(this, provider);
        }

        public List<CustomMetaTable> AvailableTables
        {
            get
            {
                return this.VisibleTables.Cast<CustomMetaTable>().Where(t => t.Scaffold).ToList();
            }
        }
    }
}