using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using DotNetGigs.Models;

namespace DotNetGigs.Services
{
    public interface IPlaceService
    {
        Task<IList<Place>> GetPlaces(PlaceFilter filter);
    }
}