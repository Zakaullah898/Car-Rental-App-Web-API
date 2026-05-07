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
            // Booking Mappings and some configurations
            CreateMap<CreateBookingRequest, Booking>()
            .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.Rental!.CarId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Billing!.UserId))
            .ForMember(dest => dest.PickupLocationId, opt => opt.MapFrom(src => src.Rental!.PickupLocationId))
            .ForMember(dest => dest.DropoffLocationId, opt => opt.MapFrom(src => src.Rental!.DropoffLocationId))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Rental!.PickupDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Rental!.DropoffDate))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Billing!.Name))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Billing!.Phone))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Billing!.Address))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Billing!.City))
            .ForMember(dest => dest.Payment, opt => opt.Ignore())
            .ReverseMap();
            // Mapping Payment
            CreateMap<PaymentInfoDto, Payment>()
                .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.TransactionId, opt => opt.Ignore())
                .ForMember(dest => dest.Amount, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentStatus, opt => opt.Ignore());

            // Mapping for Location
            CreateMap<LocationDTO, Location>().ReverseMap();
        }
    }
}
