using HelpdeskViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;

namespace HelpdeskWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        [HttpGet("{email}")]
        public async Task<IActionResult> GetByLastname(string email)
        {
            try
            {
                EmployeeViewModel viewmodel = new() { Email = email };
                await viewmodel.GetByEmail();
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put(EmployeeViewModel viewModel)
        {
            try
            {
                int retVal = await viewModel.Update();
                return retVal switch
                {
                    1 => Ok(new { msg = "Employee " + viewModel.Lastname + " updated!" }),
                    -1 => Ok(new { msg = "Employee " + viewModel.Lastname + " not updated!" }),
                    -2 => Ok(new { msg = "Data is stale for " + viewModel.Lastname + ", Employee not updated!" }),
                    _ => Ok(new { msg = "Employee " + viewModel.Lastname + " not updated!" })
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                EmployeeViewModel viewModel = new();
                List<EmployeeViewModel> allEmployees = await viewModel.GetAll();
                return Ok(allEmployees);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong    
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(EmployeeViewModel viewModel)
        {
            try
            {
                await viewModel.Add();
                return viewModel.Id > 1
                    ? Ok(new { msg = "Employee " + viewModel.Lastname + " added!" })
                    : Ok(new { msg = "Employee " + viewModel.Lastname + " NOT added!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
    }
}
