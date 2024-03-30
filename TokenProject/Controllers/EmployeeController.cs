using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TokenProject.ViewModels;

namespace TokenProject.Controllers
{
    [ApiController, Authorize, Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        public EmployeeController()
        {
            
        }

        [HttpGet]
        public ActionResult<List<EmployeeViewModel>> Gets()
        {
            var _empList = new List<EmployeeViewModel>();
            for (int _a = 1; _a <= 5; _a++)
            {
                //new EmployeeViewModel { EmployeeId = 1, EmployeeNo = "1", EmployeeName = "Employee1", Level = "Admin1", Position = "Staff1" },
                //new EmployeeViewModel { EmployeeId = 2, EmployeeNo = "2", EmployeeName = "Employee2", Level = "Admin2", Position = "Staff2" },
                //new EmployeeViewModel { EmployeeId = 3, EmployeeNo = "3", EmployeeName = "Employee3", Level = "Admin3", Position = "Staff3" },
                //new EmployeeViewModel { EmployeeId = 4, EmployeeNo = "4", EmployeeName = "Employee4", Level = "Admin4", Position = "Staff4" },

                _empList.Add(new EmployeeViewModel { EmployeeId = _a, EmployeeNo = _a.ToString(), EmployeeName = "Employee"+ _a.ToString(), Level = "Admin"+_a.ToString(), Position = "Staff"+ _a.ToString() });
            };
            return _empList.ToList();
        }
    }
}
