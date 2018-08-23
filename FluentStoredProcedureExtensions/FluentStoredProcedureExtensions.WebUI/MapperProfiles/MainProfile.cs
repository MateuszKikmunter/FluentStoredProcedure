using AutoMapper;
using FluentStoredProcedureExtensions.Core.Entities;
using FluentStoredProcedureExtensions.WebUI.ViewModels;

namespace FluentStoredProcedureExtensions.WebUI.MapperProfiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<Employee, EditEmployeeViewModel>();
            CreateMap<EditEmployeeViewModel, Employee>();

            CreateMap<Employee, CreateEmployeeViewModel>();
            CreateMap<CreateEmployeeViewModel, Employee>();

            CreateMap<Employee, EmployeeViewModel>();
        }
    }
}
