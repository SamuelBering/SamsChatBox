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

namespace DotNetGigs.Services
{
    public class PlaceService : IPlaceService
    {

        private readonly string apiKey;
        private readonly string baseUrl;
        //      baseUrl: string = 'https://maps.googleapis.com/maps/api/place/nearbysearch/json';
        // apiKey: string = 'AIzaSyCxKlIFM4NyNdg6o6SJV6_lW6ryG0m019A';
        public PlaceService(string baseUrl, string apiKey)
        {
            this.baseUrl = baseUrl;
            this.apiKey = apiKey;
        }

        public async Task<IList<Place>> GetPlaces(PlaceFilter filter)
        {
            using (HttpClient client = new HttpClient())
            {

                SetupRequest(client);
                //`${this.baseUrl}?location=-33.8670522,151.1957362&radius=500&types=food&name=harbour&key=${this.apiKey}`
                NearbySearchResult nearbySearchResult = await client.GetAsync<NearbySearchResult>($"?location=-33.8670522,151.1957362&radius=500&types=food&name=harbour&key={this.apiKey}");

                return nearbySearchResult.results;

                //     return new List<Place>{
                //        new Place{ 
                //            name="Samuels plats"
                //        }
                //    };
            }
        }

        private void SetupRequest(HttpClient client)
        {
            client.BaseAddress = new Uri(this.baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new
            MediaTypeWithQualityHeaderValue("application/json"));
        }



    }
}