using Authorization.Domain.Roles;
using Authorization.WebApi.Authorization;
using Authorization.WebApi.Filtes;
using Authorization.WebApi.Models.Roles;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Benraz.Infrastructure.Web.Filters;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Authorization.WebApi.Controllers
{
    /// <summary>
    /// Roles controller.
    /// </summary>
    [ApiController]
    [ServiceFilter(typeof(LogUsageFilter))]
    [Route("/v{version:ApiVersion}/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoleStore<IdentityRole> _roleStore;
        private readonly IRolesRepository _rolesRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates controller.
        /// </summary>
        /// <param name="roleManager">Role manager.</param>
        /// <param name="roleStore">Role store.</param>
        /// <param name="rolesRepository">Roles repository.</param>
        /// <param name="mapper">Auto mapper.</param>
        public RolesController(
            RoleManager<IdentityRole> roleManager,
            IRoleStore<IdentityRole> roleStore,
            IRolesRepository rolesRepository,
            IMapper mapper)
        {
            _roleManager = roleManager;
            _roleStore = roleStore;
            _rolesRepository = rolesRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns roles.
        /// </summary>
        /// <returns>Roles.</returns>
        [HttpGet]
        [Authorize(ApplicationPolicies.ROLE_READ)]
        [ProducesResponseType(typeof(IEnumerable<RoleViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRolesAsync()
        {
            var roles = await _rolesRepository.GetAllAsync();
            var viewModels = _mapper.Map<IEnumerable<RoleViewModel>>(roles);

            return Ok(viewModels);
        }

        /// <summary>
        /// Returns role by identifier.
        /// </summary>
        /// <param name="roleId">Role identifier.</param>
        /// <returns>Role.</returns>
        [HttpGet("{roleId}")]
        [Authorize(ApplicationPolicies.ROLE_READ)]
        [ProducesResponseType(typeof(RoleViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoleAsync([FromRoute] string roleId)
        {
            var role = await _rolesRepository.GetByIdAsync(roleId);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            var viewModel = _mapper.Map<RoleViewModel>(role);

            return Ok(viewModel);
        }

        /// <summary>
        /// Adds role.
        /// </summary>
        /// <param name="viewModel">Role view model.</param>
        /// <returns>Role identifier.</returns>
        [HttpPost]
        [Authorize(ApplicationPolicies.ROLE_ADD)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> PostRoleAsync([FromBody] RoleViewModel viewModel)
        {
            if (await _roleManager.RoleExistsAsync(viewModel.Name))
            {
                return BadRequest("Role already exists.");
            }

            var role = _mapper.Map<IdentityRole>(viewModel);
            await _roleManager.CreateAsync(role);
            await _roleStore.UpdateAsync(role, CancellationToken.None);

            return Ok(role.Id);
        }

        /// <summary>
        /// Updates role.
        /// </summary>
        /// <param name="roleId">Role identifier.</param>
        /// <param name="viewModel">Role view model.</param>
        /// <returns>Action result.</returns>
        [HttpPut("{roleId}")]
        [Authorize(ApplicationPolicies.ROLE_UPDATE)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> PutRoleAsync([FromRoute] string roleId, [FromBody] RoleViewModel viewModel)
        {
            var role = await _rolesRepository.GetByIdAsync(roleId);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            _mapper.Map(viewModel, role);
            await _roleManager.UpdateAsync(role);
            await _roleStore.UpdateAsync(role, CancellationToken.None);

            return NoContent();
        }

        /// <summary>
        /// Deletes role.
        /// </summary>
        /// <param name="roleId">Role identifier.</param>
        /// <returns>Action result.</returns>
        [HttpDelete("{roleId}")]
        [Authorize(ApplicationPolicies.ROLE_DELETE)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> DeleteRoleAsync([FromRoute] string roleId)
        {
            var role = await _rolesRepository.GetByIdAsync(roleId);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            await _roleManager.DeleteAsync(role);
            await _roleStore.UpdateAsync(role, CancellationToken.None);

            return NoContent();
        }

        /// <summary>
        /// Returns role claims.
        /// </summary>
        /// <param name="roleId">Role identifier.</param>
        /// <returns>Role claims.</returns>
        [HttpGet("{roleId}/claims")]
        [Authorize(ApplicationPolicies.ROLE_READ)]
        [ProducesResponseType(typeof(RoleViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClaimsAsync([FromRoute] string roleId)
        {
            var role = await _rolesRepository.GetByIdAsync(roleId);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            var claims = await _roleManager.GetClaimsAsync(role);
            var viewModels = _mapper.Map<IEnumerable<RoleClaimViewModel>>(claims);

            return Ok(viewModels);
        }

        /// <summary>
        /// Updates role claims.
        /// </summary>
        /// <param name="roleId">Role identifier.</param>
        /// <param name="viewModels">Claims view models.</param>
        /// <returns>Action result.</returns>
        [HttpPut("{roleId}/claims")]
        [Authorize(ApplicationPolicies.ROLE_UPDATE)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(DRFilterAttribute))]
        public async Task<IActionResult> PutClaimsAsync(
            [FromRoute] string roleId, [FromBody] IEnumerable<RoleClaimViewModel> viewModels)
        {
            var role = await _rolesRepository.GetByIdAsync(roleId);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            var newClaims = _mapper.Map<IEnumerable<Claim>>(viewModels);
            var oldClaims = await _roleManager.GetClaimsAsync(role);

            foreach (var oldClaim in oldClaims)
            {
                await _roleManager.RemoveClaimAsync(role, oldClaim);
            }

            foreach (var newClaim in newClaims)
            {
                await _roleManager.AddClaimAsync(role, newClaim);
            }

            await _roleStore.UpdateAsync(role, CancellationToken.None);

            return NoContent();
        }
    }
}


