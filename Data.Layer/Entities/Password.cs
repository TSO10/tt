using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data.Objects;
using System.Data;

namespace Data.Layer
{
    [MetadataType(typeof(VEDK_Password.Metadata))]
    [ScaffoldTable(true)]
    [DisplayName("Password")]
    partial class VEDK_Password : ISavableObject
    {
        private abstract class Metadata
        {
            public object Reference { get; set; }

            [Display(Name = "Login")]
            [Required()]
            public object Login { get; set; }

            [Display(Name = "Password")]
            [Required()]
            public object Password { get; set; }

            [Display(Name = "Updated")]
            [DataType(DataType.DateTime)]
            [ReadOnly(true)]
            public object Updated { get; set; }

            [Display(Name = "Comments")]
            public object Comments { get; set; }
        }

        public void BeforeSave(ObjectStateEntry stateEntry)
        {
            if (stateEntry.State == EntityState.Modified)
            {
                this.Updated = DateTime.Now;
            }
        }
    }
}
