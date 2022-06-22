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
    public class UserEventController : ControllerBase
    {
        private readonly IUserEventService _userEventService;
        
        public UserEventController(IUserEventService userEventService)
        {
            _userEventService = userEventService;
        }

        [HttpPost]
        public async Task<MUserEvent> Insert(UserEventUpsertRequest request)
        {
            return await _userEventService.Insert(request);
        }
       
    }
}
