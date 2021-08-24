using BethanysPieShopHRM.Api.Models;
using BethanysPieShopHRM.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BethanysPieShopHRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            return Ok(_employeeRepository.GetAllEmployees());
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            try
            {
                Employee employee = _employeeRepository.GetEmployeeById(id);
                if (employee != null)
                    return Ok(employee);
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
                feedbackMessages.Add_2("Captured error in API - 20210824-1234 - Begin ", false);
                feedbackMessages.Add_2(ex.Message, true);
                feedbackMessages.Add_2(ex.StackTrace, true);
                feedbackMessages.Add_2("Captured error in API - 20210824-1234- End ", false);

                return new JsonResult(feedbackMessages);
            }
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            if (employee == null)
                return BadRequest();

            if (employee.FirstName == string.Empty || employee.LastName == string.Empty)
            {
                ModelState.AddModelError("Name/FirstName", "The name or first name shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdEmployee = _employeeRepository.AddEmployee(employee);

            return Created("employee", createdEmployee);
        }

        [HttpPut]
        public IActionResult UpdateEmployee([FromBody] Employee employee)
        {
            if (employee == null)
                return BadRequest();

            if (employee.FirstName == string.Empty || employee.LastName == string.Empty)
            {
                ModelState.AddModelError("Name/FirstName", "The name or first name shouldn't be empty (2202)");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            // 08/22/2021 01:29 pm - SSN - [20210822-1222] - [009] - M04-06 - Demo: Enhancing the application's routing features

            // var employeeToUpdate = _employeeRepository.GetEmployeeById(employee.EmployeeId);
            var employeeToUpdate = _employeeRepository.GetEmployeeById(employee.EmployeeId);

            if (employeeToUpdate == null)
                return NotFound();

            try
            {
                _employeeRepository.UpdateEmployee(employee);
            }
            catch (System.Exception ex)
            {
                // Does not work on model.
                // ModelState.AddModelError("Name/FirstName", ex.Message);

                List<FeedbackMessage> feedbackMessages = new List<FeedbackMessage>();

                feedbackMessages.Add_2(ex.Message, true);

                return BadRequest(feedbackMessages);
            }

            return NoContent(); //success
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            if (id == 0)
                return BadRequest();

            var employeeToDelete = _employeeRepository.GetEmployeeById(id);
            if (employeeToDelete == null)
                return NotFound();

            _employeeRepository.DeleteEmployee(id);

            return NoContent();//success
        }
    }
}
