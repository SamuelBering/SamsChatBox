using System.Threading.Tasks;
using DotNetGigs.Models;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using System.IO;

namespace DotNetGigs.Services
{
    public class HttpClientService: IHttpClientService
    {

        private readonly string baseUrl;
        public HttpClientService(string baseUrl)
        {
            this.baseUrl=baseUrl;
        }
        
        public async Task<T> Get<T>(string url, DataContractJsonSerializerSettings settings = null)
        {
            using (HttpClient client = new HttpClient())
            {

                SetupRequest(client);
                HttpResponseMessage response =
                       await client.GetAsync(url);
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

        private void SetupRequest(HttpClient client)
        {
            client.BaseAddress = new Uri(this.baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new
            MediaTypeWithQualityHeaderValue("application/json"));
        }



    }
}