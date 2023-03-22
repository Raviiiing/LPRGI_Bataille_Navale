using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Jeu
{
    internal class Api
    {
        private String url;
        private String key;

        public Api(String url, String key)
        {
            this.url = url;
            this.key = key;
        }
        
        public async Task<string> GetApiContent()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("x-functions-key", key);
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception($"Failed to retrieve HTML content from URL: {url}, status code: {response.StatusCode}");
            }
        }

        public dynamic GetDynamicFromJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<dynamic>(json);
            }
            catch (JsonException ex)
            {
                throw new Exception("Failed to deserialize JSON string to dynamic object.", ex);
            }
        }
    }
}
