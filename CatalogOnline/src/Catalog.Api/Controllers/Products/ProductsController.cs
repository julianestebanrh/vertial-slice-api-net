using Catalog.Application.Features.Products.CreateProduct;
using Catalog.Application.Features.Products.GetProducts;
using Catalog.Application.Features.Products.SearchProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers.Products
{
	[ApiController]
	[Route("api/products")]
	public class ProductsController : ControllerBase
	{
		private readonly ISender _sender;

		public ProductsController(ISender sender)
		{
			_sender = sender;
		}

		[HttpGet("code/{value}")]
		public async Task<IActionResult> GetByCode(string value)
		{
			var context = HttpContext;
			var query = new SearchProductQuery { Code = value, Context = context };
			var response = await _sender.Send(query);
			return Ok(response);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllProducts()
		{
			var context = HttpContext;
			var query = new GetProductsQuery { Context = context };
			var response = await _sender.Send(query);
			return Ok(response);
		}

		[HttpPost]
		public async Task<IActionResult> CreateProduct([FromBody] ProductRequest request)
		{
			var query = new CreateProductCommand(request.Name, request.Description, request.Price, request.CategoryId);
			var response = await _sender.Send(query);
			return Ok(response);
		}
	}
}
