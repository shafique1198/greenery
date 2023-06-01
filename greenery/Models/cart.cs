using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace greenery.Models
{
    public class cart
    {
        public int productid { get; set; }
        public string productname { get; set; }

        public float price { get; set; }
        public int qty { get; set; }
        public float bill { get; set; }

        public string orderstatus { get; set; }
    }
}