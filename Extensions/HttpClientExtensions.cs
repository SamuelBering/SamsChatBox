using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DotNetGigs.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetAsync<T>(this HttpClient client, string url, DataContractJsonSerializerSettings settings = null)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            DataContractJsonSerializer serializer;

            if (settings != null)
                serializer = new DataContractJsonSerializer(typeof(T), settings);
            else
                serializer = new DataContractJsonSerializer(typeof(T));

            Stream responseStream = await response.Content.ReadAsStreamAsync();
            T data = (T)serializer.ReadObject(responseStream);
            var answer = await response.Content.ReadAsStringAsync();
            return data;
        }
        
    }
}