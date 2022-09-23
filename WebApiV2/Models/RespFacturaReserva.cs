using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiV2.Models
{
    public class RespFacturaReserva
    {
        //public FacturaReservaModel f_reserva { get; set; }
        //public DetailFacturaReservaModel detail_f_reserva { get; set; }
        //public int delivered_quantity { get; set; }

        public string Doc_id { get; set; }
        public string Doc_num { get; set; }
        public string Doc_date { get; set; }
        public string Cli_code { get; set; }
        public string cli_name { get; set; }
        public string Doc_status { get; set; }
        public string Line_num { get; set; }
        public string Item_code { get; set; }
        public string Quantity { get; set; }
        public string Slope_quantity { get; set; }
        public string Delivered { get; set; }
        public string Line_status { get; set; }




    }
}