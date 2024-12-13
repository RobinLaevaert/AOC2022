using System.Net;

namespace Shared.Interfacing;

public interface IAoCClient
{
    Task<string> GetInputForDayAsync(int year, int day);
    Task<string> PostAnswerForDayAsync(int year, int day, int part, string answer);
}

public class AoCClient : IAoCClient
{
    private readonly Uri _baseUri;
    private CookieContainer _container;

    public AoCClient(string baseUrl, string sessionCookie)
    {
        _baseUri = new Uri(baseUrl);
        _container = new CookieContainer();
        _container.Add(_baseUri, new Cookie("session", sessionCookie));
    }
    
    public async Task<string> GetInputForDayAsync(int year, int day)
    {
        using var client = new HttpClient(new HttpClientHandler() {CookieContainer = _container })
        {
            BaseAddress = _baseUri 
        };
        return await client.GetStringAsync($"{year}/day/{day}/input");
    }

    public async Task<string> PostAnswerForDayAsync(int year,int day, int part, string answer)
    {
        using var client = new HttpClient(new HttpClientHandler() {CookieContainer = _container })
        {
            BaseAddress = _baseUri 
        };
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        var values = new Dictionary<string, string>
        {
            { "level", $"{part}"},
            { "answer", $"{answer}" }   
        };
        var response = await client.PostAsync($"{year}/day/{day}/answer", new FormUrlEncodedContent(values));
        return await response.Content.ReadAsStringAsync();
    }
}