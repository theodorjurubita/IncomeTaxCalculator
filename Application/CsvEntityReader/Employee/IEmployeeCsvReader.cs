using Microsoft.AspNetCore.Http;

namespace Application.CsvEntityReader.Employee
{
    public interface IEmployeeCsvReader
    {
        List<EmployeeFromCsv> ReadEmployeesFromFormFileCollection(IFormFileCollection formCollection);
    }
}
