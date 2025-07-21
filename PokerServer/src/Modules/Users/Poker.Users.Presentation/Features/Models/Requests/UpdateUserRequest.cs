using System.ComponentModel.DataAnnotations;

namespace Poker.Users.Presentation.Features.Models.Requests;

public class UpdateUserRequest
{
	[Required]
	public string NewUsername { get; set; }
}
