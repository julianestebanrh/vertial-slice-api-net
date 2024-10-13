using Catalog.Domain.Abstractions;
using Catalog.Domain.Entities.Products;
using MediatR;

namespace Catalog.Application.Features.Products.CreateProduct
{
	internal sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IProductRepository _productRepository;

		public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_productRepository = productRepository;
		}

		public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
		{
			var product = Product.Create(request.Name, request.Price, request.Description, null!, null!, request.CategoryId);
			_productRepository.Add(product);
			await _unitOfWork.SaveChangesAsync(cancellationToken);
			return product.Id;

		}
	}
}
