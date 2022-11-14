using BethanysPieShopHRM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanysPieShopHRM.UI.Services
{
    public class ManagerApprovalService : IExpenseApprovalService
    {
        private readonly IEmployeeDataService _employeeService;

        public ManagerApprovalService(IEmployeeDataService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<ExpenseStatus> GetExpenseStatus(Expense expense)
        {

            // 08/23/2021 03:43 pm - SSN - [20210822-1222] - [026] - M04-06 - Demo: Enhancing the application's routing features
            // var employee = await _employeeService.GetEmployeeDetails(expense.EmployeeId);

            var employee = default(Employee);

            APIBag<Employee> result = await _employeeService.GetEmployeeDetails(expense.EmployeeId);
            if (result.FeedbackMessages.Count == 0)
            {
                employee = result.ModelRecord;
            }
            else
            {
                throw new Exception($"Incomplete coding - 20210823-1544 - ManagerApprovalService: Failed to getEmployeeDetail [{expense.EmployeeId}]");
            }




            // New sample approval scenarios
            if (employee.IsFTE)
            {
                if (expense.Amount < 250)
                {
                    switch (expense.ExpenseType)
                    {
                        case ExpenseType.Training:
                            return ExpenseStatus.Approved;
                        case ExpenseType.Food:
                            return ExpenseStatus.Approved;
                        case ExpenseType.Office:
                            return ExpenseStatus.Approved;
                    }
                }

                if (employee.JobCategory.JobCategoryName == "Sales" && expense.Amount < 500)
                {
                    switch (expense.ExpenseType)
                    {
                        case ExpenseType.Transportation:
                            return ExpenseStatus.Approved;
                        case ExpenseType.Travel:
                            return ExpenseStatus.Approved;
                        case ExpenseType.Hotel:
                            return ExpenseStatus.Approved;
                    }
                }
            }
            
            return ExpenseStatus.Pending;
        }
    }
}
