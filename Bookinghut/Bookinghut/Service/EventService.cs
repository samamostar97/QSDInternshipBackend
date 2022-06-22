using AutoMapper;
using Bookinghut.Database;
using Bookinghut.Model;
using Bookinghut.Model.Request;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookinghut.Service
{
    public interface IEventService
    {
        Task<List<MEvent>> Get(EventSearchRequestdto search);
        Task<MEvent> GetByID(int ID);
        Task<MEvent> Insert(EventUpsertRequestdto request);
        Task<MEvent> Update(int ID, EventUpsertRequestdto request);
        Task<bool> Delete(int ID);
    }
    public class EventService : IEventService
    {
        public BookinghutContext Context { get; set; }
        protected readonly IMapper _mapper;

        public EventService(BookinghutContext bookinghutContext, IMapper mapper)
        {
            Context = bookinghutContext;
            _mapper = mapper;
        }

        public async Task<List<MEvent>> Get(EventSearchRequestdto search)
        {
            var query = Context.Event.AsQueryable();

            if (search.EventID != 0)
            {
                query = query.Where(i => i.EventID == search.EventID);
            }

            var list = await query.ToListAsync();
            return _mapper.Map<List<MEvent>>(list);
        }

        public async Task<MEvent> GetByID(int ID)
        {
            var entity = await Context.Event.Where(i => i.EventID == ID).SingleOrDefaultAsync();
            return _mapper.Map<MEvent>(entity);
        }

        public async Task<MEvent> Insert(EventUpsertRequestdto request)
        {
            var entity = _mapper.Map<Event>(request);
            Context.Set<Event>().Add(entity);
            await Context.SaveChangesAsync();
            return _mapper.Map<MEvent>(entity);
        }

        public async Task<MEvent> Update(int ID, EventUpsertRequestdto request)
        {
            var entity = Context.Set<Event>().Find(ID);
            Context.Set<Event>().Attach(entity);
            Context.Set<Event>().Update(entity);

            _mapper.Map(request, entity);

            await Context.SaveChangesAsync();

            return _mapper.Map<MEvent>(entity);
        }

        public async Task<bool> Delete(int ID)
        {
            var user = await Context.Event.Where(i => i.EventID == ID).FirstOrDefaultAsync();
            if (user != null)
            {
                Context.Event.Remove(user);
                await Context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}