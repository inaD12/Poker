using System.ComponentModel.DataAnnotations;

namespace PokerServer.Options;

public class CorsOptions
{
	[Required]
	public string AllowedOrigins { get; set; } = string.Empty;
}
