using System;
using System.Threading.Tasks;
using AutoMapper;
using Avt.Web.Backend.Data.Entities;
using Avt.Web.Backend.Data.Repositories;
using Avt.Web.Backend.Data.Types;
using Avt.Web.Backend.DTO;
using Microsoft.AspNetCore.SignalR;

namespace Avt.Web.Backend.Service
{
    public class PingService
    {
        private readonly IHubContext<PingHub> _pingHub;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehicleStatusRepository _vehicleStatusRepository;

        public PingService(IHubContext<PingHub> pingHub,
                           IVehicleRepository vehicleRepository,
                           IVehicleStatusRepository vehicleStatusRepository)
        {
            _pingHub = pingHub;
            _vehicleRepository = vehicleRepository;
            _vehicleStatusRepository = vehicleStatusRepository;
        }

        public async Task SendVehicleStatusAsync(VehicleStatusDto dto)
        {
            var vehicle = await _vehicleRepository.GetWithOwnerAsync(dto.VehicleId);

            if (vehicle != null)
            {
                var statusDetail = Mapper.Map<VehicleStatusDetail>(dto);
                statusDetail.Vehicle = null;
                int increment = -1;
                while (true)
                {
                    try
                    {
                        statusDetail.Id = (await _vehicleStatusRepository.FindIdAsync()) + ++increment;
                        await _vehicleStatusRepository.InsertAsync(statusDetail);

                        await _vehicleStatusRepository.CommitAsync();
                        break;
                    }
                    catch
                    {
                        _vehicleStatusRepository.ClearChangeTracker();
                    } // very very carzy!!! sqllite is very crazy!!
                }
                statusDetail.Vehicle = vehicle;
                var status = Mapper.Map<StatusClientDto>(statusDetail);
                await _pingHub.Clients.All.SendAsync("statusRecieved", status);
            }
        }
    }
}
