using Catalog.Application.Features.Products.Dtos;
using Catalog.Domain.Entities.Products;
using MediatR;

namespace Catalog.Application.Features.Products.GetProducts
{
	internal sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
	{
		private readonly IProductRepository _productRepository;

		public GetProductsQueryHandler(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
		{
			var products = await _productRepository.GetAllAsync(cancellationToken);
			var productsDto = products.ConvertAll(x => x.ToDto(request.Context!));
			return productsDto;
		}
	}
}
