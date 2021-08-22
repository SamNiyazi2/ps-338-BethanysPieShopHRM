using BethanysPieShopHRM.Shared;
using BethanysPieShopHRM.UI.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// 08/21/2021 03:23 pm - SSN - [20210821-1500] - [004] - M02-08 - Demo: Improving the component's reusability

namespace BethanysPieShopHRM.UI.Pages
{
    public partial class ReportConcerns : ComponentBase
    {
        public Concern NewConcern { get; set; } = new Concern();
        public bool EmailSent { get; set; } = false;

        [Inject]
        public IEmailService emailSerivce { get; set; }

        public void SubmitConcern()
        {
            var newEmail = new Email()
            {
                Body = NewConcern.Description,
                Subject = NewConcern.Title
            };

            emailSerivce.SendEmail();
            EmailSent = true;
        }

        public void ResetForm()
        {
            NewConcern = new Concern();
            EmailSent = false;
        }


    }
}
