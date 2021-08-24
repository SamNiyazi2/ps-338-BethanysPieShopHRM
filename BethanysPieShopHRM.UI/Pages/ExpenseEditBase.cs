using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BethanysPieShopHRM.UI.Components;
using BethanysPieShopHRM.UI.Services;
using BethanysPieShopHRM.Shared;
using Microsoft.AspNetCore.Components;

namespace BethanysPieShopHRM.UI.Pages
{
    public class ExpenseEditBase : ComponentBase
    {
        [Inject]
        public IExpenseDataService ExpenseDataService { get; set; }

        [Inject]
        public IEmployeeDataService EmployeeDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ICurrencyDataService CurrencyDataService { get; set; }

        [Inject]
        public IExpenseApprovalService ExpenseApprovalService { get; set; }


        public Expense Expense { get; set; } = new Expense();

        //needed to bind to select value
        protected string CurrencyId = "1";

        // 08/22/2021 12:41 pm - SSN - [20210822-1222] - [004] - M04-06 - Demo: Enhancing the application's routing features
        // protected string EmployeeId = "1";
        protected int EmployeeId ;

        [Parameter]
        // 08/24/2021 02:28 pm - SSN - [20210822-1222] - [044] - M04-06 - Demo: Enhancing the application's routing features
        //public string ExpenseId { get; set; }
        public int ExpenseId { get; set; }


        public string Message { get; set; }
        public List<Currency> Currencies { get; set; } = new List<Currency>();
        public List<Employee> Employees { get; set; } = new List<Employee>();


        public bool DisplayForm  { get; set; }
        public MarkupString FeedbackMessages { get; set; }


        protected override async Task OnInitializedAsync()
        {
            Employees = (await EmployeeDataService.GetAllEmployees()).ToList();
            Currencies = (await CurrencyDataService.GetAllCurrencies()).ToList();

            // 08/24/2021 02:28 pm - SSN - [20210822-1222] - [044] - M04-06 - Demo: Enhancing the application's routing features
            // int.TryParse(ExpenseId, out var expenseId);

            DisplayForm = false;

            // 08/24/2021 02:28 pm - SSN - [20210822-1222] - [044] - M04-06 - Demo: Enhancing the application's routing features
            // if (expenseId != 0)
            if (ExpenseId != 0)
            {
                try
                {
                    // 08/24/2021 01:49 pm - SSN - [20210822-1222] - [041] - M04-06 - Demo: Enhancing the application's routing features
                    // 08/24/2021 02:28 pm - SSN - [20210822-1222] - [044] - M04-06 - Demo: Enhancing the application's routing features
                    // APIBag<Expense> result = await ExpenseDataService.GetExpenseById(expenseId);
                    APIBag<Expense> result = await ExpenseDataService.GetExpenseById(ExpenseId);

                    if (result.ModelRecord == null)
                    {
                        FeedbackMessages = new MarkupString(result.FeedbackMessages.GetFeedbackMessagesAsHTML());
                    }
                    else
                    {
                        // Expense = await ExpenseDataService.GetExpenseById(int.Parse(ExpenseId));
                        Expense = result.ModelRecord;
                        DisplayForm = true;
                    }
                }
                catch (System.Exception ex )
                {
                    FeedbackMessages = new MarkupString( ex.Message) ;
                }
            } 
            else
            {
                Expense = new Expense() { EmployeeId = 1, CurrencyId = 1, Status = ExpenseStatus.Open, ExpenseType = ExpenseType.Other };
                DisplayForm = true;
            }

            CurrencyId = Expense.CurrencyId.ToString();

            // 08/22/2021 12:41 pm - SSN - [20210822-1222] - [004] - M04-06 - Demo: Enhancing the application's routing features
            // EmployeeId = Expense.EmployeeId.ToString();
            EmployeeId = Expense.EmployeeId;
        }

        protected async Task HandleValidSubmit()
        {

            // 08/22/2021 12:41 pm - SSN - [20210822-1222] - [005] - M04-06 - Demo: Enhancing the application's routing features
            // Expense.EmployeeId = int.Parse(EmployeeId);
            Expense.EmployeeId = EmployeeId;

            Expense.CurrencyId = int.Parse(CurrencyId);


            //////////////////////// Expense.Amount *= Currencies.FirstOrDefault(x => x.CurrencyId == Expense.CurrencyId).USExchange;

            Expense.Status = await ExpenseApprovalService.GetExpenseStatus(Expense);


            if (Expense.ExpenseId == 0) // New 
            {
                await ExpenseDataService.AddExpense(Expense);
                NavigationManager.NavigateTo("/expenses");
            } 
            else
            {
                await ExpenseDataService.UpdateExpense(Expense);
                NavigationManager.NavigateTo("/expenses");
            }
        }

        protected void NavigateToOverview()
        {
            NavigationManager.NavigateTo("/expenses");
        }
    }
}
