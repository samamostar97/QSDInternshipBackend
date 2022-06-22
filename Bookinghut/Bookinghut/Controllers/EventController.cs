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
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }
        [HttpGet]
        public async Task<List<MEvent>> Get([FromQuery] EventSearchRequestdto search)
        {
            return await _eventService.Get(search);
        }
        [HttpGet("{ID}")]
        public async Task<MEvent> GetById(int ID)
        {
            return await _eventService.GetByID(ID);
        }
        [HttpPost]
        public async Task<MEvent> Insert(EventUpsertRequestdto request)
        {
            return await _eventService.Insert(request);
        }
        [HttpPut("{ID}")]
        public async Task<MEvent> Update(int ID, EventUpsertRequestdto request)
        {
            return await _eventService.Update(ID, request);
        }
        [HttpDelete("{ID}")]
        public async Task<bool> Delete(int ID)
        {
            return await _eventService.Delete(ID);
        }
    }
}
