
using Catalog.Api.Extensions;
using Catalog.Application;
using Catalog.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
	config.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "Product Catalog",
		Version = "v1",
	});

});

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.ApplyMigration();
await app.SeedCatalogProduct();

app.MapControllers();
//app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();

