using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiV2.Models
{
    public class Users
    {
        //public int UserId { get; set; }
        //public Guid Id { get; set; }
        public int id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        //public DateTime CreatedDate { get; set; }
    }
}