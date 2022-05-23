using abcclicktrans.Data.Models;
using abcclicktrans.ViewModels;
using AutoMapper;
using Microsoft.CodeAnalysis.Scripting;

namespace abcclicktrans.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TransportOrderViewModel, TransportOrder>()
                .ForMember(x=>x.Height,opt=>opt.MapFrom(src=>src.ParcelSize.Height))
                .ForMember(x => x.Length, opt => opt.MapFrom(src => src.ParcelSize.Length))
                .ForMember(x => x.Width, opt => opt.MapFrom(src => src.ParcelSize.Width))
                .ForMember(x => x.Weight, opt => opt.MapFrom(src => src.ParcelSize.Weight))
                ;
            CreateMap<TransportOrder, TransportOrderViewModel>()
                .ForMember(x => x.Image, opt => opt.Ignore());

            CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(x=>x.SubscriptionDateTime,opt=>opt.MapFrom(src=>src.Subscription.ExpirationDateTime))
                .ForMember(x => x.SubscriptionIdGuid, opt => opt.MapFrom(src => src.Subscription.Id))
                .ReverseMap();
        }
    }
}
