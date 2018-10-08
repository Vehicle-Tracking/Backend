using System;
using System.Collections.Generic;
using AutoMapper;
using Avt.Web.Backend.Data.Entities;
using Avt.Web.Backend.Data.Types;
using Avt.Web.Backend.DTO;

namespace Avt.Web.Backend.Utils
{
   public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.AllowNullCollections = false;

            CreateMap<VehicleStatusDto, VehicleStatusDetail>()
                .ForMember(des => des.VehicleId,
                    opt => opt.MapFrom(src => src.VehicleId))
                .ForMember(des => des.SyncDate,
                    opt => opt.MapFrom(src => ParseDate(src.StatusDate)))
                .ForMember(des => des.Status,
                    opt => opt.MapFrom(src =>
                        src.VehicleStatus == VehicleStatus.Unknown
                            ? VehicleStatus.Disconnected
                            : src.VehicleStatus))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<VehicleStatusDetail, StatusClientDto>()
                .ForMember(des => des.VehicleId,
                    opt => opt.MapFrom(src => src.VehicleId))
                .ForMember(des => des.RegNumber,
                    opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.RegNumber : ""))
                .ForMember(des => des.Status,
                    opt => opt.MapFrom(src => src.Status))
                .ForMember(des => des.OwnerName,
                    opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.OwnerId : ""))
                .ForMember(des => des.OwnerAddress,
                    opt => opt.MapFrom(src =>
                        src.Vehicle != null && src.Vehicle.Owner != null ? src.Vehicle.Owner.Address : ""))
                .ForMember(des => des.StatusDate,
                    opt => opt.MapFrom(src => src.SyncDate.ToString("HH:mm:ss (dd MMMM)")))
                .ForMember(des => des.StatusDateNumber,
                    opt => opt.MapFrom(src => src.SyncDate.Ticks))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<VehicleAggregateOverview, OverviewClientDto>()
                .ForMember(des => des.Vehicle,
                    opt => opt.MapFrom(src => new VehicleClientDto()
                    {
                        VehicleId = src.Id,  
                        RegistrationNumber = src.RegNumber, 
                        Owner = new OwnerDto()
                        {
                            Name =  src.OwnerName
                        }
                    }))
                .ForMember(des => des.Availiblity,
                    opt => opt.MapFrom(src => src.Total > 0
                        ? (float) (Math.Truncate(((double) src.ConnectedStatusCount / src.Total) * 100.0) / 100.0)
                        : 0.00f))
                .ForMember(des => des.LastStatus,
                    opt => opt.MapFrom(src => src.LastStatus))
                .ForMember(des => des.LastSync,
                    opt => opt.MapFrom(src => src.LastSync.ToString("yyyy-MM-dd HH:mm:ss").Replace(" ", "T")))
                .ForMember(des => des.LastSyncStr,
                    opt => opt.MapFrom(src => src.LastSync.ToString("HH:mm:ss (dd MMMM)")))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

           
        }

        private static DateTime ParseDate(string date)
        {
            if (!DateTime.TryParse(date, out var statusDate))
                statusDate = DateTime.UtcNow.AddSeconds(-5);
            return statusDate;
        }

    }
}
