using Polly;
using Polly.Fallback;
using ShoppingCart.Api;
using ShoppingCart.Api.Shared.Domain.Models;
using ShoppingCart.Api.Shared.Extensions;
using ShoppingCart.Api.Shared.Networking.Catalog.Api;
using ShoppingCart.Api.Shared.Slices;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddHttpClient<CatalogApiService>((serviceProvider, httpClient) =>
{
	httpClient.BaseAddress = new Uri("http://localhost:5167");
}).ConfigurePrimaryHttpMessageHandler(() =>
{
	return new SocketsHttpHandler
	{
		PooledConnectionLifetime = TimeSpan.FromMinutes(5),
	};
}).SetHandlerLifetime(Timeout.InfiniteTimeSpan);


builder.Services.AddResiliencePipeline<string, IEnumerable<Catalog>>("catalog-products", pipelineBuilder =>
{
	pipelineBuilder.AddFallback(new FallbackStrategyOptions<IEnumerable<Catalog>>
	{
		FallbackAction = _ => Outcome.FromResultAsValueTask<IEnumerable<Catalog>>(Enumerable.Empty<Catalog>())
	});
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddEndpoints();
builder.Services.AddApplication();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	//app.UseSwaggerUi(options =>
	//{
	//	options.Path = "/openapi";
	//	options.DocumentPath = "/openapi/v1.json";
	//});

	//app.UseReDoc(options =>
	//{
	//	options.Path = "/openapi";
	//	options.DocumentPath = "/openapi/v1/json";
	//});

	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/openapi/v1.json", "V1 Shopping Cart Documentation");
	});
}

//app.UseHttpsRedirection();



await app.ApplyMigration();

app.UseExceptionHandler();
app.MapEndpoints();

app.Run();

