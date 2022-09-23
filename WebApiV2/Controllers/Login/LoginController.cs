using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApiV2.Controllers
{
    [EnableCors("*", "*", "*")]
    public class LoginController : ApiController
    {
        [HttpPost] 

        // POST: api/Login
        //[AllowAnonymous]
        public async Task<IHttpActionResult> LoginAsync(Users usuarioLogin)
        {
            if (usuarioLogin == null)
                return BadRequest("Usuario y Contraseña requeridos.");

            var _userInfo = await AutenticarUsuarioAsync(usuarioLogin.username, usuarioLogin.password);

            if (_userInfo != null)
            {
                return Ok(new { token = GenerarTokenJWT(_userInfo) });
            }
            else
            {
                return Unauthorized();
            }
        }

        // COMPROBAMOS SI EL USUARIO EXISTE EN LA BASE DE DATOS 
        Users users;
        private async Task<Users> AutenticarUsuarioAsync(string username, string password)
        {
            // LÓGICA DE AUTENTICACIÓN //

            if (username == "" || username == null && password == null || password == "")
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            else
            {
                //pregunta si existe el usuario con nombre y contraseña
                //retorna el usuario para que se genere el token

                //IQueryable comprobar = Enumerable.Empty<Users>().AsQueryable();
                IEnumerable<Users> comprobar;
                using (JWTAuthentication0Entities db = new JWTAuthentication0Entities())
                {
                    db.Database.Connection.Open();
                    comprobar = from b in db.Users where b.username == username && b.password == password select b;
                    //IEnumerable<string> query = (IEnumerable<string>)comprobar.AsQueryable();
                    //var user = db.Users.Where(obj => obj.username == usuarioDatos.username && obj.password == usuarioDatos.password).ToList();
                    //var existeUsuario = db.Users.Include('').Where(d => d.username== usuarioDatos.username )
                    if (comprobar != null && comprobar.Any())
                    {
                        foreach (var item in comprobar)
                        {
                            users = new Users()
                            {
                                Id = item.Id,
                                Nombre = item.Nombre,
                                Apellidos = item.Apellidos,
                                Email = item.Email,
                                Rol = item.Rol
                            };
                        }
                    }
                    else return null;
                }
            }
            return users;
        }
        // GENERAMOS EL TOKEN CON LA INFORMACIÓN DEL USUARIO
        private string GenerarTokenJWT(Users usuarioInfo)
        {
            // RECUPERAMOS LAS VARIABLES DE CONFIGURACIÓN
            var _ClaveSecreta = ConfigurationManager.AppSettings["ClaveSecreta"];
            var _Issuer = ConfigurationManager.AppSettings["Issuer"];
            var _Audience = ConfigurationManager.AppSettings["Audience"];
            if (!Int32.TryParse(ConfigurationManager.AppSettings["Expires"], out int _Expires))
                _Expires = 11;
            //24


            // CREAMOS EL HEADER //
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_ClaveSecreta));
            var _signingCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );
            var _Header = new JwtHeader(_signingCredentials);

            // CREAMOS LOS CLAIMS //
            var _Claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, usuarioInfo.Id.ToString()),
                new Claim("nombre", usuarioInfo.Nombre),
                new Claim("apellidos", usuarioInfo.Apellidos),
                new Claim(JwtRegisteredClaimNames.Email, usuarioInfo.Email),
                new Claim(ClaimTypes.Role, usuarioInfo.Rol)
            };

            // CREAMOS EL PAYLOAD //
            var _Payload = new JwtPayload(
                    issuer: _Issuer,
                    audience: _Audience,
                    claims: _Claims,
                    notBefore: DateTime.UtcNow,
                    // Exipra a la 24 horas, 1h
                    expires: DateTime.UtcNow.AddHours(_Expires)
                );

            // GENERAMOS EL TOKEN //
            var _Token = new JwtSecurityToken(
                    _Header,
                    _Payload
                );

            return new JwtSecurityTokenHandler().WriteToken(_Token);
        }
    }

}
