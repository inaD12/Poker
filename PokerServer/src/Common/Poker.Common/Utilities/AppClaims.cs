namespace Poker.Common.Utilities;

public abstract class AppClaims
{
	public static readonly List<string> All = new() { Id};

	public const string Id = nameof(Id);
}
