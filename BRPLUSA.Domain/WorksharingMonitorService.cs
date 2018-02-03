using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BRPLUSA.Domain.Entities;
using Newtonsoft.Json;

namespace BRPLUSA.Domain
{
    public class WorksharingMonitorService
    {
        private static readonly HttpClient client = new HttpClient();
        private const string _url = "http://localhost:7855/api/worksharing";

        public async void PostModelOpenedEvent()
        {
            var user = new User {Name = "psmith@brplusa.com"};
            var state = new UserOpenedModelEvent(user);
            var serialized = JsonConvert.SerializeObject(state);

            var content = new StringContent(serialized, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(_url, content);
        }

        public void PostModelClosedEvent()
        {
            
        }

    }
}
