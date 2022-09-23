using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiV2.Models
{
    public class DetailFacturaReservaModel
    {
        public string Doc_id { get; set; }
        public string Line_num { get; set; }
        public string Item_code { get; set; }
        public string Quantity { get; set; }
        public string Slope_quantity { get; set; }
        public string delivered { get; set; }
        public string Line_status { get; set; }

        public static implicit operator DetailFacturaReservaModel(List<DetailFacturaReservaModel> v)
        {
            throw new NotImplementedException();
        }

       
    }
}
