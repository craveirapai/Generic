using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Generic.Domain.Enums;
using Generic.Domain.Service;

namespace Generic.API.Providers
{
    public class CustomOAuthProviderJwt : OAuthAuthorizationServerProvider
    {
        private String AudienceId { get; set; } = Startup.AudienceId;
        private UserService UserService { get; set; } = new UserService();

        private readonly String FacebookPath = "https://graph.facebook.com/me?fields=id,name,email&access_token={0}";


        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            string symmetricKeyAsBase64 = string.Empty;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            context.Validated();

            return Task.FromResult<object>(null);
        }
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var email = context.UserName;
            var password = context.Password;

            var identity = new ClaimsIdentity("JWT");

            if (!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(password))
            {

                var user = this.UserService.GetByEmailAndPassword(email, password);

                if (user != null && user.IsEnabled)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
                    identity.AddClaim(new Claim("sub", user.Email));
                    identity.AddClaim(new Claim(ClaimTypes.Sid, user.Id.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Role, RoleEnum.user.ToString()));
                }
                else
                {
                        context.SetError("invalid_grant", "Usuário ou senha invalidos");
                        return Task.FromResult<object>(null);
                }
            }
            else
            {
                var facebookAccessToken = HttpContext.Current.Request.Form["facebookToken"].ToString();

                var fbUser = Task.Run(() => this.GetFacebookUser(facebookAccessToken)).Result;

                if (fbUser == null)
                {
                    context.SetError("invalid_grant", "Facebook token is invalid");
                    return Task.FromResult<object>(null);
                }

            }

            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                 {
                     "audience", this.AudienceId
                 }
            });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
            return Task.FromResult<object>(null);
        }

        private async Task<FacebookUser> GetFacebookUser(string facebookAccessToken)
        {
            var path = String.Format(this.FacebookPath, facebookAccessToken);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var uri = new Uri(path);
            var response = await client.GetAsync(path);

            FacebookUser facebook = null;
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                facebook = Newtonsoft.Json.JsonConvert.DeserializeObject<FacebookUser>(content);
            }


            return facebook;
        }

        public class FacebookUser
        {
            public string id { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string name { get; set; }
            public string email { get; set; }
        }
    }
}