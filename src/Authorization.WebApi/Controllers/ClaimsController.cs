using Authorization.Domain.Claims;
using Authorization.WebApi.Authorization;
using Authorization.WebApi.Filtes;
using Authorization.WebApi.Models.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Benraz.Infrastructure.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.WebApi.Controllers
{
    /// <summary>
    /// Claims controller.
    /// </summary>
    [ApiController]
    [ServiceFilter(typeof(LogUsageFilter))]
    [Route("/v{version:ApiVersion}/[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimsRepository _claimsRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates controller.
        /// </summary>
        /// <param name="claimsRepository">Claims repository.</param>
        /// <param name="mapper">Auto mapper.</param>
        public ClaimsController(IClaimsRepository claimsRepository, IMapper mapper)
        {
            _claimsRepository = claimsRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns claims.
        /// </summary>
        /// <returns>Claims.</returns>
        [HttpGet]
        [Authorize(AuthorizationPolicies.CLAIM_READ)]
        [ProducesResponseType(typeof(IEnumerable<ClaimViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetClaimsAsync()
        {
            var claims = await _claimsRepository.GetAllAsync();
            var viewModels = _mapper.Map<IEnumerable<ClaimViewModel>>(claims);

            return Ok(viewModels);
        }

        /// <summary>
        /// Adds claim.
        /// </summary>
        /// <param name="viewModel">Claim view model.</param>
        /// <returns>Claim identifier.</returns>
        [HttpPost]
        [Authorize(AuthorizationPolicies.CLAIM_ADD)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> PostClaimAsync([FromBody] ClaimViewModel viewModel)
        {
            var claims = await _claimsRepository.GetAllAsync();
            if (claims.Any(x => x.Type == viewModel.Type && x.Value == viewModel.Value))
            {
                return BadRequest("Claim already exists.");
            }

            var claim = _mapper.Map<IdentityClaim>(viewModel);
            await _claimsRepository.AddAsync(claim);

            return Ok(claim.Id);
        }

        /// <summary>
        /// Deletes claim.
        /// </summary>
        /// <param name="claimId">Claim identifier.</param>
        /// <returns>Action result.</returns>
        [HttpDelete("{claimId:Guid}")]
        [Authorize(AuthorizationPolicies.CLAIM_DELETE)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> DeleteClaimAsync([FromRoute] Guid claimId)
        {
            var claim = await _claimsRepository.GetByIdAsync(claimId);
            if (claim == null)
            {
                return NotFound("Claim not found.");
            }

            await _claimsRepository.RemoveAsync(claim);

            return NoContent();
        }
    }
}


