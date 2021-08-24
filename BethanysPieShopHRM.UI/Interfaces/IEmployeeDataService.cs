using System.Collections.Generic;
using System.Threading.Tasks;
using BethanysPieShopHRM.Shared;

namespace BethanysPieShopHRM.UI.Services
{
    public interface IEmployeeDataService
    {
        Employee SavedEmployee { get; set; }
        Task<IEnumerable<Employee>> GetAllEmployees();

        // 08/23/2021 03:28 pm - SSN - [20210822-1222] - [022] - M04-06 - Demo: Enhancing the application's routing features
        // Task<Employee> GetEmployeeDetails(int employeeId);
        Task<APIBag<Employee>> GetEmployeeDetails(int employeeId);
        
        Task<Employee> AddEmployee(Employee employee);

        // 08/23/2021 07:48 am - SSN - [20210822-1222] - [017] - M04-06 - Demo: Enhancing the application's routing features
        // Task UpdateEmployee(Employee employee);
        Task<APIBag<Employee>> UpdateEmployee(Employee employee);
        
        Task DeleteEmployee(int employeeId);
    }
}
