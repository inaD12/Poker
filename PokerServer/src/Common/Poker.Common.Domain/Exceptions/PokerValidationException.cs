using FluentValidation.Results;

namespace Poker.Common.Domain.Exceptions;

public class PokerValidationException : Exception
{
    public PokerValidationException(IEnumerable<ValidationFailure> failures)
        : base("Validation failed.")
    {
        Errors = failures
            .Select(f => new ValidationError(f.PropertyName, f.ErrorMessage))
            .ToArray();
    }

    public IReadOnlyCollection<ValidationError> Errors { get; }

    public class ValidationError
    {
        public ValidationError(string property, string message)
        {
            Property = property;
            Message = message;
        }

        public string Property { get; }
        public string Message { get; }
    }
}