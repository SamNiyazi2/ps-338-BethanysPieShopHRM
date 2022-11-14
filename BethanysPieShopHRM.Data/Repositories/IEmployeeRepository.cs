using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BethanysPieShopHRM.Shared;

namespace BethanysPieShopHRM.Api.Models
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployees();

        // 08/22/2021 01:29 pm - SSN - [20210822-1222] - [010] - M04-06 - Demo: Enhancing the application's routing features
        //Employee GetEmployeeById(int employeeId);
        Employee GetEmployeeById(int employeeId, bool noTracking = true);
        
        Employee AddEmployee(Employee employee);
        Employee UpdateEmployee(Employee employee);     
        void DeleteEmployee(int employeeId);
    }
}
