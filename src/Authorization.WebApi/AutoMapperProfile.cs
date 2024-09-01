using Authorization.Domain.Applications;
using Authorization.Domain.ApplicationTokens;
using Authorization.Domain.Claims;
using Authorization.Domain.SsoConnections;
using Authorization.Domain.Users;
using Authorization.Infrastructure.Jwt;
using Authorization.WebApi.Models.Applications;
using Authorization.WebApi.Models.Auth;
using Authorization.WebApi.Models.Claims;
using Authorization.WebApi.Models.InternalLogin;
using Authorization.WebApi.Models.Roles;
using Authorization.WebApi.Models.Users;
using Authorization.WebApi.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Benraz.Infrastructure.Common.Paging;
using Benraz.Infrastructure.Domain.Authorization;
using System;
using System.Linq;
using System.Security.Claims;

namespace Authorization.WebApi
{
    class AutoMapperProfile : Profile
    {
        private readonly IMaskingDataService _maskingDataService;

        public AutoMapperProfile(IMaskingDataService maskingDataService)
        {
            _maskingDataService = maskingDataService;

            CreateCommonMaps();
            CreateAuthMaps();
            CreateInternalLoginMaps();
            CreateApplicationsMaps();
            CreateUsersMaps();
            CreateRolesMaps();
            CreateClaimsMaps();
        }

        private void CreateCommonMaps()
        {
            CreateMap(typeof(Page<>), typeof(Page<>));
        }

        private void CreateInternalLoginMaps()
        {
            CreateMap<SignUpViewModel, RegisterUserModel>();
            CreateMap<RegisterUserResult, SignUpResultViewModel>();
        }

        private void CreateAuthMaps()
        {
            CreateMap<AuthParameters, AuthParametersViewModel>();
            CreateMap<SsoConnection, SsoProviderViewModel>()
                .ForMember(x => x.Code, o => o.MapFrom(x => x.SsoProviderCode));
        }

        private void CreateApplicationsMaps()
        {
            CreateMap<Application, ApplicationViewModel>()
                .ForMember(x => x.CreateTimeUtc, o => o.MapFrom(x => SpecifyUtc(x.CreateTimeUtc)))
                .ForMember(x => x.UpdateTimeUtc, o => o.MapFrom(x => SpecifyUtc(x.UpdateTimeUtc)))
                .ReverseMap()
                .ForMember(x => x.Id, o => o.Ignore())
                .ForMember(x => x.CreatedBy, o => o.Ignore())
                .ForMember(x => x.CreateTimeUtc, o => o.Ignore())
                .ForMember(x => x.UpdatedBy, o => o.Ignore())
                .ForMember(x => x.UpdateTimeUtc, o => o.Ignore());

            CreateMap<SsoConnection, ApplicationSsoConnectionViewModel>()
                .ForMember(x => x.ClientSecret, o => o.MapFrom(x => _maskingDataService.HideSecret(x.ClientSecret)))
                .ReverseMap()
                .ForMember(x => x.ApplicationId, o => o.Ignore())
                .ForMember(x => x.SsoProvider, o => o.Ignore())
                .ForMember(x => x.CreateTimeUtc, o => o.MapFrom(x => DateTime.UtcNow))
                .ForMember(x => x.UpdateTimeUtc, o => o.MapFrom(x => DateTime.UtcNow))
                .ForMember(x => x.ClientSecret, o => o.MapFrom(x => x.NewClientSecret));

            CreateMap<ApplicationUrl, ApplicationUrlViewModel>()
                .ReverseMap()
                .ForMember(x => x.Id, o => o.Ignore());

            CreateMap<ApplicationToken, ApplicationTokenViewModel>()
                .ForMember(x => x.Roles, o => o.MapFrom(x => x.Roles.Select(y => y.Role.Name)))
                .ForMember(x => x.Claims,
                    o => o.MapFrom(x => x.Claims.Select(y => new Claim(y.Claim.Type, y.Claim.Value))))
                .ForMember(x => x.CreateTimeUtc, o => o.MapFrom(x => SpecifyUtc(x.CreateTimeUtc)))
                .ForMember(x => x.ExpirationTimeUtc, o => o.MapFrom(x => SpecifyUtc(x.ExpirationTimeUtc)));

            CreateMap<CreateApplicationTokenViewModel, CreateApplicationToken>();

            CreateMap<Claim, ApplicationTokenClaimViewModel>().ReverseMap();
            CreateMap<ApplicationTokenCustomField, ApplicationTokenCustomFieldViewModel>()
                .ReverseMap();
        }

        private void CreateUsersMaps()
        {
            CreateMap<User, UserInfoViewModel>()
                .ForMember(x => x.IsSsoOnly, o => o.MapFrom(x => string.IsNullOrEmpty(x.PasswordHash)));

            CreateMap<CreateUserViewModel, User>()
                .ForMember(x => x.StatusCode, o => o.MapFrom(x => UserStatusCode.Active));

            CreateMap<ChangeUserViewModel, User>();

            CreateMap<Claim, UserClaimViewModel>().ReverseMap();
        }

        private void CreateRolesMaps()
        {
            CreateMap<IdentityRole, RoleViewModel>()
                .ReverseMap()
                .ForMember(x => x.Id, o => o.Ignore());

            CreateMap<Claim, RoleClaimViewModel>().ReverseMap();
        }

        private void CreateClaimsMaps()
        {
            CreateMap<IdentityClaim, ClaimViewModel>().ReverseMap();
        }

        private static DateTime? SpecifyUtc(DateTime? dateTime)
        {
            return dateTime.HasValue ? DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc) : (DateTime?)null;
        }
    }
}


