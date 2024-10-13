using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Api.Shared.Persistence;
using ShoppingCart.Api.Shared.Slices;

namespace ShoppingCart.Api.Features.ShoppingCart
{
	public class DeleteItem : IEndpoint
	{
		public void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
		{
			endpointRouteBuilder.MapDelete("api/items/{id)", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
			{
				var command = new DeleteItemCommand(id);
				return await mediator.Send(command, cancellationToken);
			});
		}

		public sealed class DeleteItemCommand(Guid id) : IRequest<IResult>
		{
			public Guid Id { get; set; } = id;
		}

		public sealed class DeleteItemCommandValidator : AbstractValidator<DeleteItemCommand>
		{
			public DeleteItemCommandValidator()
			{
				RuleFor(x => x.Id).NotEmpty().NotNull();
			}
		}

		public sealed class DeleteItemCommandHandler(ShoppingDbContext context) : IRequestHandler<DeleteItemCommand, IResult>
		{
			private readonly ShoppingDbContext _context = context;

			public async Task<IResult> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
			{
				var rowsAffected = await _context
					.Items
					.Where(x => x.Id == request.Id)
					.ExecuteDeleteAsync(cancellationToken);

				return rowsAffected > 0 ? Results.NoContent() : Results.NotFound();
			}
		}
	}
}
