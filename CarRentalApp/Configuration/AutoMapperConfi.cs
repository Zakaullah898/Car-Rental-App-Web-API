using AutoMapper;
using CarRentalApp.Data;
using CarRentalApp.models;
using CarRentalApp.Service;

namespace CarRentalApp.Configuration
{
    public class AutoMapperConfi:Profile
    {
        public AutoMapperConfi()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Car, CarDTO>().ReverseMap();
            CreateMap<OTPVerification, OtpVerificationDTO>().ReverseMap();
            CreateMap<UserProfileCreateDTO, UserProfile>().ReverseMap();
            CreateMap<UserProfile, UserProfileDTO>().ReverseMap();
            CreateMap<FavoriteCars, FavouriteDTO>().ReverseMap();
            CreateMap<Car, FavouriteCarsDTO>().ForMember(dest => dest.IsFavorite, opt => opt.MapFrom(src => true)).ReverseMap();
        }
    }
}
