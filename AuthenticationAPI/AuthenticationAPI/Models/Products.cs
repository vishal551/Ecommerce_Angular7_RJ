using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthenticationAPI.Models
{
    public class Products
    {
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public string ProductName { get; set; }
        public Nullable<decimal> ProductQuantity { get; set; }
        public Nullable<decimal> ProductPrice { get; set; }
        public Nullable<bool> InStock { get; set; }
        public string ProductImage { get; set; }
        public string ProductDescription { get; set; }
    }
}