using Catalog.Domain.Abstractions;

namespace Catalog.Domain.Entities.Products.Events
{
	public sealed record ProductCreatedDomainEvent(Guid id) : IDomainEvent;
}
