using AutoMapper;
using Application.Features.Staffs.DTOs;
using Domain;
using Application.Features.Accounts.DTOs;
using Application.Features.Alumnis.DTOs;
using Application.Features.Projects.DTOs;
using Application.Features.SubCategories.DTOs;

namespace Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<CreateStaffDto, Staff>().ReverseMap();
            CreateMap<UpdateStaffDto, Staff>().ReverseMap();
            CreateMap<Staff, StaffDto>()
            .ForMember(x => x.PhotoUrl, o => o.MapFrom(s => s.Photo.Url));

            CreateMap<CreateSubCategoryDto, SubCategory>()
                .ForMember(dest => dest.Photos, opt => opt.Ignore())
                .ForMember(dest => dest.MainPhoto, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<SubCategory, SubCategoryDto>()
                .ForMember(dest => dest.MainPhoto, opt => opt.MapFrom(src => src.MainPhoto.Url))
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos.Select(p => p.Url).ToList()));

            CreateMap<UpdateSubCategoryDto, SubCategory>()
                .ForMember(dest => dest.Photos, opt => opt.Ignore())
                .ForMember(dest => dest.MainPhoto, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<CreateAlumniDto, Alumni>().ReverseMap();
            CreateMap<UpdateAlumniDto, Alumni>().ReverseMap();
            CreateMap<Alumni, AlumniDto>()
                .ForMember(x => x.PhotoUrl, o => o.MapFrom(s => s.Photo.Url));


            CreateMap<CreateProjectDto, Project>().ReverseMap();
            CreateMap<UpdateProjectDto, Project>().ReverseMap();
            CreateMap<Project, ProjectDto>()
                .ForMember(x => x.PhotoUrl, o => o.MapFrom(s => s.Photo.Url));



            CreateMap<UserAccountDto, AppUser>().ReverseMap();

        }
    }
}
