using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiV2.Models
{
    public class DetailEntregaModel
    {
        public string Doc_id { get; set; }
        public string Item_code { get; set; }
        public string Base_entry { get; set; }
        public string Base_line { get; set; }
        public string Quantity { get; set; }
    }
}