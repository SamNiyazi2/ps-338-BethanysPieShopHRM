using System.Collections.Generic;
using System.Threading.Tasks;
using BethanysPieShopHRM.ComponentsLibrary.Map;
using BethanysPieShopHRM.UI.Services;
using BethanysPieShopHRM.Shared;
using Microsoft.AspNetCore.Components;

namespace BethanysPieShopHRM.UI.Pages
{
    public class EmployeeDetailBase : ComponentBase
    {
        [Inject]
        public IEmployeeDataService EmployeeDataService { get; set; }

        [Inject]
        public IJobCategoryDataService JobCategoryDataService { get; set; }

        [Parameter]
        // 08/22/2021 12:22 pm - SSN - [20210822-1222] - [001] - M04-06 - Demo: Enhancing the application's routing features
        // public string EmployeeId { get; set; }
        public int EmployeeId { get; set; }

        public List<Marker> MapMarkers { get; set; } = new List<Marker>();

        protected string JobCategory = string.Empty;

        public Employee Employee { get; set; } = new Employee();


        public MarkupString FeedbackMessage { get; set; }
        public bool ValidRecord { get; set; }

        protected override async Task OnInitializedAsync()
        {
            // Employee = await EmployeeDataService.GetEmployeeDetails(int.Parse(EmployeeId));

            // 08/23/2021 03:13 pm - SSN - [20210822-1222] - [020] - M04-06 - Demo: Enhancing the application's routing features
            // Employee = await EmployeeDataService.GetEmployeeDetails(EmployeeId);

            APIBag<Employee> result = await EmployeeDataService.GetEmployeeDetails(EmployeeId);

            ValidRecord = result.ModelRecord != null;

            if (result.ModelRecord == null)
            {
                FeedbackMessage = new MarkupString(result.FeedbackMessages.GetFeedbackMessagesAsHTML());
            }
            else
            {

                Employee = result.ModelRecord;

                MapMarkers = new List<Marker>
            {
                new Marker{Description = $"{Employee.FirstName} {Employee.LastName}",  ShowPopup = false, X = Employee.Longitude, Y = Employee.Latitude}
            };

                JobCategory = (await JobCategoryDataService.GetJobCategoryById(Employee.JobCategoryId)).JobCategoryName;

            }

        }
    }
}
