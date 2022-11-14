using System;
using System.Collections.Generic;
using System.Linq;
using BethanysPieShopHRM.Shared;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShopHRM.Api.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _appDbContext;

        public EmployeeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _appDbContext.Employees;
        }


        // 08/22/2021 01:30 pm - SSN - [20210822-1222] - [011] - M04-06 - Demo: Enhancing the application's routing features
        // public Employee GetEmployeeById(int employeeId)
        public Employee GetEmployeeById(int employeeId, bool noTracking = true)
        {
            // 08/22/2021 07:08 am - SSN - [20210822-0652] - [002] - M03-06 - Demo: Managing dependency implementation
            // return _appDbContext.Employees.FirstOrDefault(c => c.EmployeeId == employeeId);
            // 08/24/2021 07:31 am - SSN - [20210822-1222] - [034] - M04-06 - Demo: Enhancing the application's routing features
            // Already added include JobCategory.  Adding include Country.
              
            if (noTracking)
            {
                var results = _appDbContext.Employees.Include(r => r.Country).Include(r => r.JobCategory).AsNoTracking().FirstOrDefault(c => c.EmployeeId == employeeId);
                return results;
            }
            else
            {
                var results =  _appDbContext.Employees.Include(r => r.Country).Include(r => r.JobCategory).FirstOrDefault(c => c.EmployeeId == employeeId);
                return results;
            }
        }

        public Employee AddEmployee(Employee employee)
        {
            var addedEntity = _appDbContext.Employees.Add(employee);
            _appDbContext.SaveChanges();
            return addedEntity.Entity;
        }


        // 08/24/2021 09:17 am - SSN - [20210822-1222] - [038] - M04-06 - Demo: Enhancing the application's routing features
        // public void UpdateEmployee(Employee employee)
        public Employee UpdateEmployee(Employee employee)
        {


            // var foundEmployee = _appDbContext.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);

            // if (foundEmployee != null)
            //{
            // 08/22/2021 12:58 pm - SSN - [20210822-1222] - [008] - M04-06 - Demo: Enhancing the application's routing features

            //    foundEmployee.CountryId = employee.CountryId;
            //    foundEmployee.MaritalStatus = employee.MaritalStatus;
            //    foundEmployee.BirthDate = employee.BirthDate;
            //    foundEmployee.City = employee.City;
            //    foundEmployee.Email = employee.Email;
            //    foundEmployee.FirstName = employee.FirstName;
            //    foundEmployee.LastName = employee.LastName;
            //    foundEmployee.Gender = employee.Gender;
            //    foundEmployee.PhoneNumber = employee.PhoneNumber;
            //    foundEmployee.Smoker = employee.Smoker;
            //    foundEmployee.Street = employee.Street;
            //    foundEmployee.Zip = employee.Zip;
            //    foundEmployee.JobCategoryId = employee.JobCategoryId;
            //    foundEmployee.Comment = employee.Comment;
            //    foundEmployee.ExitDate = employee.ExitDate;
            //    foundEmployee.JoinedDate = employee.JoinedDate;


            employee.Country = null;
            employee.JobCategory = null;

            _appDbContext.Attach(employee).State = EntityState.Modified;


            int recordsChanged = _appDbContext.SaveChanges();
            return employee;

            //     return foundEmployee;
            //  }


            //  return null;
        }

        public void DeleteEmployee(int employeeId)
        {
            var foundEmployee = _appDbContext.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
            if (foundEmployee == null) return;

            _appDbContext.Employees.Remove(foundEmployee);
            _appDbContext.SaveChanges();
        }
    }
}
