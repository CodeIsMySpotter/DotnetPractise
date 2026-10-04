


using NbpExchangeRateAggregator.Dto;

namespace NbpExchangeRateAggregator.Services;

public interface IApplicationService
{
    Task<CurrencyData> GetFullCurrencyData(string code, int days);
    Task<(decimal, decimal, decimal)> GetSummarizedCurrencyData(string code, int days);
}


public class ApplicationService(
    ALocalCacheService<CurrencyData> memoryCacheService,
    INbpFetchService nbpFetchService
) : IApplicationService
{
    public async Task<CurrencyData> GetFullCurrencyData(string code, int days)
    {
        var data = await memoryCacheService.GetFromCache(code);
        
        if (data is not null && data.Rates.Count >= days)
        {
            return data with { Rates = data.Rates.GetRange(0, days) };
        }

        var fetchedData = await nbpFetchService.FetchCurrencyDataFromNbp(code, days);
        await memoryCacheService.UpdateCache(code, fetchedData);

        return fetchedData;
    }


    public async Task<(decimal, decimal, decimal)> GetSummarizedCurrencyData(string code, int days)
    {
        var fetchedData = await GetFullCurrencyData(code, days);
        
        var min = fetchedData.Rates.Min(r => r.Mid);
        var max = fetchedData.Rates.Max(r => r.Mid);
        var avg = fetchedData.Rates.Average(r => r.Mid);

        return (min, max, avg);
    }
}