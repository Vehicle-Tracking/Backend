using System;
using System.Threading.Tasks;
using AutoMapper;
using Avt.Web.Backend.Data.Entities;
using Avt.Web.Backend.Data.Repositories;
using Avt.Web.Backend.DTO;
using Avt.Web.Backend.Helper;
using Microsoft.AspNetCore.SignalR;

namespace Avt.Web.Backend.Service
{
    public class OverviewService
    {
        private readonly IHubContext<StatusOverviewHub> _overviewHub;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehicleOverviewRepository _vehicleOverviewRepository;

        public OverviewService(IHubContext<StatusOverviewHub> overviewHub,
            IVehicleRepository vehicleRepository,
            IVehicleOverviewRepository vehicleOverviewRepository)
        {
            _overviewHub = overviewHub;
            _vehicleRepository = vehicleRepository;
            _vehicleOverviewRepository = vehicleOverviewRepository;
        }

        public async Task SendVehicleOverviewStatusAsync(VehicleStatusDto dto)
        {
            var vehicle = await _vehicleRepository.GetWithOwnerAsync(dto.VehicleId);

            if (vehicle != null)
            {
                var statusDetail = Mapper.Map<VehicleStatusDetail>(dto);
                await _vehicleOverviewRepository.UpdateOverviewAsync(vehicle, statusDetail);

                await _vehicleOverviewRepository.CommitAsync();

                var vehicleOverview = await _vehicleOverviewRepository.FindAsync(vehicle.Id);// if we reach here then it is not null
                var clientOverView = Mapper.Map<OverviewClientDto>(vehicleOverview);
              
                await _overviewHub.Clients.All.SendAsync("OverviewReceived", clientOverView);
            }
        }
    }
}