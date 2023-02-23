using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Chat.Net
{
    public class AuthenticateUser
    {
        private readonly string _apiUrl;

        public AuthenticateUser(string apiUrl)
        {
            _apiUrl = apiUrl;
        }

        public async Task<string> AuthenticateAsync(string username, string passwordHash)
        {
            var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", passwordHash)
            });

            var response = await client.PostAsync(_apiUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(responseString);

            var responseValue = json.RootElement.GetProperty("Response").GetString();
            var infoValue = json.RootElement.GetProperty("Info").GetString();

            return $"{responseValue}:{infoValue}";
        }
    }
}
