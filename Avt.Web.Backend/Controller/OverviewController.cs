using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Avt.Web.Backend.Data.Repositories;
using Avt.Web.Backend.DTO;
using Avt.Web.Backend.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Avt.Web.Backend.Controller
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class OverviewController : ControllerBase
    {
        private readonly IVehicleOverviewRepository _overviewRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IHubContext<StatusOverviewHub> _hub;
        public OverviewController(IVehicleOverviewRepository overviewRepository, IHubContext<StatusOverviewHub> hub, IOwnerRepository ownerRepository)
        {
            _overviewRepository = overviewRepository;
            _hub = hub;
            _ownerRepository = ownerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var overviewItems = await _overviewRepository.GetAllAsync();
                var result = Mapper.Map<List<OverviewClientDto>>(overviewItems);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetFiletersAsync()
        {
            try
            {
                var result = new FilterItemsDto()
                {
                    Owners = (await _ownerRepository.GetAllAsync())
                        .Select(t => new Pair<string, string>() {Key = t.Id, Value = t.Id}).ToList(),
                    AllStatus = (new[] { new Pair<int, string>(0, "Disconnected"), new Pair<int, string>(1, "Connected") }).ToList()
                };
            return Ok(result);
        }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError);
    }
}

    }
}
