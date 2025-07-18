using System.ComponentModel.DataAnnotations;

namespace Poker.Common.Domain.Options;

public sealed class AuthOptions
{
	[Required]
	public string SecretKey { get; set; }
	[Required]
	public string Audience { get; set; }
	[Required]
	public string Issuer { get; set; }
	[Required]
	public int SecondsValid { get; set; }
}
