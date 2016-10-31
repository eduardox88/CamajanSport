using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ApiCamajan.Manejadores;

[assembly: OwinStartup(typeof(ApiCamajan.Startup))]
namespace ApiCamajan
{
    
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            var config = new HttpConfiguration();
            //other configurations

            ConfigureOAuth(app);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(2),
                Provider = new AuthorizationServerProvider()
            };

            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }

    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            try
            {
                ManejadorUsuario manejador = new ManejadorUsuario();

                //retrieve your user from database. ex:
                var user = await manejador.Autenticar(context.UserName, context.Password);

                if (user == null)
                {
                    throw new ArgumentNullException();
                }

                //var user = await userService.Authenticate(context.UserName, context.Password); Ese era el ejemplo

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                identity.AddClaim(new Claim(ClaimTypes.Name, user.NombreUsuario));
                identity.AddClaim(new Claim(ClaimTypes.Role, user.rol.Nombre));

                List<string> roles = new List<string>();
                roles.Add(user.rol.Nombre);

                //identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

                //roles example
                //var rolesTechnicalNamesUser = new List<string>();

                //if (user.Roles != null)
                //{
                //    rolesTechnicalNamesUser = user.Roles.Select(x => x.TechnicalName).ToList();

                //    foreach (var role in user.Roles)
                //identity.AddClaim(new Claim(ClaimTypes.Role, role.TechnicalName));
                //       
                //}

                //var principal = new GenericPrincipal(identity, rolesTechnicalNamesUser.ToArray());

                var principal = new GenericPrincipal(identity, roles.ToArray());

                Thread.CurrentPrincipal = principal;

                context.Validated(identity);
            }
            catch (Exception ex)
            {
                context.SetError("invalid_grant", "message");
            }
        }
    }
}