using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PanoptoRest.Auth
{
    public class Token
    {
        public string AuthKey;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _username;
        private readonly string _password;
        private readonly string _baseUrl;

        public Token(string clientId, string clientSecret, string username, string password, string baseUrl)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _username = username;
            _password = password;
            _baseUrl = baseUrl;
        }

        public TokenResult GetAdminToken()
        {
            // Code from Paul Redmond https://community.panopto.com/discussion/1208/rest-api-authentication-issue

            // See 1.2 https://support.panopto.com/s/article/oauth2-for-services
            // Combine the API Key and secret value with a colon between the two <ClientAPIKey>:<ClientSecretValue> 
            AuthKey = _clientId + ":" + _clientSecret;
            //Then, Base64 encode the result. Set the Authorization header to: Basic<EncodedClientCredentials>
            AuthKey = Convert.ToBase64String(Encoding.ASCII.GetBytes(AuthKey));
            // Create a Http Client, add the base URL to the token URL, add the AuthKey as Basic Auth
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            HttpRequestMessage tokenRequest = new HttpRequestMessage(HttpMethod.Post, "/Panopto/oauth2/connect/token");
            tokenRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            tokenRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", AuthKey);

            // Create a form object with the username and password. See 2.1.d https://support.panopto.com/s/article/oauth2-for-services
            // Note: It is strongly recommended to use a Hybrid or Server-side Web Application client instead in addition to the required single user login.
            HttpContent httpContent = new FormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", _username),
                    new KeyValuePair<string, string>("password", _password),
                    new KeyValuePair<string, string>("scope", "api")
                    
                });
            tokenRequest.Content = httpContent;
            tokenRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            // Send the request to Panopto. The task is asynchronous, but the await doesn't allow the function to continue
            var response = Task.Run(async () => await client.SendAsync(tokenRequest));
            Task<string> values = response.Result.Content.ReadAsStringAsync();

            // Json is returned, either as an Access token with Expires and Type, or with Error and an Error Description
            // JsonConvert will create an object that can be more easily passed around
            var tokenResult = JsonConvert.DeserializeObject<TokenResult>(values.Result);

            // If we have a valid token that has not errored, keep a friendlier note of when it expires
            if (tokenResult != null && string.IsNullOrEmpty(tokenResult.Error) && !tokenResult.ExpiresIn.Equals(0))
            {
                tokenResult.Created = DateTime.Now;
                tokenResult.Expires = DateTime.Now.AddSeconds(tokenResult.ExpiresIn);
            }

            return tokenResult;
        }
    }
}
