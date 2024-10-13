using Catalog.Application.Features.Products.Dtos;
using Catalog.Domain.Entities.Products;
using MediatR;

namespace Catalog.Application.Features.Products.SearchProducts
{
	internal sealed class SearchProductQueryHandler : IRequestHandler<SearchProductQuery, ProductDto>
	{
		private readonly IProductRepository _productRepository;
		public SearchProductQueryHandler(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<ProductDto> Handle(SearchProductQuery request, CancellationToken cancellationToken)
		{
			var product = await _productRepository.GetByCodeAsync(request.Code!, cancellationToken);
			return product!.ToDto(request.Context!);
		}
	}
}
