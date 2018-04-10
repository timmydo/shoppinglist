using backend.Interfaces.Auth;
using backend.Interfaces.Database;
using backend.Models.Config;
using backend.Models.Responses;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Services.Auth
{
    public class TokenBuilder : ITokenBuilder
    {
        private readonly JwtSettings options;
        private readonly SymmetricSecurityKey key;

        public TokenBuilder(IOptions<JwtSettings> options, ISecretStore secretStore)
        {
            this.options = options.Value;
            var keys = this.options.SymmetricSigningKeys.Split(',');
            if (!keys.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(this.options.SymmetricSigningKeys));
            }

            this.key = new SymmetricSecurityKey(Convert.FromBase64String(secretStore.Get(keys[0].Trim())));
        }

        public TokenResponse Build(string username)
        {
            var now = DateTimeOffset.UtcNow;

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var expiration = TimeSpan.FromMinutes(options.ExpirationMinutes);
            var alg = SecurityAlgorithms.HmacSha256;
            var signingCreds = new SigningCredentials(key, alg);
            var jwt = new JwtSecurityToken(
                issuer: options.CreateWithIssuer,
                audience: options.Audience,
                claims: claims,
                notBefore: now.UtcDateTime,
                expires: now.Add(expiration).UtcDateTime,
                signingCredentials: signingCreds);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new TokenResponse()
            {
                Token = encodedJwt,
                ExpiresIn = (int)expiration.TotalSeconds
            };
        }
    }
}
