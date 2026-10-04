using Microsoft.AspNetCore.Mvc;
using NbpExchangeRateAggregator.Dto;
using NbpExchangeRateAggregator.Services;
using System.Net.Http;

namespace NbpExchangeRateAggregator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExchangeDataFetchController(
    IApplicationService applicationService
) : ControllerBase
{

    [HttpGet("get/currencydata")]
    public async Task<IActionResult> GetCurrencyData(string currencyCode, int days) {
        
        var fetchedData = await applicationService.GetFullCurrencyData(currencyCode, days);
        return Ok(fetchedData);
    }
}
