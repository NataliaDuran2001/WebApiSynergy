using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
using System.Net;
using System.Configuration;
using SAPbobsCOM;

namespace WebApiV2.Controllers
{
    internal class TokenValidationHandler : DelegatingHandler
    {
        //recupera el token de la petición
        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            //IEnumerable<string> authzHeaders;

            if (!request.Headers.TryGetValues("Authorization", out IEnumerable<string> authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;

            //determina si jwt existe o no
            if (!TryRetrieveToken(request, out string token))
            {
                /*var resp = new ConexionSap();
                Company oCompany = resp.ConnectSap();
                if (oCompany.Connect() == 0)
                {
                    oCompany.Disconnect();
                }*/
                statusCode = HttpStatusCode.Unauthorized;
                return base.SendAsync(request, cancellationToken);
            }
            try
            {
                //var secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
                //var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
                //var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];

                var claveSecreta = ConfigurationManager.AppSettings["ClaveSecreta"];
                var issuerToken = ConfigurationManager.AppSettings["Issuer"];
                var audienceToken = ConfigurationManager.AppSettings["Audience"];

                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(claveSecreta));

                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = audienceToken,
                    ValidIssuer = issuerToken,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifetimeValidator,
                    IssuerSigningKey = securityKey

                };

                //extraer y asignar usuario actual
                //comprueba la validez del token

                Thread.CurrentPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
                HttpContext.Current.User = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                return base.SendAsync(request, cancellationToken);

            }
            catch (SecurityTokenValidationException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }
            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });

        }

        //comprueba la caducidad del token
        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            /*if(expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;*/
            var valid = false;
            if ((expires.HasValue && DateTime.UtcNow < expires) && (notBefore.HasValue && DateTime.UtcNow > notBefore))
            {
                valid = true;

            }
            return valid;
        }
    }
}