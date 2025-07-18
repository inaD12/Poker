using System.ComponentModel.DataAnnotations;

namespace Poker.Common.Domain.Options;

public sealed class DatabaseOptions
{
	[Required]
	public string ConnectionString { get; set; } = string.Empty;
}
