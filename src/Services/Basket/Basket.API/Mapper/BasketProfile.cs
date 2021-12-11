using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Basket.API.Entities;
using EventBus.Messages.Events;

namespace Basket.API.Mapper
{
    public class BasketProfile:Profile
    {
        public BasketProfile()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
