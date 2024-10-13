using Catalog.Domain.Abstractions;

namespace Catalog.Domain.Entities.Categories.Events
{
	public sealed record CategoryCreatedDomainEvent(Guid Id) : IDomainEvent;
}
