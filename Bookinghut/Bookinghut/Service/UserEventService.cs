using AutoMapper;
using Bookinghut.Database;
using Bookinghut.Model;
using Bookinghut.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookinghut.Service
{
    public interface IUserEventService
    {
        Task<MUserEvent> Insert(UserEventUpsertRequest request);

    }
    public class UserEventService : IUserEventService
    {
        public BookinghutContext _context { get; set; }
        protected readonly IMapper _mapper;

        public UserEventService(BookinghutContext bookinghutContext, IMapper mapper)
        {
            _context = bookinghutContext;
            _mapper = mapper;
        }
        public async Task<MUserEvent> Insert(UserEventUpsertRequest request)
        {
            var entity = _mapper.Map<UserEvent>(request);
            _context.Set<UserEvent>().Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<MUserEvent>(entity);
        }
    }
}
