using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Discount.GRPC.Entities;
using Discount.GRPC.Protos;

namespace Discount.GRPC.Mapper
{
    public class DiscounProfile : Profile
    {
        public DiscounProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
