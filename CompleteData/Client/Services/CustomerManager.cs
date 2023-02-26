using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompleteData.Shared.Models;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Net;

namespace CompleteData.Client.Services
{
    public class CustomerManager : APIRepository<Customers>
    {
        HttpClient http;

        public CustomerManager(HttpClient _http) 
            : base(_http, "customers", "CustomerId")
        {
            http = _http;
        }

        public async Task<IEnumerable<Customers>> SearchByContactName(string ContactName)
        {
            try
            {
                var arg = WebUtility.HtmlEncode(ContactName);
                var url = $"customers/{arg}/searchbycontactname";
                var result = await http.GetAsync(url);
                result.EnsureSuccessStatusCode();
                string responseBody = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<APIListOfEntityResponse<Customers>>(responseBody);
                if (response.Success)
                    return response.Data;
                else
                    return new List<Customers>();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return null;
            }
        }

    
    }
}
