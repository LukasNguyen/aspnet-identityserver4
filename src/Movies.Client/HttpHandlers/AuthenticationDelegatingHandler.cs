using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Movies.Client.HttpHandlers
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        // Use authentication flow approach
        //private readonly IHttpClientFactory httpClientFactory;
        //private readonly ClientCredentialsTokenRequest tokenRequest;

        //public AuthenticationDelegatingHandler(IHttpClientFactory httpClientFactory, ClientCredentialsTokenRequest tokenRequest)
        //{
        //    this.httpClientFactory = httpClientFactory;
        //    this.tokenRequest = tokenRequest;
        //}

        //protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        //{
        //    var httpClient = httpClientFactory.CreateClient("IDPClient");

        //    var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(tokenRequest);

        //    if (tokenResponse.IsError)
        //    {
        //        throw new HttpRequestException("Something went wrong while requesting the access token");
        //    }

        //    request.SetBearerToken(tokenResponse.AccessToken);

        //    return await base.SendAsync(request, cancellationToken);
        //}

        // Use hybrid approach
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthenticationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                request.SetBearerToken(accessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
