using Application.CsvEntityReader;
using Application.CsvEntityReader.Employee;
using Application.Services;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Employees
{
    public class CreateFromCsv
    {
        public class Command : IRequest
        {
            public IFormFileCollection EmployeesFromCsvFile { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly HrContext _context;
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;
            private readonly IEmployeeCsvReader _employeeCsvEntityReader;
            private readonly AnnualncomeCalculatorService _annualncomeCalculatorService;

            public Handler(HrContext context, IMapper mapper, ILogger<Handler> logger, IEmployeeCsvReader employeeCsvEntityReader, AnnualncomeCalculatorService annualncomeCalculatorService)
            {
                _context = context;
                _mapper = mapper;
                _logger = logger;
                _employeeCsvEntityReader = employeeCsvEntityReader;
                _annualncomeCalculatorService = annualncomeCalculatorService;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                IEnumerable<Employee> employees;
                var taxBands = await _context.TaxBands.ToListAsync(cancellationToken);

                try
                {
                    var records = _employeeCsvEntityReader.ReadEmployeesFromFormFileCollection(request.EmployeesFromCsvFile);

                    employees = records.Select(r =>
                    {
                        return new Employee
                        {
                            EmployeeId = r.EmployeeID,
                            FirstName = r.FirstName,
                            LastName = r.LastName,
                            BirthDate = r.DateOfBirth,
                            AnnualIncome = _annualncomeCalculatorService.CalculateAnnualIncome(r.GrossAnnualSalary, taxBands)
                        };
                    });


                    var existingEmployees = _context.Employees.Where(e => employees.Select(a => a.EmployeeId).Contains(e.EmployeeId)).ToList();
                    foreach (var employee in employees)
                    {
                        var existingEmployee = existingEmployees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
                        if (existingEmployee != null)
                        {
                            _mapper.Map(employee, existingEmployee);
                        }
                        else
                        {
                            _context.Employees.Add(employee);
                        }
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError($"Could not create the employees from csv, the following error occured: {exception.Message}");
                }
                return Unit.Value;
            }
        }
    }
}
