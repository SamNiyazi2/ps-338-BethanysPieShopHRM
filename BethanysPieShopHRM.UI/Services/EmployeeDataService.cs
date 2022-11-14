using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BethanysPieShopHRM.Shared;
using Microsoft.AspNetCore.Components;

namespace BethanysPieShopHRM.UI.Services
{
    public class EmployeeDataService : IEmployeeDataService
    {
        private readonly HttpClient _httpClient;

        public Employee SavedEmployee { get; set; }

        public EmployeeDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Employee>>
                (await _httpClient.GetStreamAsync($"api/employee"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }


        // 08/23/2021 03:14 pm - SSN - [20210822-1222] - [021] - M04-06 - Demo: Enhancing the application's routing features
        // public async Task<Employee> GetEmployeeDetails(int employeeId)
        public async Task<APIBag<Employee>> GetEmployeeDetails(int employeeId)
        {

            //return await JsonSerializer.DeserializeAsync<Employee>
            //    (await _httpClient.GetStreamAsync($"api/employee/{employeeId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            APIBag<Employee> result = new APIBag<Employee>();

            try
            {
                // System.IO.Stream ioStream = await _httpClient.GetStreamAsync($"api/employee/{employeeId}");
                var response = await _httpClient.GetStreamAsync($"api/employee/{employeeId}");

                result.parseRespose(response);
            }
            catch (Exception ex)
            {
                string message = ex.Message;

                result.FeedbackMessages.Add_2(message, true);
                result.FeedbackMessages.Add_2("Additional messages (good)", false);
                result.FeedbackMessages.Add_2("Additional messages (bad)", true);
            }

            return result;



        }
 
        public async Task<Employee> AddEmployee(Employee employee)
        {
            var employeeJson =
                new StringContent(JsonSerializer.Serialize(employee), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/employee", employeeJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Employee>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }


        public async Task<APIBag<Employee>> UpdateEmployee(Employee employee)
        {
            var employeeJson = new StringContent(JsonSerializer.Serialize(employee), Encoding.UTF8, "application/json");

            // 08/22/2021 02:00 pm - SSN - [20210822-1222] - [012] - M04-06 - Demo: Enhancing the application's routing features
            HttpResponseMessage response = await _httpClient.PutAsync("api/employee", employeeJson);

            APIBag<Employee> result = new APIBag<Employee>();

            // if (response.IsSuccessStatusCode)
            {
                try
                {
                    //return await JsonSerializer.DeserializeAsync<Employee>(await response.Content.ReadAsStreamAsync());

                    result.ModelRecord = await JsonSerializer.DeserializeAsync<Employee>(await response.Content.ReadAsStreamAsync());
                }
                catch (System.Exception ex)
                {
                    string message = ex.Message;
                }

                try
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    {
                        result.FeedbackMessages = JsonSerializer.Deserialize<List<FeedbackMessage>>(await response.Content.ReadAsStringAsync());
                    }
                }
                catch (System.Exception ex)
                {
                    string message = ex.Message;
                    throw;
                }
            }

            return result;

        }

        public async Task DeleteEmployee(int employeeId)
        {
            await _httpClient.DeleteAsync($"api/employee/{employeeId}");
        }
    }
}
