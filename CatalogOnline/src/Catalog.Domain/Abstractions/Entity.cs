﻿namespace Catalog.Domain.Abstractions
{
	public abstract class Entity
	{
		private readonly List<IDomainEvent> _domainEvents = new();

		public Guid Id { get; set; }

		protected Entity() { }

		protected Entity(Guid id)
		{
			Id = id;
		}

		public IReadOnlyList<IDomainEvent> DomainEvents => [.. _domainEvents];

		public IReadOnlyList<IDomainEvent> GetDomainEvents()
		{
			return _domainEvents;
		}

		public void ClearDomainEvents()
		{
			_domainEvents.Clear();
		}

		public void RaiseDomainEvent(IDomainEvent domainEvent)
		{
			_domainEvents.Add(domainEvent);
		}
	}
}
