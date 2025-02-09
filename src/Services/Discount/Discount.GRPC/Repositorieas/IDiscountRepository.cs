﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discount.GRPC.Entities;

namespace Discount.GRPC.Repositorieas
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productName);
        Task<bool> CreateDiscount(Coupon coupon);
        Task<bool> UpdateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productName);
    }
}
