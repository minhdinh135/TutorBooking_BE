using AutoMapper;
using PRN231.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {

            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<Credential, CredentialDTO>();
            CreateMap<CredentialDTO, Credential>();
        public MappingProfile() 
        {
            //Account
            CreateMap<TutorRequestDTO, User>().ReverseMap();
            CreateMap<TutorRequestDTO, Subject>().ReverseMap();
            CreateMap<TutorRequestDTO, Schedule>().ReverseMap();
        }
    }
}
