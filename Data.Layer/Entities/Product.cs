using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Data.Layer
{
    [MetadataType(typeof(VEDK_Product.Metadata))]
    [ScaffoldTable(true)]
    [DisplayName("Product")]
    partial class VEDK_Product
    {
        private abstract class Metadata
        {
            [Display(Name = "VEDK ProductID")]
            [ScaffoldColumn(true)] 
            public object VEDK_ProductID { get; set; }

            [Display(AutoGenerateField = false)]
            public object productId { get; set; }

            [Display(Name = "Product Name")]
            [Required()]
            public object productName { get; set; }

            [Display(Name = "Product Start Date")]
            [DataType(DataType.Date)]
            [Required()]
            public object productStartDate { get; set; }

            [Display(Name = "Product End Date")]
            [DataType(DataType.Date)]
            [Required()]
            public object productEndDate { get; set; }

            [Display(Name = "Product Description")]
            public object productDescription { get; set; }

            [Display(Name = "Region")]
            public object VEDK_Region { get; set; }

            [Display(Name = "Product Type")]
            public object VEDK_AgreementIDTbl { get; set; }

            [Display(AutoGenerateField = false)]
            public object ProductDescriptionLink { get; set; }

            [Display(Name = "Active")]
            public object Active { get; set; }


            [Display(AutoGenerateField = false)]
            public object balanceIncluded { get; set; }

            [Display(AutoGenerateField = false)]
            public object VEDK_Price { get; set; }

            [Display(AutoGenerateField = false)]
            public object Shortname { get; set; }

            [Display(AutoGenerateField = false)]
            public object Shortdescription { get; set; }

            [Display(AutoGenerateField = false)]
            public object ExclusiveSupplierID { get; set; }

            [Display(AutoGenerateField = false)]
            public object isFastpris { get; set; }

            [Display(AutoGenerateField = false)]
            public object TilbagekøbsProductTypeID { get; set; }
        }
    }
}
