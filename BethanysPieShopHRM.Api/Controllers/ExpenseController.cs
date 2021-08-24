using BethanysPieShopHRM.Api.Models;
using BethanysPieShopHRM.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BethanysPieShopHRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : Controller
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseController(IExpenseRepository ExpenseRepository)
        {
            _expenseRepository = ExpenseRepository;
        }

        // GET: api/<controller>
        [HttpGet]
        public IActionResult GetExpenses()
        {
            return Ok(_expenseRepository.GetAllExpenses());
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult GetExpenseById(int id)
        {


            // 08/24/2021 01:58 pm - SSN - [20210822-1222] - [042] - M04-06 - Demo: Enhancing the application's routing features
            // return Ok(_expenseRepository.GetExpenseById(id));

            try
            {
                Expense expense = _expenseRepository.GetExpenseById(id);
                if (expense != null)
                    return Ok(expense);
                else
                {
                    List<FeedbackMessage> feedbackMessages = new List<FeedbackMessage>();
                    feedbackMessages.Add_2("Record not found ", true);
                    return new JsonResult(feedbackMessages);
                }
            }
            catch (System.Exception ex)
            {
                List<FeedbackMessage> feedbackMessages = new List<FeedbackMessage>();
                feedbackMessages.Add_2("Captured error in API - 20210824-1401 Begin ", false);
                feedbackMessages.Add_2(ex.Message, true);
                feedbackMessages.Add_2(ex.StackTrace, true);
                feedbackMessages.Add_2("Captured error in API - 20210824-1401- End ", false);

                return new JsonResult(feedbackMessages);
            }

        }

        [HttpPost]
        public IActionResult CreateExpense([FromBody] Expense Expense)
        {
            if (Expense == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdExpense = _expenseRepository.AddExpense(Expense);

            return Created("Expense", createdExpense);
        }

        [HttpPut]
        public IActionResult UpdateExpense([FromBody] Expense expense)
        {
            if (expense == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var expenseToUpdate = _expenseRepository.GetExpenseById(expense.ExpenseId);

            if (expenseToUpdate == null)
                return NotFound();

            _expenseRepository.UpdateExpense(expense);

            return NoContent(); //success
        }

    }
}
