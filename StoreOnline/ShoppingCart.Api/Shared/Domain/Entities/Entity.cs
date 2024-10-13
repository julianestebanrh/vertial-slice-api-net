﻿namespace ShoppingCart.Api.Shared.Domain.Entities
{
	public abstract class Entity(Guid id)
	{
		protected Entity() : this(default) { }
		public Guid Id { get; set; } = id;
		public DateTime? CreatedOn { get; set; }
		public string CreatedBy { get; set; } = string.Empty;
		public DateTime? LastModifiedOn { get; set; }
		public string? LastModifiedBy { get; set; }
	}
}
