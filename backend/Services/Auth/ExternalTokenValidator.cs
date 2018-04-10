using backend.Interfaces.Auth;
using backend.Interfaces.Database;
using backend.Models.Config;
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
    public class ExternalTokenValidator : IExternalTokenValidator
    {
        private readonly TokenValidationParameters validationParameters;

        public ExternalTokenValidator(IOptions<ExternalTokenSettings> settings)
        {
            var options = settings.Value;
            this.validationParameters = new TokenValidationParameters()
            {
                ValidateAudience = options.ValidateAudience,
                ValidateIssuer = options.ValidateIssuer,
                ValidateLifetime = options.ValidateLifetime,
                ValidateIssuerSigningKey = options.ValidateIssuerSigningKey,
                ValidAudiences = options.ValidAudiences.Split(',').Select(iss => iss.Trim()).ToArray(),
                ValidIssuers = options.ValidIssuers.Split(',').Select(iss => iss.Trim()).ToArray(),
            };
        }

        public IValidationResult Validate(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = null;
            SecurityToken validToken = null;

            try
            {
                principal = handler.ValidateToken(token, this.validationParameters, out validToken);

                var validJwt = validToken as JwtSecurityToken;

                if (validJwt == null)
                {
                    throw new ArgumentException("Invalid JWT");
                }

                return new ValidationResult()
                {
                    Valid = true,
                };
            }
            catch (SecurityTokenValidationException)
            {
                return new ValidationResult()
                {
                    Valid = false,
                };
            }
            catch (ArgumentException)
            {
                return new ValidationResult()
                {
                    Valid = false,
                };
            }
        }

        private class ValidationResult : IValidationResult
        {
            public bool Valid { get; set; }
        }
    }
}
