using AutoMapper;
using Demo.DAL.Entities;
using DemoPL.Models;

namespace DemoPL.Mappers
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeViewModel>().ReverseMap();

        }
    }
}
