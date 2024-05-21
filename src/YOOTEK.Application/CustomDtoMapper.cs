using AutoMapper;
using Yootek.Authorization.Permissions.Dto;
using Yootek.EntityDb;
using Yootek.Services;
using Yootek.Services.Dto;
using System.Collections;
using System.Collections.Generic;
using Permission = Abp.Authorization.Permission;
using System.Reflection;
using System;
using AutoMapper.Internal;
using AutoMapper.Configuration;
using System.Linq;

namespace Yootek
{
    internal static class CustomDtoMapper
    {
        private static volatile bool _mappedBefore;
        private static readonly object SyncObj = new object();

        public static void CreateMappings(IMapperConfigurationExpression mapper)
        {
            lock (SyncObj)
            {
                if (_mappedBefore)
                {
                    return;
                }

                CreateMappingsInternal(mapper);

                _mappedBefore = true;
            }
        }

        private static void CreateMappingsInternal(IMapperConfigurationExpression mapper)
        {
            //Permission
            mapper.CreateMap<Permission, FlatPermissionDto>();
            mapper.CreateMap<Permission, FlatPermissionWithLevelDto>();
            
           
            // MeterType
            mapper.CreateMap<CreateMeterTypeInput, MeterType>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)));
            
            mapper.CreateMap<CreateMeterTypeByUserInput, MeterType>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)));

            mapper.CreateMap<UpdateMeterTypeInput, MeterType>()
                .ForMember(dest => dest.Name,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.Description,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.BillType,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)));
                
            mapper.CreateMap<UpdateMeterTypeByUserInput, MeterType>()
                .ForMember(dest => dest.Name,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.Description,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.BillType,
                opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)));

            // Meter
            mapper.CreateMap<CreateMeterInput, Meter>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)));
            
            mapper.CreateMap<CreateMeterByUserInput, Meter>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)));
            
            mapper.CreateMap<UpdateMeterInput, Meter>()
                .ForMember(dest => dest.Name,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.QrCode,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.Code,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.MeterTypeId,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.UrbanId,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.BuildingId,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.ApartmentCode,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)));
            
            mapper.CreateMap<UpdateMeterByUserInput, Meter>()
                .ForMember(dest => dest.Name,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.QrCode,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.Code,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.MeterTypeId,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.UrbanId,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.BuildingId,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.ApartmentCode,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)));

             // MeterMonthly
            mapper.CreateMap<CreateMeterMonthlyInput, MeterMonthly>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)));
            
            mapper.CreateMap<CreateMeterMonthlyByUserInput, MeterMonthly>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)));
            
            mapper.CreateMap<UpdateMeterMonthlyInput, MeterMonthly>()
                .ForMember(dest => dest.MeterId,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.Period,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.Value,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)));
            
            mapper.CreateMap<UpdateMeterMonthlyByUserInput, MeterMonthly>()
                .ForMember(dest => dest.MeterId,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.Period,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)))
                .ForMember(dest => dest.Value,
                    opt => opt.Condition((src, dest, srcMember) => IsNotNullOrDefault(srcMember)));
            
        }

        #region method helpers 
        private static bool IsNotNullOrDefault<T>(T srcMember)
        {
            if (srcMember is IEnumerable list)
            {
                return list.GetEnumerator().MoveNext();
            }
            return srcMember != null && !EqualityComparer<T>.Default.Equals(srcMember, default);
        }
        #endregion
    }

    public static class AutoMapperExtensions
    {
        private static readonly PropertyInfo TypeMapActionsProperty = typeof(TypeMapConfiguration).GetProperty("TypeMapActions", BindingFlags.NonPublic | BindingFlags.Instance);

        // not needed in AutoMapper 12.0.1
        private static readonly PropertyInfo DestinationTypeDetailsProperty = typeof(TypeMap).GetProperty("DestinationTypeDetails", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void ForAllOtherMembers<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression, Action<IMemberConfigurationExpression<TSource, TDestination, object>> memberOptions)
        {
            var typeMapConfiguration = (TypeMapConfiguration)expression;

            var typeMapActions = (List<Action<TypeMap>>)TypeMapActionsProperty.GetValue(typeMapConfiguration);

            typeMapActions.Add(typeMap =>
            {
                var destinationTypeDetails = (TypeDetails)DestinationTypeDetailsProperty.GetValue(typeMap);

                foreach (var accessor in destinationTypeDetails.WriteAccessors.Where(m => typeMapConfiguration.GetDestinationMemberConfiguration(m) == null))
                {
                    expression.ForMember(accessor.Name, memberOptions);
                }
            });
        }
    }
}