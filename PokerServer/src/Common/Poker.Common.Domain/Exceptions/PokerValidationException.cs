using FluentValidation.Results;

namespace Poker.Common.Domain.Exceptions;

public class PokerValidationException : Exception
{
	public IReadOnlyCollection<ValidationError> Errors { get; }

	public PokerValidationException(IEnumerable<ValidationFailure> failures)
		: base("Validation failed.")
	{
		Errors = failures
			.Select(f => new ValidationError(f.PropertyName, f.ErrorMessage))
			.ToArray();
	}

	public class ValidationError
	{
		public string Property { get; }
		public string Message { get; }

		public ValidationError(string property, string message)
		{
			Property = property;
			Message = message;
		}
	}
}
