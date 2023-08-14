using Authorization.Domain.Settings;
using Authorization.WebApi.Models.Settings;
using AutoMapper;
using Benraz.Infrastructure.Common.Paging;
using System;

namespace Authorization.WebApi
{
    class AuthorizationAutoMapperProfile : Profile
    {
        public AuthorizationAutoMapperProfile()
        {
            CreateCommonMaps();
            CreateSettingsMaps();
        }

        private void CreateCommonMaps()
        {
            CreateMap(typeof(Page<>), typeof(Page<>));
        }

        private void CreateSettingsMaps()
        {
            CreateMap<SettingsEntry, SettingsEntryViewModel>()
                .ForMember(x => x.CreateTimeUtc, o => o.MapFrom(x => SpecifyUtc(x.CreateTimeUtc)))
                .ForMember(x => x.UpdateTimeUtc, o => o.MapFrom(x => SpecifyUtc(x.UpdateTimeUtc)));
            CreateMap<AddSettingsEntryViewModel, SettingsEntry>();
            CreateMap<ChangeSettingsEntryViewModel, SettingsEntry>();
        }

        private static DateTime? SpecifyUtc(DateTime? dateTime)
        {
            return dateTime.HasValue ? DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Utc) : (DateTime?)null;
        }
    }
}

