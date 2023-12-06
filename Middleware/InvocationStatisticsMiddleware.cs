using System.Diagnostics;
using Prometheus;

namespace DZarsky.PrometheusWeatherApi.Api.Middleware;

public class InvocationStatisticsMiddleware(RequestDelegate next)
{
    private readonly Counter _httpRequestCounter = Metrics.CreateCounter("http_requests_total", "Total number of HTTP requests",
        new CounterConfiguration
        {
            LabelNames = new[] { "endpoint", "status_code" }
        });
    private readonly Histogram _httpRequestDurationHistogram = Metrics.CreateHistogram("http_request_duration_seconds",
        "Histogram of HTTP request durations", new HistogramConfiguration
        {
            LabelNames = new[] { "endpoint" }
        });

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.Request.Path;

        var stopwatch = Stopwatch.StartNew();
        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();

            _httpRequestCounter.WithLabels(endpoint, context.Response.StatusCode.ToString()).Inc();
            _httpRequestDurationHistogram.WithLabels(endpoint).Observe(stopwatch.Elapsed.TotalSeconds);
        }
    }
}
