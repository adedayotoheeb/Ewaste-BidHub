using ElectronicBidding.Application.Abstractions.Listings;
using ElectronicBidding.Application.Abstractions.Users;
using ElectronicBidding.Application.Implementation.Listing;
using ElectronicBidding.Application.Implementation.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicBidding.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private readonly IListingService _listService;
        private readonly ILogger<ListingController> _logger;

        public ListingController(IListingService listService, ILogger<ListingController> logger)
        {
            _listService = listService;
            _logger = logger;
        }

        [Route("get-listing")]
        [HttpGet]
        public async Task<IActionResult> GetListing([FromQuery] ListingQueryParameters request)
        {
            var result = await _listService.GetListings(request);
            return result.IsSuccess ? Ok( result) : BadRequest(result);
        }
        
        [Route("get-my-listing")]
        [HttpGet]
        public async Task<IActionResult> GetCurrentUserListing()
        {
            var result = await _listService.GetMyListings();
            return Ok(result);
        }

        [Authorize(Roles = "Seller")]
        [HttpPost]
        public async Task<IActionResult> CreateListing(CreateListingRequest request)
        {
            var result = await _listService.CreateListing(request);
            return result.IsSuccess ? Created(nameof(CreateListing),result) : BadRequest(result);
        } 

        [Route("update-listing")]
        [HttpPatch]
        public async Task<IActionResult> UpdateListing(Guid id, UpdateListingRequest request)
        {
            var result = await _listService.UpdateListing(id, request);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
