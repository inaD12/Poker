using System.ComponentModel.DataAnnotations;

namespace Poker.Common.Presentation.Options;

public class CorsOptions
{
	[Required]
	public string AllowedOrigins { get; set; } = string.Empty;
}
