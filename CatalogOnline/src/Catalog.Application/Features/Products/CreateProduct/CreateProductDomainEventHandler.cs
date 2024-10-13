using Catalog.Domain.Entities.Products.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.Products.CreateProduct
{
	internal sealed class CreateProductDomainEventHandler : INotificationHandler<ProductCreatedDomainEvent>
	{
		private readonly ILogger _logger;

		public CreateProductDomainEventHandler(ILogger logger)
		{
			_logger = logger;
		}

		public Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"A new product has been created with the identifier: {notification.id}");

			return Task.FromResult(1);
		}
	}
}
