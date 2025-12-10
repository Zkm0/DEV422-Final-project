using PerformanceService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR().AddAzureSignalR("Server=tcp:nzc-fantasyteam-sql.database.windows.net,1433;" +
    "Initial Catalog=PerformanceDB;Persist Security Info=False;User ID=nathanzoecolin;Password=Test1234;" +
    "MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

var app = builder.Build();

app.MapHub<RealTimePerformance>("/performance");

app.MapFallbackToFile("performance.html");

app.MapGet("/", () => "Hello World!");

app.Run();
