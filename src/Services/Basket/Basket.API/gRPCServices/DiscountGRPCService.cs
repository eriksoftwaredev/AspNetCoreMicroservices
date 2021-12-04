using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discount.GRPC.Protos;

namespace Basket.API.gRPCServices
{
    public class DiscountGRPCService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient;

        public DiscountGRPCService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        {
            this.discountProtoServiceClient = discountProtoServiceClient ?? throw new ArgumentNullException(nameof(discountProtoServiceClient));
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var getDiscountRequest = new GetDiscountRequest();
            getDiscountRequest.ProductName = productName;
            var result = await discountProtoServiceClient.GetDiscountAsync(getDiscountRequest);

            return result;
        }
    }
}
