using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repo;
        private readonly ILogger<DiscountService> _logger;

        public DiscountService(IDiscountRepository repo, ILogger<DiscountService> logger)
        {
            _repo = repo ?? throw new ArgumentException(nameof(repo));
            _logger = logger ?? throw new ArgumentException(nameof(repo));
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repo.GetDiscount(request.ProductName);
            if(coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName}"));
            }
            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }
    }
}
