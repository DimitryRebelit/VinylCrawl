using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using VinylCrawl.Discogs.Configuration;
using VinylCrawl.Discogs.Exceptions;
using VinylCrawl.Discogs.Responses;

namespace VinylCrawl.Discogs.Agents
{
    /// <summary>
    ///     Base http caller
    /// </summary>
    public abstract class BaseHttpAgent
    {
        protected static async Task<RetrieveRequestTokenResponse?> AccessTokenOAuth(string uri,
            string? authenticationHeaders = null)
        {
            var clientHandler = new HttpClientHandler();
            using var httpClient = new HttpClient(clientHandler);

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("OAuth", authenticationHeaders);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            httpClient.DefaultRequestHeaders.Add("User-Agent", "VinylCrawl");

            var url = $"{Settings.BaseUrl}{uri}";
            var result = await httpClient.PostAsync(url, null);

            if (!result.IsSuccessStatusCode)
                throw new ApiException(
                    $"[{result.StatusCode}] Error sending get request to {uri}",
                    await result.Content.ReadAsStringAsync().ConfigureAwait(false),
                    result.StatusCode
                );

            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            return !string.IsNullOrEmpty(content) ? new RetrieveRequestTokenResponse(content) : null;
        }

        protected static async Task<RetrieveRequestTokenResponse?> RequestTokenOAuth(string uri,
            string? authenticationHeaders = null)
        {
            var clientHandler = new HttpClientHandler();
            using var httpClient = new HttpClient(clientHandler);

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("OAuth", authenticationHeaders);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            httpClient.DefaultRequestHeaders.Add("User-Agent", "VinylCrawl");

            var url = $"{Settings.BaseUrl}{uri}";
            var result = await httpClient.GetAsync(url);

            if (!result.IsSuccessStatusCode)
                throw new ApiException(
                    $"[{result.StatusCode}] Error sending get request to {uri}",
                    await result.Content.ReadAsStringAsync().ConfigureAwait(false),
                    result.StatusCode
                );

            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            return !string.IsNullOrEmpty(content) ? new RetrieveRequestTokenResponse(content) : null;
        }


        /// <summary>
        ///     Sends a get request with the latest token found in the local database
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="token"></param>
        /// <param name="authenticationHeaders"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        protected static async Task<T?> GetWithAuthAsync<T>(string uri)
        {
            var token = await SecureStorage.GetAsync("oauth_token");
            var tokenSecret = await SecureStorage.GetAsync("oauth_secret");

            var clientHandler = new HttpClientHandler();
            using var httpClient = new HttpClient(clientHandler);

            var authorizationHeader =
               $"oauth_consumer_key=\"{Settings.ConsumerKey}\"," +
               $"oauth_nonce=\"{Guid.NewGuid()}\"," +
               $"oauth_token=\"{token}\"," +
               $"oauth_signature=\"{Settings.ConsumerSecret}&{tokenSecret}\", " +
               $"oauth_signature_method=\"PLAINTEXT\"," +
               $"oauth_timestamp=\"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}\"";

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("OAuth", authorizationHeader);

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.discogs.v2.discogs+json"));
            httpClient.DefaultRequestHeaders.Add("User-Agent", "VinylCrawl");

            var url = $"{Settings.BaseUrl}{uri}";
            var result = await httpClient.GetAsync(url);

            if (!result.IsSuccessStatusCode)
                throw new ApiException(
                    $"[{result.StatusCode}] Error sending get request to {uri}",
                    await result.Content.ReadAsStringAsync().ConfigureAwait(false),
                    result.StatusCode
                );

            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(content);
        }

