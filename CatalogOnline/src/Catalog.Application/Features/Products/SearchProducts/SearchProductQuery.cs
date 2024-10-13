using Catalog.Application.Features.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Features.Products.SearchProducts
{
	public sealed class SearchProductQuery : IRequest<ProductDto>
	{
		public HttpContext? Context { get; set; }
		public string? Code { get; set; }
	}
}
