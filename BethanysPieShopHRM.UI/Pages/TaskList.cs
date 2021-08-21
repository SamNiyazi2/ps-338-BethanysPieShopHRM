using BethanysPieShopHRM.Shared;
using BethanysPieShopHRM.UI.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// 08/21/2021 03:03 pm - SSN - [20210821-1500] - [002] - M02-08 - Demo: Improving the component's reusability

namespace BethanysPieShopHRM.UI.Pages
{
    public partial class TaskList:ComponentBase
    {

        // 08/21/2021 03:00 pm - SSN - [20210821-1500] - [001] - M02-08 - Demo: Improving the component's reusability
        [Parameter]
        public int recordsToDisplay { get; set; }

        public int totalNumberOfTasks { get; set; }
        public bool showToalNumberOfTasks { get; set; }

        [Inject]
        public ITaskDataService taskService { get; set; }

        [Inject]
        public NavigationManager navManager { get; set; }



        public List<HRTask> Tasks { get; set; } = new List<HRTask>();

        protected override async Task OnInitializedAsync()
        {
            Tasks = (await taskService.GetAllTasks()).ToList();

            if (recordsToDisplay > 0)
            {
                totalNumberOfTasks = Tasks.Count;

                Tasks = Tasks.Take(recordsToDisplay).ToList();
                showToalNumberOfTasks = recordsToDisplay  < totalNumberOfTasks;

            }
        }


        public void AddTask()
        {
            navManager.NavigateTo("taskedit");
        }
    }
}
