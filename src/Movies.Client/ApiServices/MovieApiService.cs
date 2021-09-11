using IdentityModel.Client;
using Movies.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Movies.Client.ApiServices
{
    public class MovieApiService : IMovieApiService
    {
        private readonly IHttpClientFactory httpClientFactory;

        public MovieApiService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
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