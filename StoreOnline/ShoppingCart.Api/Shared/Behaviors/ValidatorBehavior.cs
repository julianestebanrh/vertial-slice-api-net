using FluentValidation;
using MediatR;

namespace ShoppingCart.Api.Shared.Behaviors
{
	public sealed class ValidatorBehavior<TRequest, IResult>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, IResult> where TRequest : IRequest<IResult>
	{
		private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

		public async Task<IResult> Handle(TRequest request, RequestHandlerDelegate<IResult> next, CancellationToken cancellationToken)
		{
			if (!_validators.Any())
			{
				return await next();
			}

			var context = new ValidationContext<TRequest>(request);
			var validationResults = _validators.Select(x => x.Validate(context)).ToList();

			var groupValidations = validationResults
					.SelectMany(x => x.Errors)
					.GroupBy(x => x.PropertyName)
					.Select(x => new
					{
						PropertyName = x.Key,
						ValidationFailures = x.Select(v => new { v.ErrorMessage })
					}).ToList();

			if (groupValidations.Count != 0)
			{
				var validationDictionary = new Dictionary<string, string[]>();
				foreach (var groupValidation in groupValidations)
				{
					var errorMessages = groupValidation.ValidationFailures.Select(x => x.ErrorMessage);
					validationDictionary.Add(groupValidation.PropertyName, errorMessages.ToArray());
				}
				return (IResult)Results.ValidationProblem(validationDictionary);
			}

			return await next();

		}
	}
}
