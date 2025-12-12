using PerformanceService;
using Microsoft.Azure.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR().AddAzureSignalR("Endpoint=https://fantasysportsteam.service.signalr.net;" +
    "AccessKey=9rh7KDtpp0LWOcakcbSNyHkIPrsYMmdeOaiC50mElPqHPuw2Jz7gJQQJ99BLACYeBjFXJ3w3AAAAASRSdhNV;Version=1.0;");

var app = builder.Build();

app.MapHub<RealTimePerformance>("/performance");

app.MapFallbackToFile("index.html");

app.Run();
