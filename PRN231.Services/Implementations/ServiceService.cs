using AutoMapper;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Repository.Interfaces;
using PRN231.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN231.Services.Implementations
{
    public class ServiceService : GenericService<Service, ServiceDTO>, IServiceService
    {
        public ServiceService(IMapper mapper, IGenericRepository<Service> genericRepository) : base(mapper, genericRepository)
        {
        }
    }
}
