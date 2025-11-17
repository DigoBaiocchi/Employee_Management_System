using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HelpdeskDAL
{
    public class EmployeeDAO
    {
        readonly IRepository<Employee> _repo;
        public EmployeeDAO()
        {
            _repo = new HelpDeskRepository<Employee>();
        }
        public async Task<Employee> GetByEmail(string email)
        {
            Employee? selectedEmployee;
            try
            {
                selectedEmployee = await _repo.GetOne(emp => emp.Email == email);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }

            return selectedEmployee!;
        }

        public async Task<Employee> GetById(int id)
        {
            Employee? selectedEmployee;
            try
            {
                selectedEmployee = await _repo.GetOne(emp => emp.Id == id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }

            return selectedEmployee!;
        }
        public async Task<Employee> GetByPhoneNumber(string phoneNumber)
        {
            Employee? allEmployee;
            try
            {
                allEmployee = await _repo.GetOne(emp => emp.PhoneNo == phoneNumber);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }

            return allEmployee!;
        }

        public async Task<List<Employee>> GetAll()
        {
            List<Employee> allEmployees;
            try
            {
                allEmployees = await _repo.GetAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }

            return allEmployees;
        }

        public async Task<int> AddEmployee(Employee newEmployee)
        {
            try
            {
                await _repo.Add(newEmployee);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }

            return newEmployee.Id;
        }

        public async Task<UpdateStatus> Update(Employee updatedEmployee)
        {
            UpdateStatus status = UpdateStatus.Failed;
            try
            {
                status = await _repo.Update(updatedEmployee);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return status;
        }

        public async Task<int> Delete(int? id)
        {
            int EmployeesDeleted = -1;
            try
            {
                EmployeesDeleted = await _repo.Delete((int)id!); // save changes to database
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }

            return EmployeesDeleted;
        }
    }
}
