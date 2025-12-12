using Microsoft.EntityFrameworkCore;
using PlayerManagementService.Models;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PlayerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PlayerDB")));


var teamServiceUrl = builder.Configuration["ServiceUrls:TeamService"]
    ?? throw new InvalidOperationException("TeamService URL is missing");

builder.Services.AddHttpClient("TeamService", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:TeamService"]);
});

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
