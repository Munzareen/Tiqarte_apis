using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using WebAPI.Models;

namespace WebAPI
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);
            ApplicationUser user = new ApplicationUser();
            try
            {
                user = await manager.FindByNameAsync(context.UserName);
                if (user != null)
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim("Username", user.UserName));
                    identity.AddClaim(new Claim("Email", user.Email));
                    identity.AddClaim(new Claim("FirstName", user.FirstName));
                    identity.AddClaim(new Claim("LastName", user.LastName));
                    identity.AddClaim(new Claim("LoggedOn", DateTime.Now.ToString()));
                    var userRoles = manager.GetRoles(user.Id);
                    var user1 = user;
                    foreach (string roleName in userRoles)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
                    }
                    var additionalData = new AuthenticationProperties(new Dictionary<string, string>{
                    {
                            
                        //"role", JsonConvert.SerializeObject(userRoles),
                       // "user",user.ToString()
                            "user",JsonConvert.SerializeObject(user)
                    }
                });
                    var token = new AuthenticationTicket(identity, additionalData);
                    context.Validated(token);
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}