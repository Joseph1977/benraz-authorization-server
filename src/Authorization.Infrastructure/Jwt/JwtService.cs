using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Authorization.Infrastructure.Jwt
{
    /// <summary>
    /// JWT service.
    /// </summary>
    public class JwtService : IJwtService
    {
        private const string AUDIENCE_CLAIM_TYPE = "aud";

        private readonly JwtServiceSettings _settings;

        /// <summary>
        /// Creates service.
        /// </summary>
        /// <param name="settings">Service settings.</param>
        public JwtService(IOptions<JwtServiceSettings> settings)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Creates JWT token.
        /// </summary>
        /// <param name="audience">Audience.</param>
        /// <param name="validityPeriod">Validity period.</param>
        /// <param name="claims">Claims to include into token.</param>
        /// <returns>Token.</returns>
        public string CreateToken(string audience, TimeSpan? validityPeriod, IEnumerable<Claim> claims)
        {
            var actualValidityPeriod = validityPeriod ?? _settings.ValidityPeriod;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = audience,
                Expires = DateTime.UtcNow.Add(actualValidityPeriod),
                Subject = new ClaimsIdentity(claims)
            };

            return CreateToken(tokenDescriptor);
        }

        /// <summary>
        /// Creates JWT token.
        /// </summary>
        /// <param name="audience">Audience.</param>
        /// <param name="expires">Expires in UTC.</param>
        /// <param name="claims">Claims to include into token.</param>
        /// <returns>Token.</returns>
        public string CreateToken(string audience, DateTime? expires, IEnumerable<Claim> claims)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = audience,
                Expires = expires,
                Subject = new ClaimsIdentity(claims)
            };

            return CreateToken(tokenDescriptor);
        }

        /// <summary>
        /// Creates JWT token for change password operation.
        /// </summary>
        /// <param name="audience">Audience.</param>
        /// <param name="claims">Claims to include into token.</param>
        /// <returns>Token.</returns>
        public string CreatePasswordExpiredToken(string audience, IEnumerable<Claim> claims)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = audience,
                Expires = DateTime.UtcNow.Add(_settings.PasswordExpiredValidityPeriod),
                Subject = new ClaimsIdentity(claims)
            };
            tokenDescriptor.Subject.AddClaim(new Claim(AUDIENCE_CLAIM_TYPE, _settings.PasswordExpiredAudience));

            return CreateToken(tokenDescriptor);
        }

        /// <summary>
        /// Creates JWT token for set password operation.
        /// </summary>
        /// <param name="audience">Audience.</param>
        /// <param name="claims">Claims to include into token.</param>
        /// <returns>Token.</returns>
        public string CreateSetPasswordToken(string audience, IEnumerable<Claim> claims)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = audience,
                Expires = DateTime.UtcNow.Add(_settings.SetPasswordValidityPeriod),
                Subject = new ClaimsIdentity(claims)
            };
            tokenDescriptor.Subject.AddClaim(new Claim(AUDIENCE_CLAIM_TYPE, _settings.SetPasswordAudience));

            return CreateToken(tokenDescriptor);
        }


        /// <summary>
        /// Creates JWT token for validate mfa code operation.
        /// </summary>
        /// <param name="audience">Audience.</param>
        /// <param name="claims">Claims to include into token.</param>
        /// <param name="validityPeriod">Validity period.</param>
        /// <returns>Token.</returns>
        public string CreateValidateMfaCodeToken(string audience, IEnumerable<Claim> claims, TimeSpan validityPeriod)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = audience,
                Expires = DateTime.UtcNow.Add(validityPeriod),
                Subject = new ClaimsIdentity(claims)
            };
            tokenDescriptor.Subject.AddClaim(new Claim(AUDIENCE_CLAIM_TYPE, _settings.SetPasswordAudience));

            return CreateToken(tokenDescriptor);
        }

        /// <summary>
        /// Returns public keys.
        /// </summary>
        /// <returns>Public keys.</returns>
        public string GetPublicKeys()
        {
            return CreatePublicKeys();
        }

        /// <summary>
        /// Returns auth parameters.
        /// </summary>
        /// <returns>Auth parameters.</returns>
        public AuthParameters GetAuthParameters()
        {
            return new AuthParameters
            {
                KeySet = CreatePublicKeys(),
                Issuer = _settings.Issuer
            };
        }

        private string CreateToken(SecurityTokenDescriptor tokenDescriptor)
        {
            tokenDescriptor.Issuer = _settings.Issuer;
            tokenDescriptor.IssuedAt = DateTime.UtcNow;
            tokenDescriptor.NotBefore = DateTime.UtcNow;
            tokenDescriptor.SigningCredentials = new SigningCredentials(
                GetRsaPrivateKey(_settings.PrivateKeyPem), SecurityAlgorithms.RsaSha256Signature);

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwt = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            var publicJwk = ConvertToJwk(GetRsaPublicKey(_settings.PublicKeyPem).Parameters);
            jwt.Header.Add("kid", publicJwk.Kid);

            var token = jwtTokenHandler.WriteToken(jwt);
            return token;
        }

        private string CreatePublicKeys()
        {
            var jsonWebKey = ConvertToJwk(GetRsaPublicKey(_settings.PublicKeyPem).Parameters);

            var jsonWebKeySet = new JsonWebKeySet();
            jsonWebKeySet.Keys.Add(jsonWebKey);

            return JsonSerializer.Serialize(jsonWebKeySet);
        }

        private RsaSecurityKey GetRsaPrivateKey(string privateKeyPem)
        {
            if (string.IsNullOrEmpty(privateKeyPem))
            {
                throw new InvalidOperationException("No private key set.");
            }

            var pemReader = new PemReader(new StringReader(privateKeyPem));
            var keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();
            var privateKeyParameters = (RsaPrivateCrtKeyParameters)keyPair?.Private;
            if (privateKeyParameters == null)
            {
                throw new InvalidOperationException("Invalid private key.");
            }

            var rsaParameters = DotNetUtilities.ToRSAParameters(privateKeyParameters);

            return new RsaSecurityKey(rsaParameters);
        }

        private RsaSecurityKey GetRsaPublicKey(string publicKeyPem)
        {
            if (string.IsNullOrEmpty(publicKeyPem))
            {
                throw new InvalidOperationException("No public key set.");
            }

            var pemReader = new PemReader(new StringReader(publicKeyPem));
            var publicKeyParameters = (RsaKeyParameters)pemReader.ReadObject();
            if (publicKeyParameters == null)
            {
                throw new InvalidOperationException("Invalid public key.");
            }

            var rsaParameters = DotNetUtilities.ToRSAParameters(publicKeyParameters);

            return new RsaSecurityKey(rsaParameters);
        }

        private JsonWebKey ConvertToJwk(RSAParameters rsaParameters)
        {
            var e = Base64UrlEncoder.Encode(rsaParameters.Exponent);
            var n = Base64UrlEncoder.Encode(rsaParameters.Modulus);
            var dict = new Dictionary<string, string>() {
                { "e", e },
                { "kty", "RSA" },
                { "n", n }
            };
            var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(System.Text.Encoding.ASCII.GetBytes(JsonSerializer.Serialize(dict)));
            var jsonWebKey = new JsonWebKey()
            {
                Kid = Base64UrlEncoder.Encode(hashBytes),
                Kty = "RSA",
                E = e,
                N = n
            };

            return jsonWebKey;
        }
    }
}


