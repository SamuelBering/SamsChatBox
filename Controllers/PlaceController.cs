
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DotNetGigs.Models;
using DotNetGigs.Services;
using DotNetGigs.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DotNetGigs.Controllers
{
    [Route("api/[controller]")]
    public class PlaceController : Controller
    {

        private readonly IMapper _mapper;
        private IPlaceService _placeService;
        private readonly JsonSerializerSettings _serializerSettings;

        public PlaceController(IPlaceService placeService, IMapper mapper)
        {
            _placeService = placeService;
            _mapper = mapper;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        // GET api/place/getplaces
        [HttpGet("getplaces")]
        public async Task<IActionResult> Get(PlaceFilter model)
        {
            var places = await _placeService.GetPlaces(model);

            var placesVM = _mapper.Map<List<PlaceViewModel>>(places);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new
            {
                Result = "Test"
            });
        }

    }
}