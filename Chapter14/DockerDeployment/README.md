# MCP Weather Server - Docker Deployment

A sample MCP server demonstrating Docker containerization and deployment best practices.

## Build and Run

```bash
dotnet build
dotnet run
```

## Docker

```bash
docker build -t mcp-weather-server .
docker run -i mcp-weather-server
```

## Install as .NET Tool

```bash
dotnet pack -c Release
dotnet tool install --global --add-source ./nupkg McpWeatherServer
```
