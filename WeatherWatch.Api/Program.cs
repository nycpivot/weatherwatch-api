//using Prometheus;
//using Tap.Dotnet.Weather.Api;
//using Tap.Dotnet.Weather.Api.Interfaces;
using WeatherBit.Domain;
using WeatherBit.Domain.Interfaces;
using WeatherWatch.Api;
using WeatherWatch.Api.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//var serviceBindings = Environment.GetEnvironmentVariable("SERVICE_BINDING_ROOT") ?? String.Empty;

//var weatherDbApi = Environment.GetEnvironmentVariable("WEATHER_DB_API") ?? String.Empty;

var weatherBitUrl = Environment.GetEnvironmentVariable("weather-bit-api-url");
var weatherBitKey = Environment.GetEnvironmentVariable("weather-bit-api-key");
//var wavefrontUrl = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "wavefront-api-resource-claim", "host"));
//var wavefrontToken = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "wavefront-api-resource-claim", "token"));

// setup weather bit service
var weatherBitService = new WeatherBitService()
{
    Url = weatherBitUrl,
    Key = weatherBitKey
};

builder.Services.AddSingleton<IWeatherBitService>(weatherBitService);

// setup wavefront
//var wfSender = new WavefrontDirectIngestionClient.Builder(wavefrontUrl, wavefrontToken).Build();
//builder.Services.AddSingleton<IWavefrontSender>(wfSender);

// setup weather data service
var weatherDataService = new WeatherDataService(); // { Url = weatherDbApi };
builder.Services.AddSingleton<IWeatherDataService>(weatherDataService);


builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();
//app.UseHttpMetrics(); // prometheus

app.MapControllers();
//app.MapMetrics(); // prometheus

app.Run();
