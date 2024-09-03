using Authorization.Domain;
using Authorization.Domain.Applications;
using Authorization.Domain.ApplicationTokens;
using Authorization.Domain.SsoServices;
using Authorization.Infrastructure.Jwt;
using Authorization.WebApi.Authorization;
using Authorization.WebApi.Filtes;
using Authorization.WebApi.Models.Auth;
using Authorization.WebApi.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Benraz.Infrastructure.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace Authorization.WebApi.Controllers
{
    /// <summary>
    /// Authorization controller.
    /// </summary>
    [ApiController]
    [ServiceFilter(typeof(LogUsageFilter))]
    [Route("/v{version:ApiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IApplicationsRepository _applicationsRepository;
        private readonly IApplicationTokensRepository _applicationTokensRepository;
        private readonly IJwtService _jwtService;
        private readonly IUsageLogsService _usageLogsService;
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates controller.
        /// </summary>
        /// <param name="authorizationService">Authorization service.</param>
        /// <param name="applicationsRepository">Applications repository.</param>
        /// <param name="applicationTokensRepository">Application tokens repository.</param>
        /// <param name="jwtService">JWT service.</param>
        /// <param name="usageLogsService">Usage logs service.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="mapper">Mapper.</param>
        public AuthController(
            IAuthorizationService authorizationService,
            IApplicationsRepository applicationsRepository,
            IApplicationTokensRepository applicationTokensRepository,
            IJwtService jwtService,
            IUsageLogsService usageLogsService,
            ILogger<AuthController> logger,
            IMapper mapper)
        {
            _authorizationService = authorizationService;
            _applicationsRepository = applicationsRepository;
            _applicationTokensRepository = applicationTokensRepository;
            _jwtService = jwtService;
            _usageLogsService = usageLogsService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Logging in a user.
        /// </summary>
        /// <param name="model">Login model.</param>
        /// <returns>Redirect URL.</returns>
        [HttpGet("login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status307TemporaryRedirect)]
        public async Task<IActionResult> LoginAsync([FromQuery] LoginViewModel model)
        {
            try
            {
                var authorizationUrl = await _authorizationService
                    .CreateAuthorizationUrlAsync(model.ApplicationId, model.SsoProviderCode, model.ReturnUrl);

                return Redirect(authorizationUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating authorization URL.");

                var errorCallbackUrl = await _authorizationService.CreateErrorCallbackUrlAsync(
                    model.ApplicationId, "Unable to authorize user");

                return Redirect(errorCallbackUrl);
            }
        }

        /// <summary>
        /// Issues a token.
        /// </summary>
        /// <param name="model">Token request.</param>
        /// <returns>Token response.</returns>
        [HttpPost("token")]
        [ProducesResponseType(typeof(TokenResponseViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(TokenResponseViewModel), StatusCodes.Status226IMUsed)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IssueTokenAsync([FromBody] TokenRequestViewModel model)
        {
            if (model.GrantType != "password")
            {
                return BadRequest(TokenResponseViewModel.CreateError("Unsupported grant type"));
            }
            try
            {
                var accessTokenResult = await _authorizationService
                    .AuthorizeAsync(model.ApplicationId, model.SsoProviderCode, model.Username, model.Password);

                await _usageLogsService.LogUsageAsync(HttpContext, model.Username, "Token successfully created");

                var viewModel = TokenResponseViewModel.CreateSuccess(accessTokenResult.AccessToken);
                var statusCode = accessTokenResult.IsPasswordExpired ?
                    StatusCodes.Status226IMUsed :
                    StatusCodes.Status200OK;

                return StatusCode(statusCode, viewModel);
            }
            catch (AuthorizationFailedException ex)
            {
                _logger.LogError(ex, "Error while authorizing user.");
                await _usageLogsService.LogUsageAsync(HttpContext, model.Username, $"Operation failed: {ex.Reason}");

                return Unauthorized(TokenResponseViewModel.CreateError(ex.Reason));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while authorizing user.");
                await _usageLogsService.LogUsageAsync(
                    HttpContext, model.Username, "Operation failed: Unable to authorize user");

                return BadRequest(TokenResponseViewModel.CreateError("Unable to authorize user"));
            }
        }

        /// <summary>
        /// Exchanges an access token to a new one.
        /// </summary>
        /// <param name="model">Exchange token request.</param>
        /// <returns>Token response.</returns>
        [HttpPost("token-exchange")]
        [Authorize(ApplicationPolicies.TOKEN_EXCHANGE)]
        [ProducesResponseType(typeof(TokenResponseViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ExchangeTokenAsync([FromBody] ExchangeTokenRequestViewModel model)
        {
            try
            {
                var accessTokenResult = await _authorizationService
                    .AuthorizeAsync(model.ApplicationId, model.AccessToken);

                await _usageLogsService.LogUsageAsync(
                    HttpContext, accessTokenResult.UserId, "Token successfully exchanged");

                var viewModel = TokenResponseViewModel.CreateSuccess(accessTokenResult.AccessToken);

                return Ok(viewModel);
            }
            catch (AuthorizationFailedException ex)
            {
                _logger.LogError(ex, "Error while exchanging user token.");
                await _usageLogsService.LogUsageAsync(HttpContext, message: $"Operation failed: {ex.Reason}");

                return Unauthorized(TokenResponseViewModel.CreateError(ex.Reason));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while exchanging user token.");
                await _usageLogsService.LogUsageAsync(
                    HttpContext, message: "Operation failed: Unable to exchange user token");

                return BadRequest(TokenResponseViewModel.CreateError("Unable to exchange user token"));
            }
        }

        /// <summary>
        /// Returns public keys.
        /// </summary>
        /// <returns>Public keys.</returns>
        [HttpGet("keys")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult GetKeys()
        {
            var keys = _jwtService.GetPublicKeys();
            return Ok(keys);
        }

        /// <summary>
        /// Returns auth parameters.
        /// </summary>
        /// <returns>Auth parameters.</returns>
        [HttpGet("parameters")]
        [ProducesResponseType(typeof(AuthParametersViewModel), StatusCodes.Status200OK)]
        public IActionResult GetParameters()
        {
            var authParameters = _jwtService.GetAuthParameters();
            var viewModel = _mapper.Map<AuthParametersViewModel>(authParameters);

            return Ok(viewModel);
        }

        /// <summary>
        /// Checks if token is active.
        /// </summary>
        /// <param name="tokenId">Token identifier.</param>
        /// <returns>Result.</returns>
        [HttpGet("token/{tokenId:Guid}/is-active")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> IsTokenActiveAsync([FromRoute] Guid tokenId)
        {
            var applicationToken = await _applicationTokensRepository.GetByIdAsync(tokenId);

            var isActive = applicationToken != null && applicationToken.ExpirationTimeUtc > DateTime.UtcNow;
            if (!isActive)
            {
                _logger.LogWarning($"Token {tokenId} is not active.");
            }

            return Ok(isActive);
        }

        /// <summary>
        /// Returns enabled SSO providers for application.
        /// </summary>
        /// <param name="applicationId">Application identifier.</param>
        /// <returns>Enabled SSO providers.</returns>
        [HttpGet("applications/{applicationId:Guid}/sso-providers")]
        [ProducesResponseType(typeof(ICollection<SsoProviderViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSsoProvidersAsync([FromRoute] Guid applicationId)
        {
            var application = await _applicationsRepository.GetByIdAsync(applicationId);
            if (application == null)
            {
                return NotFound();
            }

            var ssoConnections = application.GetEnabledSsoConnections();
            var viewModels = _mapper.Map<ICollection<SsoProviderViewModel>>(ssoConnections);

            return Ok(viewModels);
        }

        /// <summary>
        /// Processes Microsoft authentication callback.
        /// </summary>
        /// <param name="state">State.</param>
        /// <param name="code">Authorization code.</param>
        /// <returns>Redirect URL.</returns>
        [HttpGet("microsoft-callback")]
        [ProducesResponseType(typeof(string), StatusCodes.Status307TemporaryRedirect)]
        public async Task<IActionResult> ProcessMicrosoftAuthCodeAsync(string state, string code)
        {
            var ssoState = SsoState.FromString(state);
            var callbackUrl = await ProcessCallbackAsync(SsoProviderCode.Microsoft, code, ssoState);

            return Redirect(callbackUrl);
        }

        /// <summary>
        /// Processes Facebook authentication callback.
        /// </summary>
        /// <param name="state">State.</param>
        /// <param name="code">Authorization code.</param>
        /// <returns>Redirect URL.</returns>
        [HttpGet("facebook-callback")]
        [ProducesResponseType(typeof(string), StatusCodes.Status307TemporaryRedirect)]
        public async Task<IActionResult> ProcessFacebookAuthCodeAsync(string state, string code)
        {
            var ssoState = SsoState.FromString(state);
            var callbackUrl = await ProcessCallbackAsync(SsoProviderCode.Facebook, code, ssoState);

            return Redirect(callbackUrl);
        }

        /// <summary>
        /// Processes Google authentication callback.
        /// </summary>
        /// <param name="state">State.</param>
        /// <param name="code">Authorization code.</param>
        /// <returns>Redirect URL.</returns>
        [HttpGet("google-callback")]
        [ProducesResponseType(typeof(string), StatusCodes.Status307TemporaryRedirect)]
        public async Task<IActionResult> ProcessGoogleAuthCodeAsync(string state, string code)
        {
            var ssoState = SsoState.FromString(state);
            var callbackUrl = await ProcessCallbackAsync(SsoProviderCode.Google, code, ssoState);

            return Redirect(callbackUrl);
        }

        private async Task<string> ProcessCallbackAsync(SsoProviderCode provider, string code, SsoState ssoState)
        {
            try
            {
                var accessTokenResult = await _authorizationService
                    .AuthorizeAsync(ssoState.ApplicationId, provider, code);

                await _usageLogsService.LogUsageAsync(
                    HttpContext, accessTokenResult.UserId, "Token successfully created");

                var application = await _applicationsRepository.GetByIdAsync(ssoState.ApplicationId);
                if (application.IsAccessTokenCookieEnabled)
                {
                    Response.Cookies.Append(application.AccessTokenCookieName, accessTokenResult.AccessToken);
                }

                var callbackUrl = await _authorizationService.CreateSuccessCallbackUrlAsync(
                    ssoState.ApplicationId, accessTokenResult.AccessToken, ssoState.ReturnUrl, provider: provider);

                return callbackUrl;
            }
            catch (AuthorizationFailedException ex)
            {
                _logger.LogError(ex, "Error while processing callback.");
                return await _authorizationService.CreateErrorCallbackUrlAsync(ssoState.ApplicationId, ex.Reason);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing callback.");
                return await _authorizationService.CreateErrorCallbackUrlAsync(
                    ssoState.ApplicationId, "Unable to authorize user");
            }
        }
    }
}


