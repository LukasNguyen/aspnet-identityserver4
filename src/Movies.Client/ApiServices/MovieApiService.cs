﻿using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Movies.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Movies.Client.ApiServices
{
    public class MovieApiService : IMovieApiService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor;

        public MovieApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientFactory = httpClientFactory;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            var httpClient = httpClientFactory.CreateClient("MovieAPIClient");

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/movies/");
            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Movie>>(content);

            //// 1 - Get Token from Identity Server
            //// 2 - Send Request to Protected API
            //// 3- Deserialize object to movie list

            //// 1. "retrieve" our api credentials. This must be registered on IdentityServer!
            //var apiClientCredentials = new ClientCredentialsTokenRequest
            //{
            //    Address = "https://localhost:5005/connect/token",
            //    ClientId = "movieClient",
            //    ClientSecret = "secret",

            //    //This is the scope of our Protected API requires.
            //    Scope = "movieAPI"
            //};

            //// create a new HttpClient to talk to our IdentityServer (localhost:5005)
            //var client = new HttpClient();

            //// just check if we can reach the Discovery document. Not 100% needed but...
            //var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5005");
            //if (disco.IsError)
            //{
            //    return null; // throw 500 error
            //}

            //// 2. Authenticate and get access token from Identity Server
            //var tokenResponse = await client.RequestClientCredentialsTokenAsync(apiClientCredentials);
            //if (tokenResponse.IsError)
            //{
            //    return null;
            //}

            //// 2 - Send Request to Protected API

            //// Another HttpClient for talking now with our Protected API
            //var apiClient = new HttpClient();

            //// 3. Set the access_token in the request Authorization Bearer <token>
            //apiClient.SetBearerToken(tokenResponse.AccessToken);

            //// 4. Send a request to our Protected API
            //var response = await apiClient.GetAsync("https://localhost:5001/api/movies");
            //response.EnsureSuccessStatusCode();

            //var content = await response.Content.ReadAsStringAsync();

            //// Deserialize Object to MovieList
            //return JsonConvert.DeserializeObject<List<Movie>>(content);
        }

        public async Task<UserInfoViewModel> GetUserInfo()
        {
            var idpClient = httpClientFactory.CreateClient("IDPClient");

            var metadataResponse = await idpClient.GetDiscoveryDocumentAsync();

            if (metadataResponse.IsError)
            {
                throw new HttpRequestException("Something went wrong while requesting the access token");
            }

            var accessToken = await httpContextAccessor
                .HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var userInfoResponse = await idpClient.GetUserInfoAsync(new UserInfoRequest
            {
                Address = metadataResponse.UserInfoEndpoint,
                Token = accessToken
            });

            var userInfoDictionary = new Dictionary<string, string>();

            foreach(var claim in userInfoResponse.Claims)
            {
                userInfoDictionary.Add(claim.Type, claim.Value);
            }

            return new UserInfoViewModel(userInfoDictionary);
        }

        public Task<Movie> CreateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMovie(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> GetMovie(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> UpdateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}