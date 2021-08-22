using BethanysPieShopHRM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// 08/21/2021 07:08 pm - SSN - [20210821-1903] - [001] - M03-03 - Demo: Imporving components using dependency injection

namespace BethanysPieShopHRM.UI.Services
{
    public class ExpenseApprovalService : IExpenseApprovalService
    {

        public ExpenseStatus GetExpenseStatus(Expense expense, Employee employee)
        {

            if (employee.IsOPEX)
            {
                switch (expense.ExpenseType)
                {
                    case ExpenseType.Conference:
                        return ExpenseStatus.Denied;

                    case ExpenseType.Transportation:
                        return ExpenseStatus.Denied;

                    case ExpenseType.Hotel:
                        return ExpenseStatus.Denied;

                }

                if (expense.Status != ExpenseStatus.Denied)
                {
                    expense.CoveredAmount = expense.Amount / 2;
                }
            }

            if (!employee.IsFTE)
            {
                if (expense.ExpenseType != ExpenseType.Training)
                {
                    return ExpenseStatus.Denied;
                }
            }

            if (expense.ExpenseType == ExpenseType.Food && expense.Amount > 100)
            {
                return ExpenseStatus.Pending;
            }

            if (expense.Amount > 5000)
            {
                return ExpenseStatus.Pending;
            }

            return ExpenseStatus.Pending;


        }
    }
}
