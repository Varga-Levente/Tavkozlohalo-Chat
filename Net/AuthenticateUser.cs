using System.Collections.Generic;
using System.Net.Http;
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

        // Authenticate the user with the API POST request
        // The API returns a string with the response and info
        // The response is either "Success" or "Error"
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

            // *UPDATE RESPONSES!*
            // *New responses: (Resp[Err,Succ] | MSG[*])*
            var responseValue = json.RootElement.GetProperty("Response").GetString();
            var infoValue = json.RootElement.GetProperty("Info").GetString();

            return $"{responseValue}:{infoValue}";
        }
    }
}
