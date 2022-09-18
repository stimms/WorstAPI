var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    return new WeatherForecast(forecastDate, 100, "Weather was jolly hot");
});

app.Run();

internal record WeatherForecast(DateTime date, int temperature, string? summary)
{
    
}