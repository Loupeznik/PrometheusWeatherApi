using DZarsky.PrometheusWeatherApi.Api;
using DZarsky.PrometheusWeatherApi.Api.Middleware;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<InvocationStatisticsMiddleware>();

Metrics.SuppressDefaultMetrics();

app.MapWeatherApi();
app.MapMetrics();

app.Run();
