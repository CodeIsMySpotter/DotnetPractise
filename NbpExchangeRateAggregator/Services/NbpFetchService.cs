using NbpExchangeRateAggregator.Dto;
using NbpExchangeRateAggregator.Exceptions;


namespace NbpExchangeRateAggregator.Services;

public interface INbpFetchService {
    Task<CurrencyData> FetchCurrencyDataFromNbp(string currencyCode, int days);
}

public class NbpFetchService(IHttpClientFactory httpClientFactory) : INbpFetchService
{
    public async Task<CurrencyData> FetchCurrencyDataFromNbp(string currencyCode, int days)
    {
        var client = httpClientFactory.CreateClient();
        var uri = $"http://api.nbp.pl/api/exchangerates/rates/a/{currencyCode}/last/{days}/?format=json";
        var response = await client.GetFromJsonAsync<CurrencyData>(uri);

        if (response is null) throw new ExternalServerFetchException();
        return response;
    }
}