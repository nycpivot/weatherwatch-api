using Microsoft.OpenApi.Models;
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

var weatherBitUrl = Environment.GetEnvironmentVariable("WEATHER_BIT_API_URL") ?? String.Empty;
var weatherBitKey = Environment.GetEnvironmentVariable("WEATHER_BIT_API_KEY") ?? String.Empty;
//var wavefrontUrl = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "wavefront-api-resource-claim", "host"));
//var wavefrontToken = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "wavefront-api-resource-claim", "token"));

var weatherDataApi = Environment.GetEnvironmentVariable("WEATHER_DATA_API_URL") ?? String.Empty;

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
var weatherDataService = new WeatherDataService() { Url = weatherDataApi }; // { Url = weatherDbApi };
builder.Services.AddSingleton<IWeatherDataService>(weatherDataService);

// add dapr client
builder.Services.AddDaprClient(builder => builder
    .UseHttpEndpoint("http://localhost:3500")
    .UseGrpcEndpoint("http://localhost:50001"));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyProject", Version = "v1.0.0" });

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "Using the Authorization header with the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securitySchema);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { "Bearer" } }
    });
});


builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();
//app.UseHttpMetrics(); // prometheus

app.MapControllers();
//app.MapMetrics(); // prometheus

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.Run();
