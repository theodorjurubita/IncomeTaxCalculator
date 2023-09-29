using Application.Employees;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace IncomeTaxCalculator.Controllers
{
    public class EmployeesController : BaseApiController
    {
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(ILogger<EmployeesController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "CreateEmployeesCSV")]
        public async Task<IActionResult> CreateEmployeesCSV([FromForm] IFormFileCollection employeesCsv)
        {
            return Ok(await Mediator.Send(new CreateFromCsv.Command { EmployeesFromCsvFile = employeesCsv }));
        }

        [HttpGet(Name = "GetAllEmployees")]
        public async Task<List<Employee>> GetAll()
        {
            return await Mediator.Send(new GetAll.Query());
        }
    }
}