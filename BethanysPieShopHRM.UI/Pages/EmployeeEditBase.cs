using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BethanysPieShopHRM.UI.Services;
using BethanysPieShopHRM.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BethanysPieShopHRM.UI.Pages
{
    public class EmployeeEditBase : ComponentBase
    {
        [Inject]
        public IEmployeeDataService EmployeeDataService { get; set; }

        [Inject]
        public ICountryDataService CountryDataService { get; set; }

        [Inject]
        public IJobCategoryDataService JobCategoryDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IEmailService emailService { get; set; }

        [Parameter]
        // 08/22/2021 12:42 pm - SSN - [20210822-1222] - [006] - M04-06 - Demo: Enhancing the application's routing features
        // public string EmployeeId { get; set; }
        public int EmployeeId { get; set; }

        public InputText LastNameInputText { get; set; }

        public Employee Employee { get; set; } = new Employee();

        //needed to bind to select to value
        protected string CountryId = string.Empty;
        protected string JobCategoryId = string.Empty;

        //used to store state of screen
        // 08/23/2021 05:36 pm - SSN - [20210822-1222] - [028] - M04-06 - Demo: Enhancing the application's routing features
        // protected string Message = string.Empty;
        protected MarkupString Message = new MarkupString();

        protected string StatusClass = string.Empty;
        protected bool Saved;


        // 08/23/2021 05:31 pm - SSN - [20210822-1222] - [027] - M04-06 - Demo: Enhancing the application's routing features
        protected bool Failed;

        public List<Country> Countries { get; set; } = new List<Country>();
        public List<JobCategory> JobCategories { get; set; } = new List<JobCategory>();


        // 08/23/2021 12:56 pm - SSN - [20210822-1222] - [019] - M04-06 - Demo: Enhancing the application's routing features
        public MarkupString FeedbackMessages { get; set; }



        protected override async Task OnInitializedAsync()
        {
            Saved = false;
            Failed = false;

            Countries = (await CountryDataService.GetAllCountries()).ToList();
            JobCategories = (await JobCategoryDataService.GetAllJobCategories()).ToList();

            // 08/22/2021 12:43 pm - SSN - [20210822-1222] - [007] - M04-06 - Demo: Enhancing the application's routing features
            // int.TryParse(EmployeeId, out var employeeId);
            int employeeId = EmployeeId;

            if (EmployeeDataService.SavedEmployee != null)
            {
                Employee = EmployeeDataService.SavedEmployee;
            }
            else if (employeeId == 0) //new employee is being created
            {
                //add some defaults
                Employee = new Employee { CountryId = 1, JobCategoryId = 1, BirthDate = DateTime.Now, JoinedDate = DateTime.Now };
            }
            else
            {
                // 08/22/2021 12:43 pm - SSN - [20210822-1222] - [007] - M04-06 - Demo: Enhancing the application's routing features
                // Employee = await EmployeeDataService.GetEmployeeDetails(int.Parse(EmployeeId));

                // 08/23/2021 03:35 pm - SSN - [20210822-1222] - [024] - M04-06 - Demo: Enhancing the application's routing features
                //Employee = await EmployeeDataService.GetEmployeeDetails(EmployeeId);
                APIBag<Employee> result = await EmployeeDataService.GetEmployeeDetails(EmployeeId);
                if (result.FeedbackMessages.Count == 0)
                {
                    Employee = result.ModelRecord;

                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = new MarkupString(result.FeedbackMessages.GetFeedbackMessagesAsHTML());

                    Failed = true;
                }

            }

            CountryId = Employee.CountryId.ToString();
            JobCategoryId = Employee.JobCategoryId.ToString();
        }

        protected async Task HandleValidSubmit()
        {
            // 08/24/2021 06:45 am - SSN - [20210822-1222] - [029] - M04-06 - Demo: Enhancing the application's routing features
            // Add try/catch
            try
            {

                // 08/24/2021 07:18 am - SSN - [20210822-1222] - [033] - M04-06 - Demo: Enhancing the application's routing features
                //Employee.JobCategoryId = int.Parse(JobCategoryId);
                //Employee.CountryId = int.Parse(CountryId);

                int.TryParse(JobCategoryId, out int _jobCategoryId);
                int.TryParse(CountryId, out int _countryId);

                Employee.JobCategoryId = _jobCategoryId;
                Employee.CountryId = _countryId;


                if (Employee.EmployeeId == 0) //new
                {
                    var addedEmployee = await EmployeeDataService.AddEmployee(Employee);

                    emailService.SendEmail();

                    if (addedEmployee != null)
                    {
                        StatusClass = "alert-success";
                        Message = new MarkupString("New employee added successfully.");
                        Saved = true;
                    }
                    else
                    {
                        StatusClass = "alert-danger";
                        Message = new MarkupString("Something went wrong adding the new employee. Please try again.");
                        Saved = false;
                    }
                }
                else
                {

                    // 08/22/2021 02:05 pm - SSN - [20210822-1222] - [013] - M04-06 - Demo: Enhancing the application's routing features

                    APIBag<Employee> result = await EmployeeDataService.UpdateEmployee(Employee);

                    if (result.FeedbackMessages.Count == 0)
                    {

                        Employee = result.ModelRecord;

                        StatusClass = "alert-success";
                        Message = new MarkupString("Employee updated successfully.");
                        Saved = true;

                    }
                    else
                    {
                        FeedbackMessages = new MarkupString(result.FeedbackMessages.GetFeedbackMessagesAsHTML());

                        Saved = false;
                    }

                }
            }
            catch (Exception ex)
            {

                StatusClass = "alert-danger";
                FeedbackMessages = new MarkupString("Something went wrong. Please try again.");
                Saved = false;
            }
        }



        protected void HandleInvalidSubmit()
        {
            StatusClass = "alert-danger";
            Message = new MarkupString("There are some validation errors. Please try again.");
        }

        protected async Task DeleteEmployee()
        {
            await EmployeeDataService.DeleteEmployee(Employee.EmployeeId);

            StatusClass = "alert-success";
            Message = new MarkupString("Deleted successfully");

            Saved = true;
        }

        protected void TempSave()
        {
            EmployeeDataService.SavedEmployee = Employee;
            NavigationManager.NavigateTo("/employeeoverview");
        }

        protected void NavigateToOverview()
        {
            NavigationManager.NavigateTo("/employeeoverview");
        }
    }
}
