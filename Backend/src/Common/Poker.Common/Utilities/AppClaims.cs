namespace Poker.Common.Utilities;

public abstract class AppClaims
{
    public const string Id = nameof(Id);
    public static readonly List<string> All = new() { Id };
}