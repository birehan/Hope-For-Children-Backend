using AutoMapper;
using Application.Features.Staffs.DTOs;
using Domain;
using Application.Features.Accounts.DTOs;
using Application.Features.Alumnis.DTOs;
using Application.Features.Projects.DTOs;
using Application.Features.Categories.DTOs;

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

            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<Category, CategoryDetailDto>()
            .ReverseMap();

            //   .ForMember(
            //    dest => dest.Photos,
            //    opt => opt.MapFrom(src => src.Photos.Select(photo => photo.Url).ToList()))

            CreateMap<CreateCategoryDto, Category>()
                .ForMember(dest => dest.Photos, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<UpdateCategoryDto, Category>()
                .ForMember(dest => dest.Photos, opt => opt.Ignore())
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
