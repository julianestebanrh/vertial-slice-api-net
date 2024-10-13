using FluentValidation;

namespace Catalog.Application.Features.Products.CreateProduct
{
	public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
	{
		public CreateProductCommandValidator()
		{

			RuleFor(x => x.Name).NotEmpty().NotNull().MinimumLength(5);
			RuleFor(x => x.Description).NotEmpty().NotNull().MinimumLength(15);
		}
	}
}
