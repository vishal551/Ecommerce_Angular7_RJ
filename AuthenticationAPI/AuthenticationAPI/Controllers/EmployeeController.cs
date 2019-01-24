using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using AuthenticationAPI.Models;


namespace AuthenticationAPI.Controllers
{
    [RoutePrefix("api/Employee")]
    public class EmployeeController : ApiController
    {
        [HttpGet]
        [Authorize]
        [Route("GetEmployee")]
        public IHttpActionResult GetEmployee()
        {
            IList<Employee_Master> employee = null;
            using (var DB = new AngularDemoEntities())
            {
                employee = DB.Employee_Master.ToList<Employee_Master>();
            }
            if (employee.Count == 0)
            {
                return NotFound();
            }
            return Ok(employee);
        }
    }
}
