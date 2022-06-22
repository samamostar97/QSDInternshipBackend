using AutoMapper;
using Bookinghut.Database;
using Bookinghut.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookinghut.Service
{
    public interface IRoleService
    {
        Task<MRole> GetById(int ID);

    }

        public class RoleService:IRoleService
    {
        public BookinghutContext Context { get; set; }
        protected readonly IMapper _mapper;

        public RoleService(BookinghutContext bookinghutContext, IMapper mapper)
        {
            Context = bookinghutContext;
            _mapper = mapper;
        }


     

        public async Task<MRole> GetById(int ID)
        {
            var entity = await Context.Venue
               .Where(i => i.VenueID == ID)
               .SingleOrDefaultAsync();

            return _mapper.Map<MRole>(entity);
        }

    }
}
