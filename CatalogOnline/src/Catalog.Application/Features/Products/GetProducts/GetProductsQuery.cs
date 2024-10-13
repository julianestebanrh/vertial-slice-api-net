using Catalog.Application.Features.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Features.Products.GetProducts
{
	public sealed class GetProductsQuery() : IRequest<List<ProductDto>>
	{
		public HttpContext? Context { get; set; }
	}
}
