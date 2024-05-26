using AutoMapper;
using PRN231.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Models.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {

            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
        }
    }
}
