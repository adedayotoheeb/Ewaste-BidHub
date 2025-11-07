using ElectronicBidding.Application.Abstractions.Bids;
using ElectronicBidding.Application.Abstractions.Users;
using ElectronicBidding.Application.Implementation.Bids;
using ElectronicBidding.Application.Implementation.Listing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicBidding.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidService _bidService;
        private readonly ILogger<BidController> _logger;

        public BidController(IBidService bidService, ILogger<BidController> logger)
        {
            _bidService = bidService;
            _logger = logger;
        }

        [Authorize(Roles = "Buyer")]
        [HttpPost]
        public async Task<IActionResult> CreateBid(PlaceBidRequest request)
        {
            var result = await _bidService.PlaceBidAsync(request);
            return result.IsSuccess ? Created(nameof(CreateBid), result) : BadRequest(result);
        }
    }
}
