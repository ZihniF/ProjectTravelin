using AutoMapper;
using ProjectTravelin.Dtos.BookingDtos;
using ProjectTravelin.Dtos.CategoryDtos;
using ProjectTravelin.Dtos.CommentDtos;
using ProjectTravelin.Dtos.TourDtos;
using ProjectTravelin.Dtos.TourProgramDtos;
using ProjectTravelin.Entities;

namespace ProjectTravelin.Mapping
{
    public class GeneralMapping:Profile
    {
        public GeneralMapping()
        {
            CreateMap<Category,CreateCategoryDto>().ReverseMap();
            CreateMap<Category,ResultCategoryDto>().ReverseMap();
            CreateMap<Category,UpdateCategoryDto>().ReverseMap();
            CreateMap<Category,GetCategoryByIdDto>().ReverseMap();

            CreateMap<Tour, CreateTourDto>().ReverseMap();
            CreateMap<Tour, ResultTourDto>().ReverseMap();
            CreateMap<Tour, UpdateTourDto>().ReverseMap();
            CreateMap<Tour, GetTourByIdDto>().ReverseMap();


            CreateMap<Comment, CreateCommentDto>().ReverseMap();
            CreateMap<Comment, ResultCommentDto>().ReverseMap();
            CreateMap<Comment, UpdateCommentDto>().ReverseMap();
            CreateMap<Comment, GetCommentByIdDto>().ReverseMap();
            CreateMap<Comment, ResultCommentListByTourIdDto>().ReverseMap();

            CreateMap<TourProgram, CreateTourProgramDto>().ReverseMap();
            CreateMap<TourProgram, ResultTourProgramDto>().ReverseMap();
            CreateMap<TourProgram, UpdateTourProgramDto>().ReverseMap();
            CreateMap<TourProgram, GetTourProgramByIdDto>().ReverseMap();

            CreateMap<Booking, CreateBookingDto>().ReverseMap();
            CreateMap<Booking, ResultBookingDto>().ReverseMap();
            CreateMap<Booking, GetBookingByIdDto>().ReverseMap();
            CreateMap<Booking, UpdateBookingDto>().ReverseMap();


        }
    }
}
