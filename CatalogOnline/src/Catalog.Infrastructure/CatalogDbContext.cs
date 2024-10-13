
using Catalog.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure
{
	public class CatalogDbContext : DbContext, IUnitOfWork
	{
		private readonly IPublisher _publisher;


		public CatalogDbContext(DbContextOptions options, IPublisher publisher) : base(options)
		{
			_publisher = publisher;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);

			base.OnModelCreating(modelBuilder);
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			var result = await base.SaveChangesAsync(cancellationToken);
			await PublishNotifications();
			return result;
		}

		private async Task PublishNotifications()
		{
			var domainEventNotifications = ChangeTracker.Entries<Entity>()
				.Select(entry => entry.Entity)
				.SelectMany(entry =>
				{
					var eventNotifications = entry.GetDomainEvents();
					entry.ClearDomainEvents();
					return eventNotifications;
				}).ToList();

			foreach (var domainEventNotification in domainEventNotifications)
			{
				await _publisher.Publish(domainEventNotification);
			}
		}
	}
}
