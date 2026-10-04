using NbpExchangeRateAggregator.Dto;
using NbpExchangeRateAggregator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<ALocalCacheService<CurrencyData>, CurrencyInMemoryCache>();
builder.Services.AddScoped<INbpFetchService, NbpFetchService>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
