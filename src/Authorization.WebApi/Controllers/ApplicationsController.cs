using Authorization.Domain.Applications;
using Authorization.Domain.ApplicationTokens;
using Authorization.WebApi.Authorization;
using Authorization.WebApi.Filtes;
using Authorization.WebApi.Models.Applications;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Benraz.Infrastructure.Common.Paging;
using Benraz.Infrastructure.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.WebApi.Controllers
{
    /// <summary>
    /// Applications controller.
    /// </summary>
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(LogUsageFilter))]
    [Route("/v{version:ApiVersion}/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationsRepository _applicationsRepository;
        private readonly IApplicationTokensService _applicationTokensService;
        private readonly IApplicationTokensRepository _applicationTokensRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates controller.
        /// </summary>
        /// <param name="applicationsRepository">Applications repository.</param>
        /// <param name="applicationTokensService">Application tokens service.</param>
        /// <param name="applicationTokensRepository">Application tokens repository.</param>
        /// <param name="mapper">Mapper.</param>
        public ApplicationsController(
            IApplicationsRepository applicationsRepository, 
            IApplicationTokensService applicationTokensService,
            IApplicationTokensRepository applicationTokensRepository,
            IMapper mapper)
        {
            _applicationsRepository = applicationsRepository;
            _applicationTokensService = applicationTokensService;
            _applicationTokensRepository = applicationTokensRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns registered applications.
        /// </summary>
        /// <returns>Applications.</returns>
        [HttpGet]
        [Authorize(ApplicationPolicies.APPLICATION_READ)]
        [ProducesResponseType(typeof(Page<ApplicationViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromQuery] ApplicationsQuery query)
        {
            var applicationsPage = await _applicationsRepository.GetPageAsync(query);
            var viewModelsPage = _mapper.Map<Page<ApplicationViewModel>>(applicationsPage);

            return Ok(viewModelsPage);
        }

        /// <summary>
        /// Returns application by identifier.
        /// </summary>
        /// <returns>Application.</returns>
        [HttpGet("{id:Guid}")]
        [Authorize(ApplicationPolicies.APPLICATION_READ)]
        [ProducesResponseType(typeof(Domain.Applications.Application), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOneAsync([FromRoute] Guid id)
        {
            var application = await _applicationsRepository.GetByIdAsync(id);
            if (application == null)
            {
                return NotFound("Application not found.");
            }

            var viewModel = _mapper.Map<ApplicationViewModel>(application);
            return Ok(viewModel);
        }

        /// <summary>
        /// Registers new application.
        /// </summary>
        /// <param name="viewModel">Application.</param>
        /// <returns>Application identifier.</returns>
        [HttpPost]
        [Authorize(ApplicationPolicies.APPLICATION_ADD)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> AddAsync([FromBody] ApplicationViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest();
            }

            var application = _mapper.Map<Domain.Applications.Application>(viewModel);
            application.CreatedBy = GetUserName();
            application.UpdatedBy = GetUserName();
            await _applicationsRepository.AddAsync(application);

            return Ok(application.Id);
        }

        /// <summary>
        /// Changes application.
        /// </summary>
        /// <param name="id">Application identifier.</param>
        /// <param name="viewModel">Application.</param>
        /// <returns>Application identifier.</returns>
        [HttpPut("{id:Guid}")]
        [Authorize(ApplicationPolicies.APPLICATION_UPDATE)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> ChangeAsync([FromRoute] Guid id, [FromBody] ApplicationViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest();
            }

            var application = await _applicationsRepository.GetByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            var ssoConnections = application.SsoConnections.ToList();

            _mapper.Map(viewModel, application);

            application.UpdatedBy = GetUserName();
            foreach (var ssoConnection in ssoConnections)
            {
                var newSsoConnection = application.SsoConnections
                    .FirstOrDefault(x => x.SsoProviderCode == ssoConnection.SsoProviderCode);
                if (newSsoConnection != null && string.IsNullOrEmpty(newSsoConnection.ClientSecret))
                {
                    newSsoConnection.ClientSecret = ssoConnection.ClientSecret;
                }
            }

            await _applicationsRepository.ChangeAsync(application);

            return Ok(application.Id);
        }

        /// <summary>
        /// Deletes application.
        /// </summary>
        /// <param name="id">Application identifier.</param>
        /// <returns>Result.</returns>
        [HttpDelete("{id:Guid}")]
        [Authorize(ApplicationPolicies.APPLICATION_DELETE)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var application = await _applicationsRepository.GetByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            await _applicationsRepository.RemoveAsync(application);

            return NoContent();
        }

        /// <summary>
        /// Returns tokens of an application.
        /// </summary>
        /// <param name="id">Application identifier.</param>
        /// <returns>Application tokens.</returns>
        [HttpGet("{id:Guid}/tokens")]
        [Authorize(ApplicationPolicies.APPLICATION_READ)]
        [ProducesResponseType(typeof(IEnumerable<ApplicationTokenViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTokensAsync([FromRoute] Guid id)
        {
            var application = await _applicationsRepository.GetByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            var applicationTokens = await _applicationTokensRepository.GetAsync(id);
            var viewModels = _mapper.Map<IEnumerable<ApplicationTokenViewModel>>(applicationTokens);

            return Ok(viewModels);
        }

        /// <summary>
        /// Adds token to an application.
        /// </summary>
        /// <param name="id">Application identifier.</param>
        /// <param name="viewModel">Token request view model.</param>
        /// <returns>Application token value.</returns>
        [HttpPost("{id:Guid}/tokens")]
        [Authorize(ApplicationPolicies.APPLICATION_UPDATE)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> AddTokenAsync([FromRoute] Guid id, CreateApplicationTokenViewModel viewModel)
        {
            var application = await _applicationsRepository.GetByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            var createApplicationToken = _mapper.Map<CreateApplicationToken>(viewModel);
            createApplicationToken.ApplicationId = application.Id;
            createApplicationToken.Audience = application.Audience;

            var applicationToken = await _applicationTokensService.CreateAsync(createApplicationToken);
            applicationToken.CreatedBy = GetUserName();
            await _applicationTokensRepository.AddAsync(applicationToken);

            return Ok(applicationToken.Value);
        }

        /// <summary>
        /// Revokes application token.
        /// </summary>
        /// <param name="id">Application identifier.</param>
        /// <param name="tokenId">Token identifier.</param>
        /// <returns>Result.</returns>
        [HttpDelete("{id:Guid}/tokens/{tokenId:Guid}")]
        [Authorize(ApplicationPolicies.APPLICATION_UPDATE)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> DeleteTokenAsync([FromRoute] Guid id, [FromRoute] Guid tokenId)
        {
            var application = await _applicationsRepository.GetByIdAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            var token = await _applicationTokensRepository.GetByIdAsync(tokenId);
            if (token == null)
            {
                return NotFound();
            }

            await _applicationTokensRepository.RemoveAsync(token);

            return NoContent();
        }

        private string? GetUserName()
        {
            return HttpContext.User?.Identity?.Name;
        }
    }
}