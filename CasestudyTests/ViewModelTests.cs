using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpdeskViewModels;

namespace CasestudyTests
{
    public class ViewModelTests
    {
        [Fact]
        public async Task Employee_GetByEmailTest()
        {
            EmployeeViewModel vm = new() { Email = "bs@abc.com" };
            await vm.GetByEmail();
            Assert.NotNull(vm.Firstname);
        }

        [Fact]
        public async Task Employee_GetByIdTest()
        {
            EmployeeViewModel vm = new() { Id = 1 };
            await vm.GetById();
            Assert.NotNull(vm.Firstname);
        }

        [Fact]
        public async Task Employee_GetAllTest()
        {
            List<EmployeeViewModel> allEmployeeVms;
            EmployeeViewModel vm = new();
            allEmployeeVms = await vm.GetAll();
            Assert.True(allEmployeeVms.Count > 0);
        }

        [Fact]
        public async Task Employee_AddTest()
        {
            EmployeeViewModel vm;
            vm = new()
            {
                Title = "Mr.",
                Firstname = "Rodrigo",
                Lastname = "Baiocchi Ferreira",
                Phoneno = "(777)777-7777",
                Email = "r_baiocchiferreira@fanshaweonline.ca",
                DepartmentId = 100,
                IsTech = true,
                Timer = Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))),
            };

            await vm.Add();
            Assert.True(vm.Id > 0);
        }

        [Fact]
        public async Task Employee_UpdateTest()
        {
            EmployeeViewModel vm = new() { Email = "r_baiocchiferreira@fanshaweonline.ca" };
            await vm.GetByEmail(); // Employee just added in Add test
            vm.Email = vm.Email == "r_baiocchiferreira@fanshaweonline.ca" ? "r_baiocchiferreira@fanshaweonline.ca" : "rbf@someemail.com";
            // will be -1 if failed, 0 if no data changed, 1 if successful
            Assert.True(await vm.Update() == 1); // 1 indicate the # of rows updated
        }

        [Fact]
        public async Task Employee_DeleteTest()
        {
            EmployeeViewModel vm = new() { Phoneno = "(777)777-7777" };
            await vm.GetByPhoneNumber(); // Employee just added

            Assert.True(await vm.Delete() == 1); // 1 Employee deleted
        }

        [Fact]
        public async Task Employee_GetByPhoneTest()
        {
            EmployeeViewModel vm = new() { Phoneno = "(777)777-7777" };
            await vm.GetByPhoneNumber();

            Assert.NotNull(vm.Firstname);
        }
    }
}
