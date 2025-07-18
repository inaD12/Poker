namespace Poker.Common.Domain.Exceptions;

public class PokerValidationException : Exception
{
	public string PropertyName { get; }
	public string ErrorMessage { get; }

	public PokerValidationException(string propertyName, string errorMessage)
		: base($"Validation failed for '{propertyName}': {errorMessage}")
	{
		PropertyName = propertyName;
		ErrorMessage = errorMessage;
	}
}
