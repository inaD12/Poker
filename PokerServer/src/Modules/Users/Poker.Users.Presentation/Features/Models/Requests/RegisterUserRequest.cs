using System.ComponentModel.DataAnnotations;

namespace Poker.Users.Presentation.Features.Models.Requests;

public class RegisterUserRequest
{
	[Required]
	public string Email { get; set; }
	[Required]
	public string Password { get; set; }
	[Required]
	public string Username { get; set; }
}
