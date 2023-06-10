using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;

namespace API;

public class Api
{
    private readonly string key;
    private readonly string url;
 

    public Api(string url, string key)
    {
        this.url = url;
        this.key = key;
    }

    public async Task<string> GetApiContent()
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("x-functions-key", key);
        var response = await httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadAsStringAsync();

        throw new Exception($"Failed to retrieve HTML content from URL: {url}, status code: {response.StatusCode}");
    }
}