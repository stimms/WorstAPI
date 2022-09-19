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


app.MapPost("/weatherdates", () =>
{
    return datesWithWeather;
});

app.MapPut("/weather/on/date", (DateTime forecastDate) =>
{
    return new Envelope(new WeatherForecast(
        forecastDate, 
        new DateTime(2001, 1, 1), 
        "One hundred degrees, give or take a bit", 
        "Weather was jolly hot"));
});

app.Run();

internal record WeatherForecast(DateTime date, DateTime dateOfPreviousHighTemperature, string temperature, string? summary)
{

}
 
internal record Envelope(WeatherForecast data);