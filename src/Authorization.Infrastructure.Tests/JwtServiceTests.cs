using Authorization.Infrastructure.Jwt;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using NUnit.Framework;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authorization.Infrastructure.Tests
{
    [TestFixture]
    public class JwtServiceTests
    {
        private JwtServiceSettings _settings;
        private JwtService _service;

        [SetUp]
        public void SetUp()
        {
            IdentityModelEventSource.ShowPII = true;

            _settings = new JwtServiceSettings
            {
                Issuer = "Issuer",
                ValidityPeriod = TimeSpan.FromDays(1),
                PrivateKeyPem = @"-----BEGIN RSA PRIVATE KEY-----
MIIEpAIBAAKCAQEAi6/h3mBn0lwoKXZHLnzk9gKkOr4x7Tk9hvjtyCyIh+9PFUai
ZdQbeLvVYY4fZBRyjxptFg0lvV1geK7oj2PomocsAYR2GnVR0lGs2r0oTLQdZjcJ
A2tLjdqS7xU29K5ZO9E37+PMIsnvl6fDIvVxVqw3s42RKa8zzaglZWn+1lJXCg+s
bNpVb9awjh1DOY/NxTLcK+dRGe82sf7sIOieaSHYPj3MIOcTw6Lve4kI/2dv3UUF
TM3j+5jf18gZyzUn/PRpxD6Eoyg1KYlq2tnrCCokzDdKF30bJZ6zh61MSEPeXoya
+sN5V+uXhesPoTDQWPGH/OI3YBCS5rRzzYQGaQIDAQABAoIBAEguPW/UrzFjT5mF
NekOvp1k5go4x8HGZ0W8kdpzMKXVgi8DTd2CiiOMJAoCD7R+YGgUBrRzXMIKY1RZ
RXD69nILUTEDiqfHYd1lOQXa4t0n4V01KSIsKmGFxZFni/tJsHCZfucl9hEv+e+K
z+nbnJJl8W6Fw7Ifh4xIy/IooHeMV1ywqWWPmDPIgqEIC7yBO8f8Z1KzTFupjTn8
kMfR75YH1M57M8EkD60WU90g+DxHStdIHwpSw5LVwh4OhNQ+zetP+D/DBkkMa0QF
MXSX0kAEX6zmbKmXJvWgE7XIwOfg4THZ3UMu9dqUC4IjE6yT69hz2QgWC6h/02d+
b53cBoECgYEAv4DRu7fhREE8g3nZdlevwJ+qm4ojEzepuDIz0eyogrZShqWVfBzF
hneHXS6pKSYtR1nA2FgSOeYoMNUdsxpCCxIho8iYpASfaAscvR75oQ09mPhoPRMi
G8S0LGvaYNUpfG55o2FQSrTQQvncFOUf/zc6FSsPOrn5yzhKZ+bbBDECgYEAuruI
3l+mVLrwCG88yXQobFNg9rrrbeqxfxuBB3/JTwjol1RlUbMFEcR/xaXlROLO/MNT
Ync/gGdzK8mnudjAuHd7gMso47XlLsDKPPLoG0tb8bn0bY7vr/X+zLOqpysIeUaZ
Tg5kgGF2IimCZ99ouOiR1ELaJcpmm7iHq8nJL7kCgYEAi8AkTktINxksYf3/9l7T
QUzDojJSmmmaj3MDYgTEjG8lFmGsDyCn/RMpU5UDmaXkkRcspjTPSDVvywMbY2Kc
I5wdtHpk+fztn23MkS8TmLYi54kP+NIHqCAKJAipGtU0KzwrxFD12S7OdLIGK7Fx
gjv5RQ1hVcf2RJlVozFTT2ECgYEAoqrpwWYrnJY4sfBNUOU6rZs41dbhbqBLvYG1
NlYwhQQqsmGX3cMIwICgGgq5nffC/tgdFKLzE6WK4/NIhJJ42Hllgj34wd1b6A2U
X+HvJo/QadRnROWGkY/HRoWhPP005YkF1cSd8mR0p6/nZRtuu94F45XVOaSHHFJ1
BcbD1ykCgYBaBaZgGngnD1ENmIQYVbuGpZWFfae5TZkfNJDSM1RBED2pDdVU8+9t
TXH7oXkPod22Pa56ysd4Qe15jax7PgfFiLFTC2JzZzLxXrIFqxkbqq6iJX/Z77Fr
kyUvooragJb2GUupaTeGYU3GxQlf6zSkbfRJ+CROJ7FQN8uQHNmlSw==
-----END RSA PRIVATE KEY-----",
                PublicKeyPem = @"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAi6/h3mBn0lwoKXZHLnzk
9gKkOr4x7Tk9hvjtyCyIh+9PFUaiZdQbeLvVYY4fZBRyjxptFg0lvV1geK7oj2Po
mocsAYR2GnVR0lGs2r0oTLQdZjcJA2tLjdqS7xU29K5ZO9E37+PMIsnvl6fDIvVx
Vqw3s42RKa8zzaglZWn+1lJXCg+sbNpVb9awjh1DOY/NxTLcK+dRGe82sf7sIOie
aSHYPj3MIOcTw6Lve4kI/2dv3UUFTM3j+5jf18gZyzUn/PRpxD6Eoyg1KYlq2tnr
CCokzDdKF30bJZ6zh61MSEPeXoya+sN5V+uXhesPoTDQWPGH/OI3YBCS5rRzzYQG
aQIDAQAB
-----END PUBLIC KEY-----"
            };
            _service = new JwtService(Options.Create(_settings));
        }

        [Test]
        public void CreateToken_CorrectClaims_CreatesTokenWithClaims()
        {
            var token = _service.CreateToken("audience", (TimeSpan?)null, new Claim[] { new Claim("type", "value") });

            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            CheckJwtTokenClaim(jwtToken, new Claim("type", "value", null, _settings.Issuer));
            CheckJwtTokenValidity(jwtToken, DateTime.UtcNow, DateTime.UtcNow.Add(_settings.ValidityPeriod));
        }

        [Test]
        public void CreateToken_CustomValidityPeriodAndClaims_CreatesCorrectToken()
        {
            var token = _service.CreateToken(
                "audience", TimeSpan.FromHours(1), new Claim[] { new Claim("type", "value") });

            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            CheckJwtTokenClaim(jwtToken, new Claim("type", "value", null, _settings.Issuer));
            CheckJwtTokenValidity(jwtToken, DateTime.UtcNow, DateTime.UtcNow.AddHours(1));
        }

        [Test]
        public void GetKeys_CertificateExists_ReturnsPublicKey()
        {
            var keys = _service.GetPublicKeys();
            keys.Should().NotBeNullOrEmpty();
        }

        private void CheckJwtTokenClaim(JwtSecurityToken jwtToken, Claim expectedClaim)
        {
            jwtToken.Claims.Should().ContainEquivalentOf(expectedClaim);
        }

        private void CheckJwtTokenValidity(
            JwtSecurityToken jwtToken, DateTime expectedValidFrom, DateTime expectedValidTo)
		{
            // Convert the time difference to TimeSpan
			TimeSpan tolerance = TimeSpan.FromMilliseconds(60000);
			jwtToken.ValidFrom.Should().BeCloseTo(expectedValidFrom, tolerance);
            jwtToken.ValidTo.Should().BeCloseTo(expectedValidTo, tolerance);
        }
    }
}


