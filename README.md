# .NET 8 Weather API with Prometheus

This is a simple .NET 8 Weather API exposing Promtheus metrics. Uses Minimal APIs.

## Running

Either run from console using `docker-compose` or run from the IDE.

```powershell
docker-compose up
```

By default, the `docker-compose` project will run the API on port `8080`, Prometheus on port `9090` and Grafana on port `8081`.
