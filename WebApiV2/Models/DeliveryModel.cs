using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiV2.Models
{
    public class DeliveryModel
    {
        public int id { get; set; }
        public int DocEntry { get; set; }
        //public int DocNum { get; set; }
        public string WarehouseCode { get; set; }
        //public int BaseEntry { get; set; }
        //public string CardCode { get; set; }
        public string NumAtCard { get; set; }
        public string SlpName { get; set; }
        public string U_Name { get; set; }
        public string CardName { get; set; }
        public string DocDate { get; set; }
        public string Comments { get; set; }
        public List<ItemModel> ItemList
        {
            get; set;
        }
    }
}