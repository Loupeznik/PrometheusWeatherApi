

using DZarsky.PrometheusWeatherApi.Api.Models;

namespace DZarsky.PrometheusWeatherApi.Api;

public static class Endpoints
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private static readonly int[] StatusCodes = { 200, 400, 429 };

    public static void MapWeatherApi(this WebApplication app)
    {
        app.MapGroup("/api").MapWeatherEndpoints();
    }

    private static void MapWeatherEndpoints(this IEndpointRouteBuilder group)
    {
        group.MapGet("/weather/simple", () =>
             {
                 var forecast = Enumerable.Range(1, 5).Select(index =>
                                              new WeatherForecast
                                              (
                                                  DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                                                  Random.Shared.Next(-20, 55),
                                                  Summaries[Random.Shared.Next(Summaries.Length)]
                                              ))
                                          .ToArray();
                 return forecast;
             })
             .WithName("GetWeatherForecast")
             .WithOpenApi();

        group.MapGet("/weather/delayed", () =>
             {
                 var forecast = Enumerable.Range(1, 5).Select(index =>
                                              new WeatherForecast
                                              (
                                                  DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                                                  Random.Shared.Next(-20, 55),
                                                  Summaries[Random.Shared.Next(Summaries.Length)]
                                              ))
                                          .ToArray();

                 Thread.Sleep(5000);

                 return forecast;
             })
             .WithName("GetDelayedWeatherForecast")
             .WithOpenApi();

        group.MapGet("/weather/dynamic", () =>
             {
                 var forecast = Enumerable.Range(1, 5).Select(index =>
                                              new WeatherForecast
                                              (
                                                  DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                                                  Random.Shared.Next(-20, 55),
                                                  Summaries[Random.Shared.Next(Summaries.Length)]
                                              ))
                                          .ToArray();

                 Thread.Sleep(new Random().Next(1000, 8000));

                 var statusCode = StatusCodes[new Random().Next(StatusCodes.Length)];

                 return statusCode != 200 ? Results.StatusCode(statusCode) : Results.Json(forecast);
             })
             .WithName("GetDynamicWeatherForecast")
             .WithOpenApi();
    }
}
