using AutoMapper;
using FluentStoredProcedureExtensions.Core.Entities;
using FluentStoredProcedureExtensions.WebUI.ViewModels;

namespace FluentStoredProcedureExtensions.WebUI.MapperProfiles
{
    public class MainProfile : Profile
    {
        public MainProfile()
        {
            CreateMap<Employee, EmployeeForManipulationViewModel>();
            CreateMap<EmployeeForManipulationViewModel, Employee>();
            CreateMap<Employee, EmployeeViewModel>();
        }
    }
}
