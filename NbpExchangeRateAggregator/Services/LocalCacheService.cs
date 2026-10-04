using System.Net.Http.Headers;
using NbpExchangeRateAggregator.Dto;
using NbpExchangeRateAggregator.Dto;


namespace NbpExchangeRateAggregator.Services;


public abstract class ALocalCacheService<T>
{
    public abstract Task<T?> GetFromCache(string currencyCode);
    public abstract Task UpdateCache(string currencyCode, T data);
}


public class CurrencyInMemoryCache : ALocalCacheService<CurrencyData>
{

    private Dictionary<string, CurrencyData> cache = new();
    public override Task<CurrencyData?> GetFromCache(string currencyCode)
    {
        if (cache.TryGetValue(currencyCode, out var currencyData))
        {
            return Task.FromResult<CurrencyData?>(currencyData);
        }

        return Task.FromResult<CurrencyData?>(null);
    }


    public override Task UpdateCache(string currencyCode, CurrencyData data)
    {
        if (cache.ContainsKey(currencyCode))
        {
            if(cache[currencyCode].Rates.Count < data.Rates.Count)
            {
                cache[currencyCode] = data;
            }
        }
        else
        {
            cache.Add(currencyCode, data);
        }
        return Task.CompletedTask;
    }
}



public class SummaryFileCache : ALocalCacheService<(decimal, decimal, decimal)>
{

    private static SemaphoreSlim semaphore = new(1);

    public override async Task<(decimal, decimal, decimal)?> GetFromCache(string currencyCode)
    {
        var filePath = System.IO.Path.Combine(Environment.CurrentDirectory, "Reports", $"CacheFile_{currencyCode}.txt");
        if (!System.IO.File.Exists(filePath)) return null;

        await semaphore.WaitAsync(); 
        try
        {
            using var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            using var streamReader = new System.IO.StreamReader(fileStream);

            string? line = await streamReader.ReadLineAsync();
            
            if (line != null)
            {
                var parts = line.Split(',');
                if (parts.Length == 5)
                {
                    return (
                        decimal.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture),
                        decimal.Parse(parts[3], System.Globalization.CultureInfo.InvariantCulture),
                        decimal.Parse(parts[4], System.Globalization.CultureInfo.InvariantCulture)
                    );
                }
            }

            return null;
        }
        finally
        {
            semaphore.Release();
        }
    }


    public override Task UpdateCache(string currencyCode, (decimal, decimal, decimal) data)
    {
        return Task.CompletedTask;
    }
}