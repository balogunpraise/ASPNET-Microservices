using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repo;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;

        public DiscountService(IDiscountRepository repo, ILogger<DiscountService> logger, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentException(nameof(repo));
            _logger = logger ?? throw new ArgumentException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repo.GetDiscount(request.ProductName);
            if(coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName}"));
            }
            _logger.LogInformation("Discount is retrieved for ProductName: {productName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);
            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            await _repo.CreateDiscount(coupon);
            _logger.LogInformation("Discount is successfully created. ProductName: {ProductName}", coupon.ProductName);
            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            await _repo.UpdateDiscount(coupon);
            _logger.LogInformation("Discount is successfully updated. ProductName: {ProductName}", coupon.ProductName);
            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _repo.DeleteDiscount(request.ProductName);
            var response = new DeleteDiscountResponse
            {
                Success = deleted
            };
            return response;
        }
    }
}
