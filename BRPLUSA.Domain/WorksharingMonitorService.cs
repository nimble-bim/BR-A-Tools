using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BRPLUSA.Domain.Entities;
using BRPLUSA.Domain.Entities.Events;
using Newtonsoft.Json;

namespace BRPLUSA.Domain
{
    public class WorksharingMonitorService
    {
        private static readonly HttpClient client = new HttpClient();
        //private static readonly string _url = "http://localhost:5000/api/worksharing";
        private static readonly string _url = "http://localhost:7855/api/worksharing";
        //private static readonly string _url = "https://worksharing-server.herokuapp.com/api/worksharing";

        public static async Task<HttpResponseMessage> PostModelOpenedEvent(WorksharingEvent state)
        {
            //var user = new User {Name = "psmith@brplusa.com"};
            //var state = new UserOpenedModelEvent(user);
            var serialized = JsonConvert.SerializeObject(state);

            var content = new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(_url, content);
            return response;
        }

        public void PostModelClosedEvent()
        {
            
        }
    }
}
