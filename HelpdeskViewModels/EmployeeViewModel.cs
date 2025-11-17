using HelpdeskDAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HelpdeskViewModels
{
    public class EmployeeViewModel
    {
        private readonly EmployeeDAO _dao;

        public string? Title { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Phoneno { get; set; }
        public string? Timer { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int Id { get; set; }
        public bool? IsTech {  get; set; }
        public string? StaffPicture64 { get; set; }

        // constructor
        public EmployeeViewModel() 
        {
            _dao = new EmployeeDAO();    
        }

        public async Task GetByEmail()
        {
            try
            {
                Employee emp = await _dao.GetByEmail(Email!);
                Id = emp.Id;
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                DepartmentId = emp.DepartmentId;
                IsTech = emp.IsTech;
                if (emp.StaffPicture != null)
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                Timer = Convert.ToBase64String(emp.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Email = "Not found";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        public async Task GetById()
        {
            try
            {
                Employee emp = await _dao.GetById(Id!);
                Id = emp.Id;
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                DepartmentId = emp.DepartmentId;
                IsTech = emp.IsTech;
                if (emp.StaffPicture != null)
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                Timer = Convert.ToBase64String(emp.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        public async Task<List<EmployeeViewModel>> GetAll()
        {
            List<EmployeeViewModel> allVms = new();
            try
            {
                List<Employee> allEmployees = await _dao.GetAll();

                foreach (Employee emp in allEmployees)
                {
                    EmployeeViewModel empVm = new()
                    {
                        Id = emp.Id,
                        Title = emp.Title,
                        Firstname = emp.FirstName,
                        Lastname = emp.LastName,
                        Phoneno = emp.PhoneNo,
                        Email = emp.Email,
                        DepartmentId = emp.DepartmentId,
                        IsTech = emp.IsTech,
                        StaffPicture64 = StaffPicture64 != null ? Convert.ToBase64String(emp.StaffPicture!) : null,
                        Timer = Convert.ToBase64String(emp.Timer)
                    };

                    allVms.Add(empVm);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }

            return allVms;
        }

        public async Task Add()
        {
            Id = -1;
            try
            {
                Employee emp = new()
                {
                    Title = Title,
                    FirstName = Firstname,
                    LastName = Lastname,
                    PhoneNo = Phoneno,
                    Email = Email,
                    DepartmentId = DepartmentId,
                    IsTech = IsTech,
                    StaffPicture = StaffPicture64 != null ? Convert.FromBase64String(StaffPicture64!) : null
                };
                Id = await _dao.AddEmployee(emp);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        public async Task<int> Update()
        {
            int updateStatus;
            try
            {
                Employee emp = new()
                {
                    Id = Id,
                    Title = Title,
                    FirstName = Firstname,
                    LastName = Lastname,
                    PhoneNo = Phoneno,
                    Email = Email,
                    DepartmentId = DepartmentId,
                    IsTech = IsTech,
                    StaffPicture = StaffPicture64 != null ? Convert.FromBase64String(StaffPicture64!) : null,
                    Timer = Convert.FromBase64String(Timer!)
                };

                updateStatus = -1;
                updateStatus = Convert.ToInt16(await _dao.Update(emp));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return updateStatus;
        }

        public async Task<int> Delete()
        {
            try
            {
                return await _dao.Delete(Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        public async Task GetByPhoneNumber()
        {
            try
            {
                Employee emp = await _dao.GetByPhoneNumber(Phoneno!);
                Id = emp.Id;
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                DepartmentId = emp.DepartmentId;
                IsTech = emp.IsTech;
                StaffPicture64 = StaffPicture64 != null ? Convert.ToBase64String(emp.StaffPicture!) : null;
                Timer = Convert.ToBase64String(emp.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Phoneno = "Not found";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }
    }
}
