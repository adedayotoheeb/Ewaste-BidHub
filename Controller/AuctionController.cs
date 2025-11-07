using ElectronicBidding.Application.Abstractions.Auction;
using ElectronicBidding.Application.Abstractions.Bids;
using ElectronicBidding.Application.Implementation.Auction;
using ElectronicBidding.Application.Implementation.Bids;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicBidding.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _auctionService;
        private readonly ILogger<AuctionController> _logger;

        public AuctionController(IAuctionService auctionService, ILogger<AuctionController> logger)
        {
            _auctionService = auctionService;
            _logger = logger;
        }

        [Authorize(Roles = "Seller, Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAution(CreateAuctionRequest request)
        {
            var result = await _auctionService.CreateAuctionAsync(request);
            return result.IsSuccess ? Created(nameof(CreateAution), result) : BadRequest(result);
        }

       
    }
}
