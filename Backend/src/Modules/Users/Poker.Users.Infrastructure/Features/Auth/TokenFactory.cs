using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Poker.Common.Domain.Options;
using Poker.Common.Infrastructure.Clock;
using Poker.Common.Utilities;
using Poker.Users.Domain.Abstractions.Auth;
using Poker.Users.Domain.Abstractions.Auth.Models;

namespace Poker.Users.Infrastructure.Features.Auth;

public class TokenFactory : ITokenFactory
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly AuthOptions _jwtOptions;

    public TokenFactory(IOptionsMonitor<AuthOptions> jwtOptions, IDateTimeProvider dateTimeProvider)
    {
        _jwtOptions = jwtOptions.CurrentValue;
        _dateTimeProvider = dateTimeProvider;
    }

    public TokenResult CreateToken(string id)
    {
        var secretKeyBytes = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);
        var signingKey = new SymmetricSecurityKey(secretKeyBytes);
        var expiration = _dateTimeProvider.GetUtcNow(_jwtOptions.SecondsValid);

        var claimsIdentity = new ClaimsIdentity(
            [new Claim(AppClaims.Id, id)]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512Signature),
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            Expires = expiration
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);

        return new TokenResult(token);
    }
}