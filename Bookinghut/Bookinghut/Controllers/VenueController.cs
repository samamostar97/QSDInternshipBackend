using Bookinghut.Model;
using Bookinghut.Model.Request;
using Bookinghut.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookinghut.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenueController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenueController(IVenueService venueService)
        {
            _venueService = venueService;
        }
        [HttpGet]
        public async Task<List<MVenue>> Get([FromQuery] VenueSearchRequest search)
        {
            return await _venueService.Get(search);
        }
        [HttpGet("{ID}")]
        public async Task<MVenue> GetById(int ID)
        {
            return await _venueService.GetById(ID);
        }
        [HttpPost]
        public async Task<MVenue> Insert(VenueUpsertRequest request)
        {
            return await _venueService.Insert(request);
        }
        [HttpPut("{ID}")]
        public async Task<MVenue> Update(int ID, VenueUpsertRequest request)
        {
            return await _venueService.Update(ID, request);
        }
        [HttpDelete("{ID}")]
        public async Task<bool> Delete(int ID)
        {
            return await _venueService.Delete(ID);
        }
    }
}
