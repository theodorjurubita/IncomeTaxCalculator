using Microsoft.AspNetCore.Http;

namespace Application.CsvEntityReader.Employee
{
    public class EmployeeCsvReader : CsvEntityReader<EmployeeFromCsv>, IEmployeeCsvReader
    {
        public List<EmployeeFromCsv> ReadEmployeesFromFormFileCollection(IFormFileCollection formCollection)
        {

            return base.ReadEntitiesFromCsv(formCollection[0].OpenReadStream());
        }
    }
}
