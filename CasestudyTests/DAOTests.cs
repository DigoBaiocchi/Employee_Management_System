using HelpdeskDAL;

namespace CasestudyTests
{
    public class DAOTests
    {
        [Fact]
        public async Task Employee_GetByEmailTest()
        {
            EmployeeDAO dao = new();
            Employee selectedEmployee = await dao.GetByEmail("bs@abc.com");
            Assert.NotNull(selectedEmployee);
        }

        [Fact]
        public async Task Employee_GetByIdTest()
        {
            EmployeeDAO dao = new();
            Employee selectedEmployee = await dao.GetById(1);
            Assert.NotNull(selectedEmployee);
        }

        [Fact]
        public async Task Employee_GetAllTest()
        {
            EmployeeDAO dao = new();
            List<Employee> selectedEmployee = await dao.GetAll();
            Assert.True(selectedEmployee.Count > 0);
        }

        [Fact]
        public async Task Employee_AddTest()
        {
            EmployeeDAO dao = new();
            Employee newEmployee = new()
            {
                Title = "Mr.",
                FirstName = "Rodrigo",
                LastName = "Baiocchi Ferreira",
                PhoneNo = "(555)555-1234",
                Email = "r_baiocchiferreira@fanshaweonline.ca",
                DepartmentId = 100,
                IsTech = true,
                StaffPicture = null,
            };

            Assert.True(await dao.AddEmployee(newEmployee) > 0);
        }

        [Fact]
        public async Task Employee_GetByPhoneTest()
        {
            EmployeeDAO dao = new();
            Employee selectedEmployee = await dao.GetByPhoneNumber("(555)555-1234");

            Assert.NotNull(selectedEmployee);
        }

        [Fact]
        public async Task Employee_UpdateTest()
        {
            EmployeeDAO dao = new();
            Employee? EmployeeForUpdate = await dao.GetByEmail("r_baiocchiferreira@fanshaweonline.ca");

            if (EmployeeForUpdate != null)
            {
                string oldPhoneNo = EmployeeForUpdate.PhoneNo!;
                string newPhoneNo = oldPhoneNo == "(555)555-1234" ? "555-555-5555" : "(555)555-1234";
                EmployeeForUpdate!.PhoneNo = newPhoneNo;
            }
            Assert.True(await dao.Update(EmployeeForUpdate!) == UpdateStatus.OK); // 1 indicate the # of rows updated
        }

        [Fact]
        public async Task Employee_DeleteTest()
        {
            EmployeeDAO dao = new();
            Employee? EmployeeForDelete = await dao.GetByEmail("r_baiocchiferreira@fanshaweonline.ca");

            Assert.True(await dao.Delete(EmployeeForDelete!.Id) == 1);
        }

        [Fact]
        public async Task Employee_ConcurrencyTest()
        {
            EmployeeDAO dao1 = new();
            EmployeeDAO dao2 = new();
            Employee employeeForUpdate1 = await dao1.GetByEmail("r_baiocchiferreira@fanshaweonline.ca");
            Employee employeeForUpdate2 = await dao2.GetByEmail("r_baiocchiferreira@fanshaweonline.ca");

            if (employeeForUpdate1 != null)
            {
                string? oldPhoneNo = employeeForUpdate1.PhoneNo!;
                string? newPhoneNo = oldPhoneNo == "(555)555-1234" ? "555-555-5555" : "(555)555-1234";
                employeeForUpdate1.PhoneNo = newPhoneNo;
                if (await dao1.Update(employeeForUpdate1) == UpdateStatus.OK)
                {
                    employeeForUpdate2.PhoneNo = "666-666-6668";
                    Assert.True(await dao2.Update(employeeForUpdate2) == UpdateStatus.Stale);
                }
                else
                    Assert.True(false);
            }
            else
                Assert.True(false);
        }

        [Fact]
        public async Task Employee_LoadPicsTest()
        {
            PicsUtility util = new();
            Assert.True(await util.AddEmployeePicsToDb());
        }
    }
}