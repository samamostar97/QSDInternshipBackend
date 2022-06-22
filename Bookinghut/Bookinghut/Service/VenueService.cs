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
    public interface IVenueService
    {
        Task<List<MVenue>> Get(VenueSearchRequest search);
        Task<MVenue> GetById(int ID);
        Task<MVenue> Insert(VenueUpsertRequest request);
        Task<MVenue> Update(int ID, VenueUpsertRequest request);
        Task<bool> Delete(int ID);
    }


    public class VenueService:IVenueService
    {
        public BookinghutContext Context { get; set; }
        protected readonly IMapper _mapper;

        public VenueService(BookinghutContext bookinghutContext, IMapper mapper)
        {
            Context = bookinghutContext;
            _mapper = mapper;
        }


        public async Task<List<MVenue>> Get(VenueSearchRequest search)
        {

            var query = Context.Venue.AsQueryable();

            if (search.ID != 0)
            {
                query = query.Where(i => i.VenueID == search.ID);
            }


            var list = await query.ToListAsync();
            return _mapper.Map<List<MVenue>>(list);
        }

        public async Task<MVenue> GetById(int ID)
        {
            var entity = await Context.Venue
               .Where(i => i.VenueID == ID)
               .SingleOrDefaultAsync();

            return _mapper.Map<MVenue>(entity);
        }

        public async Task<MVenue> Insert(VenueUpsertRequest request)
        {
            var entity = _mapper.Map<Venue>(request);
            Context.Set<Venue>().Add(entity);
            await Context.SaveChangesAsync();

            return _mapper.Map<MVenue>(entity);
        }

        public async Task<MVenue> Update(int ID, VenueUpsertRequest request)
        {
            var entity = Context.Set<Venue>().Find(ID);
            Context.Set<Venue>().Attach(entity);
            Context.Set<Venue>().Update(entity);

            _mapper.Map(request, entity);

            await Context.SaveChangesAsync();

            return _mapper.Map<MVenue>(entity);
        }

        public async Task<bool> Delete(int ID)
        {
            var rate = await Context.Venue.Where(i => i.VenueID == ID).FirstOrDefaultAsync();
            if (rate != null)
            {
                Context.Venue.Remove(rate);
                await Context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}