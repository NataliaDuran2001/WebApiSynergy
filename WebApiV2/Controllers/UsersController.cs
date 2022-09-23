using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiV2.Models;

namespace WebApiV2.Controllers
{
    [Authorize]
    [EnableCors("*", "*", "*")]
    public class UsersController : ApiController
    {
        // GET: api/Users
        public IEnumerable<Users> Get()
        {
            using(JWTAuthentication0Entities db = new JWTAuthentication0Entities())
            {
                
                return db.Users.ToList();
            }
        }

        // GET: api/Users/5
        /*public string Get(int id)
        {
            return "value";
        }

        // POST: api/Users
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Users/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Users/5
        public void Delete(int id)
        {
        }*/
    }
}
