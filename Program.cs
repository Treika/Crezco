using Abstraction;
using Application;
using Data;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.ConfigureAbstraction();
builder.Host.ConfigureApplication();
builder.Host.ConfigureDatabase();

builder.Services.AddMemoryCache(options =>
{
    options = new MemoryCacheOptions
    {
        ExpirationScanFrequency = TimeSpan.FromSeconds(30)
    };
});

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
