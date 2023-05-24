using System;
using AutoMapper;
using Data;
using ViewModels.SharePlus;

namespace Infrastructure
{
    //public interface IValueResolver<in TSource, in TDestination, TDestMember>
    //{
    //    TDestMember Resolve(TSource source, TDestination destination, TDestMember destMember, ResolutionContext context);
    //}

    //public class CustomResolver : IValueResolver<Employee, SharePlusArticleViewModel, int>
    //{
    //    public int Resolve(Employee source, SharePlusArticleViewModel destination, int member, ResolutionContext context)
    //    {
    //        return destination.Employee.Id;
    //    }
    //}

    //public class SharePlusProfile : Profile
    //{
    //    public SharePlusProfile()
    //    {
    //        CreateMap<SharePlusArticle, SharePlusArticleViewModel>()
    //            .ForMember(s => s.CreatedByEmployeeId, opt => opt.MapFrom(src => src.CreatedByEmpId))
    //            .ForMember(s => s.CreatedByEmployeeFirstName, opt => opt.MapFrom(src => src.CreatedByEmp.FirstName))
    //            .ForMember(s => s.CreatedByEmployeeSurname, opt => opt.MapFrom(src => src.CreatedByEmp.Surname))
    //            .ForMember(s => s.CreatedByEmployeeImageBase64, opt => opt.MapFrom(src => src.CreatedByEmp.ImageBase64));



    //        //CreateMap<Employee, EmployeeArticleViewModel>()
    //        //    .ForMember(s => s.Id, opt => opt.MapFrom(src => src.Id))
    //        //    .ForMember(s => s.FirstName, opt => opt.MapFrom(src => src.FirstName))
    //        //    .ForMember(s => s.Surname, opt => opt.MapFrom(src => src.Surname))
    //        //    .ForMember(s => s.ImageBase64, opt => opt.MapFrom(src => src.ImageBase64));
    //    }
    //}
}
