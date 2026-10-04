using System.Text.Json.Serialization;

namespace NbpExchangeRateAggregator.Dto;

public record CurrencyData(
    [property: JsonPropertyName("table")] string Table,
    [property: JsonPropertyName("currency")] string Currency,
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("rates")] List<RateData> Rates
);

public record RateData(
    [property: JsonPropertyName("no")] string No,
    [property: JsonPropertyName("effectiveDate")] string EffectiveDate,
    [property: JsonPropertyName("mid")] decimal Mid
);