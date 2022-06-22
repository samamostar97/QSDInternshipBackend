using AutoMapper;
using Bookinghut.Database;
using Bookinghut.Model;
using Bookinghut.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookinghut.Mapper
{
    public class BookinghutProfile : Profile
    {
        public BookinghutProfile()
        {
            CreateMap<Event, MEvent>().ReverseMap();
            CreateMap<Event, EventUpsertRequestdto>().ReverseMap();
            CreateMap<Event, EventSearchRequestdto>().ReverseMap();

            CreateMap<UserEvent, UserEventUpsertRequest>().ReverseMap();
            CreateMap<UserEvent, MUserEvent>().ReverseMap();

            CreateMap<User, MUser>().ReverseMap();
            CreateMap<User, UserUpsertRequestdto>().ReverseMap();
            CreateMap<User, RegisterRequest>().ReverseMap();

            CreateMap<Venue, MVenue>();
            CreateMap<Venue, VenueUpsertRequest>().ReverseMap();

            CreateMap<User, UserResponse>();

            CreateMap<User, AuthenticateResponse>();
            //CreateMap<CreateRequest, Account>();
        }
    }
}
