using Microsoft.AspNetCore.Http.Json;
using System.ComponentModel;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new APIDemo.DateTimeConverter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

Dictionary<Guid, IEnumerable<DateTime>> requests = new();

var datesWithWeather = new List<DateTime>
{
    new DateTime(2022, 1, 1),
    new DateTime(2022, 1, 2),
    new DateTime(2022, 1, 3),
    new DateTime(2022, 1, 4),
    new DateTime(2022, 1, 5),
    new DateTime(2022, 1, 6),
    new DateTime(2022, 1, 7),
    new DateTime(2022, 1, 8),
    new DateTime(2022, 1, 9),
    new DateTime(2022, 1, 10),
};


app.MapPost("/api/datesservice/weatherdates", () =>
{
    var id = Guid.NewGuid();
    requests.Add(id, datesWithWeather);
    return new AsyncWeatherDatesEnvelope(id);
});


app.MapGet("/api/datesservice/weatherdates/{id}", (Guid id) => 
    requests[id]
);

app.MapPut("/api/detailsservice/weather/on/date", (DateTime forecastDate) =>
{
    if (!datesWithWeather.Contains(forecastDate))
        return new Envelope(
            error: true, 
            message: "Unable to commit transactional data due to an easy to resolve error - which we'll leave up to you to figure out");
    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(SunData));
    var writer = new StringWriter();
    serializer.Serialize(writer, new SunData { SunRise = "6:45", SunSet = "11:15" });

    return new Envelope(new WeatherForecast[] {new WeatherForecast(
        forecastDate,
        new DateTime(2001, 1, 1),
        "One hundred degrees, give or take a bit",
        "Weather was jolly hot",
         writer.ToString(),
         true)
        });
});

app.Run();

internal record WeatherForecast(DateTime date,
    DateTime dateOfPreviousHighTemperature,
    string temperature,
    string? summary,
    string sun_data,
    bool did_not_not_rain)
{

}

public class SunData
{
    public String? SunRise { get; set; }
    public String? SunSet { get; set; }
}

record AsyncWeatherDatesEnvelope(Guid identifier);

internal record Envelope(WeatherForecast[]? data = null, bool? error = null, string? message = null);