        /// <summary>
        ///     Retrieve a stream for the api
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        protected static async Task<MemoryStream> GetStreamWithAuthAsync(string uri, string token)
        {
            var clientHandler = new HttpClientHandler();


#if DEBUG //  When we connect to localhost with https this workaround is used for ignoring certificate problems
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
#endif

            using var httpclient = new HttpClient(clientHandler);

            if (!string.IsNullOrEmpty(token))
                httpclient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

            var url = uri;
            var result = await httpclient.GetAsync(url);

            if (!result.IsSuccessStatusCode)
                throw new ApiException(
                    $"[{result.StatusCode}] Error sending get request to {uri}",
                    await result.Content.ReadAsStringAsync().ConfigureAwait(false),
                    result.StatusCode
                );

            var ms = new MemoryStream();
            await result.Content.CopyToAsync(ms);
            return ms;
        }

        /// <summary>
        ///     Generic method for posting data
        /// </summary>
        /// <param name="httpContent"></param>
        /// <param name="uri"></param>
        /// <param name="token"></param>
        /// <typeparam name="T">Object type that will be given back after the post</typeparam>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        protected static async Task<T?> PostAsync<T>(HttpContent httpContent, string uri, string token)
        {
            var clientHandler = new HttpClientHandler();
            using var httpClient = new HttpClient(clientHandler);

            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

            var url = $"{Settings.BaseUrl}{uri}";
            var result = await httpClient.PostAsync(url, httpContent);

            if (!result.IsSuccessStatusCode)
                throw new ApiException(
                    $"[{result.StatusCode}] Error sending post request to {uri} with following httpContent:{httpContent.ReadAsStringAsync().Result}",
                    await result.Content.ReadAsStringAsync().ConfigureAwait(false),
                    result.StatusCode
                );

            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            return string.IsNullOrEmpty(content) ? default : JsonConvert.DeserializeObject<T>(content);
        }

        /// <summary>
        ///     Generic method for posting data
        /// </summary>
        /// <param name="httpContent"></param>
        /// <param name="uri"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        protected static async Task DeleteAsync(HttpContent httpContent, string uri, string token)
        {
            var clientHandler = new HttpClientHandler();
            using var httpClient = new HttpClient(clientHandler);

            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{Settings.BaseUrl}{uri}"),
                Content = httpContent
            };
            var result = await httpClient.SendAsync(request);
            if (!result.IsSuccessStatusCode)
                throw new ApiException(
                    $"[{result.StatusCode}] Error sending post request to {uri}",
                    await result.Content.ReadAsStringAsync().ConfigureAwait(false),
                    result.StatusCode
                );
        }

        /// <summary>
        ///     Generic method for posting data
        /// </summary>
        /// <param name="httpContent"></param>
        /// <param name="uri"></param>
        /// <param name="token"></param>
        /// <typeparam name="T">Object type that will be given back after the post</typeparam>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        protected static async Task<T?> PutAsync<T>(HttpContent httpContent, string uri, string token)
        {
            var clientHandler = new HttpClientHandler();
            using var httpClient = new HttpClient(clientHandler);

            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

            var url = $"{Settings.BaseUrl}{uri}";
            var result = await httpClient.PutAsync(url, httpContent);

            if (!result.IsSuccessStatusCode)
                throw new ApiException(
                    $"[{result.StatusCode}] Error sending post request to {uri} with following httpContent:{httpContent.ReadAsStringAsync().Result}",
                    await result.Content.ReadAsStringAsync().ConfigureAwait(false),
                    result.StatusCode
                );

            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(content);
        }

        /// <summary>
        ///     Generic method for posting data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="uri"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        protected static async Task DeleteAsync(string id, string uri, string token)
        {
            var clientHandler = new HttpClientHandler();
            using var httpClient = new HttpClient(clientHandler);

            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

            var url = $"{Settings.BaseUrl}{uri}/{id}";

            var result = await httpClient.DeleteAsync(url, CancellationToken.None);
            if (!result.IsSuccessStatusCode)
                throw new ApiException(
                    $"[{result.StatusCode}] Error sending post request to {uri}",
                    await result.Content.ReadAsStringAsync().ConfigureAwait(false),
                    result.StatusCode
                );
        }
    }
}
