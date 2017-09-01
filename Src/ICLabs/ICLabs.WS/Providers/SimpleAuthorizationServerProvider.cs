using ICLabs.Model;
using ICLabs.Services;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ICLabs.WS.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {

        /// <summary>
        /// IICLabsService icozumoservice
        /// </summary>
        private readonly IICLabsService iICLabsService;

        public SimpleAuthorizationServerProvider(IICLabsService iICLabsService)
        {
            if (iICLabsService == null)
            {
                throw new ArgumentNullException("iICLabsService");
            }

            this.iICLabsService = iICLabsService;
        }


        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            string clientId = string.Empty;
            string clientSecret = string.Empty;
            ClsApplication client = null;
           
            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (clientId == null)
                clientId = "NULL";
            iICLabsService.AddLog(clientId, "Authentication", "RemoteIP:" + context.Request.RemoteIpAddress + ";ClientId:" + clientId );

            if (context.ClientId == null)
            {                
                context.SetError("invalid_clientId", "ClientId should be sent.");
                return Task.FromResult<object>(null);
            }

            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                context.SetError("invalid_clientId", "Client secret should be sent.");
                return Task.FromResult<object>(null);
            }
            else
            {

                client =  iICLabsService.IsAuthenticated(clientId);

                if (client == null)
                {
                    context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system. ", context.ClientId));
                    return Task.FromResult<object>(null);
                }

                if (client.clientSecret != clientSecret)
            
                {
                    context.SetError("invalid_clientId", "Client secret is invalid.");
                    return Task.FromResult<object>(null);
                }
            }

            context.OwinContext.Set<string>("as:orgID", client.orgId);
            //context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var orgId = context.OwinContext.Get<string>("as:orgID");

            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            //using (AuthRepository _repo = new AuthRepository())
            //{
            //    IdentityUser user = await _repo.FindUser(context.UserName, context.Password);

            //    if (user == null)
            //    {
            //        context.SetError("invalid_grant", "The user name or password is incorrect.");
            //        return;
            //    }
            //}

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("clientId", context.ClientId));
            identity.AddClaim(new Claim("orgId",orgId));

            context.Validated(identity);

        }
    }
}