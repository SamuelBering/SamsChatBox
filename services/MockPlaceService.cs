using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using System.IO;
using DotNetGigs.Extensions;
using System.Linq;
using DotNetGigs.Models;
using System.Text;

namespace DotNetGigs.Services
{
    public class MockPlaceService : IPlaceService
    {
        private List<Place> Places { get; }
        public MockPlaceService(string placesJson)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(NearbySearchResult));
            byte[] byteArray = Encoding.ASCII.GetBytes(placesJson);
            MemoryStream stream = new MemoryStream(byteArray);
            NearbySearchResult nearbySearchResult = (NearbySearchResult)serializer.ReadObject(stream);
            this.Places = nearbySearchResult.results;
        }

        public async Task<IList<Place>> GetPlaces(PlaceFilter filter)
        { 
             return filter.Keyword !=null ? this.Places.Where(p=>p.name.ToLower().Contains(filter.Keyword.ToLower())).ToList(): this.Places;
        }

    }
}