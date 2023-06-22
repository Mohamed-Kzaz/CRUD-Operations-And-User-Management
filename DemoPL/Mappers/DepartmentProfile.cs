using AutoMapper;
using Demo.DAL.Entities;
using DemoPL.Models;

namespace DemoPL.Mappers
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            //Department => DepartmentViewModel
            //DepartmentViewModel => Department
            CreateMap<Department, DepartmentViewModel>().ReverseMap();
        }
    }
}
