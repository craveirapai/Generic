using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Generic.API.Providers;
using System.Configuration;
using Generic.API.Formatter;
using Microsoft.Owin.Security;
using System.Text;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;

namespace Generic.API
{
    public partial class Startup
    {
        public static string Issuer { get; private set; } = ConfigurationManager.AppSettings["Issuer"].ToString();
        public static string AudienceId { get; private set; } = ConfigurationManager.AppSettings["AudienceId"].ToString();
        public static string Base64Secret { get; private set; } = ConfigurationManager.AppSettings["Base64Secret"].ToString();
        
        public void ConfigureAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions authServerOptions = new OAuthAuthorizationServerOptions()
            {
                //Em produção se atentar que devemos usar HTTPS
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth2/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(15),
                Provider = new CustomOAuthProviderJwt(),
                AccessTokenFormat = new CustomJwtFormat(Issuer)
            };

            app.UseOAuthAuthorizationServer(authServerOptions);

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AllowedAudiences = new List<String>() { Startup.AudienceId },
                IssuerSecurityTokenProviders = new List<IIssuerSecurityTokenProvider>() { new SymmetricKeyIssuerSecurityTokenProvider(Startup.Issuer, TextEncodings.Base64Url.Decode(Startup.Base64Secret)) }
            });

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(authServerOptions);
        }
    }
}
