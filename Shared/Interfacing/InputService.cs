

using System.Runtime.Caching;

namespace Shared.Interfacing;

public interface IInputService
{
    Task<string> GetInputOfDayAsync(int year, int day);
}

public class InputService : IInputService
{
    private readonly MemoryCache _memoryCache;
    private readonly IAoCClient _client;
    
    private readonly string _path;

    public InputService(IAoCClient client)
    {
        _client = client;
        
        _path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"../../../../Inputs/"));
        if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);

        _memoryCache = new MemoryCache("AOC");
    }
    
    public async Task<string> GetInputOfDayAsync(int year, int day)
    {
        if (!_memoryCache.Contains($"{year}_{day}"))
            await FetchDay(year, day);
        return (string)_memoryCache.Get($"{year}_{day}");
    }

    private async Task FetchDay(int year, int day)
    {
        var file = Path.Combine(_path, $"Year_{year}_Day_{day}.txt");
        if (!File.Exists(file))
        {
            var content = await _client.GetInputForDayAsync(year, day);
            await File.WriteAllTextAsync(file, content);
        }
        _memoryCache.Add($"{year}_{day}", await File.ReadAllTextAsync(file), new CacheItemPolicy());
    }
}