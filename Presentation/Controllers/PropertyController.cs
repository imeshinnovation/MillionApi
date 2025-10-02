using Microsoft.AspNetCore.Mvc;
using MillionApi.Application.Services;
using MillionApi.Domain.Entities;
using MillionApi.Dtos;

namespace MillionApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyController : ControllerBase
    {
        private readonly PropertyService _propertyService;

        public PropertyController(PropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProperties(
            [FromQuery] string? name,
            [FromQuery] string? address,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            var properties = await _propertyService.GetProperties(name, address, minPrice, maxPrice);
            return Ok(properties);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProperty([FromBody] PropertyDto dto)
        {
            var property = new Property
            {
                IdOwner = dto.IdOwner,
                Name = dto.Name,
                AddressProperty = dto.AddressProperty,
                PriceProperty = dto.PriceProperty,
                ImageUrl = dto.ImageUrl
            };

            var result = await _propertyService.CreateProperty(property);
            return CreatedAtAction(nameof(GetProperties), new { id = result.Id }, result);
        }
    }
}
