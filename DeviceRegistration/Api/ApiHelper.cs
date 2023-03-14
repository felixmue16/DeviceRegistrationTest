
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Http;

namespace DeviceRegistration.Api
{
    public class ApiHelper : IApiHelper
    {
        readonly HttpClient _client = new HttpClient();
        private readonly ILogger<ApiHelper> _logger;

        public ApiHelper(ILogger<ApiHelper> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetResponseAsync(string url, StringContent content, string sessionId)
        {
            try
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _client.DefaultRequestHeaders.Add("auth-sessionid", sessionId);
                using HttpResponseMessage response = await _client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(responseBody);
                // removes double quotes from the string.
                return responseBody.Trim('"');
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return e.Message;
            }
            //catch (HttpRequestException e)
            //{
            //    Console.WriteLine("\nException Caught!");
            //    Console.WriteLine("Message :{0} ", e.Message);
            //    return e.Message;
            //}


        }
    }
}
