using Application;
using Application.CsvEntityReader.Employee;
using Application.Employees;
using AutoFixture.Xunit2;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Persistence;

namespace IncomeTaxCalculator.Tests.Employees
{
    public class CreateFromCsvTests
    {
        private readonly Mock<IMapper> mapperMock = new();
        private readonly Mock<ILogger<CreateFromCsv.Handler>> loggerMock = new();
        private readonly List<TaxBand> taxBands = new List<TaxBand>
        {
            new TaxBand
            {
                TaxBandId = 1,
                TaxBandName = "Tax Band A",
                TaxBandLowerLimit = 0,
                TaxBandUpperLimit = 5000,
                TaxRate = 0
            },
            new TaxBand
            {
                TaxBandId = 2,
                TaxBandName = "Tax Band B",
                TaxBandLowerLimit = 5000,
                TaxBandUpperLimit = 20000,
                TaxRate = 20
            },
            new TaxBand
            {
                TaxBandId = 3,
                TaxBandName = "Tax Band C",
                TaxBandLowerLimit = 20000,
                TaxBandUpperLimit = int.MaxValue,
                TaxRate = 40
            }
        };

        [Theory]
        [AutoData]
        public async Task CreateFromCsv_WhenStreamHasData_ReturnsNoException(
            List<Employee> fakeEmployeesList,
            List<EmployeeFromCsv> fakeEmployeesFromCsv)
        {
            // Arrange
            var hrContextMoock = new Mock<HrContext>();
            hrContextMoock.Setup<DbSet<Employee>>(a => a.Employees)
                .ReturnsDbSet(fakeEmployeesList);
            hrContextMoock.Setup<DbSet<TaxBand>>(a => a.TaxBands)
                .ReturnsDbSet(taxBands);
            var csvEntityReaderMock = new Mock<IEmployeeCsvReader>();
            csvEntityReaderMock.Setup(c => c.ReadEmployeesFromFormFileCollection(It.IsAny<IFormFileCollection>()))
                .Returns(fakeEmployeesFromCsv);

            // Act
            var createFromCsvHandler = new CreateFromCsv.Handler(
                hrContextMoock.Object,
                mapperMock.Object,
                loggerMock.Object,
                csvEntityReaderMock.Object,
                new Application.Services.AnnualncomeCalculatorService());
            var result = await createFromCsvHandler.Handle(
                new CreateFromCsv.Command(),
                CancellationToken.None);

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Never);
        }

        [Fact]
        public async Task CreateFromCsv_WhenStreamThrowsException_LogsError()
        {
            // Arrange
            var testException = new Exception("Test exception");
            var hrContextMoock = new Mock<HrContext>();
            hrContextMoock.Setup<DbSet<TaxBand>>(a => a.TaxBands)
                .ReturnsDbSet(taxBands);
            var csvEntityReaderMock = new Mock<IEmployeeCsvReader>();
            csvEntityReaderMock.Setup(c => c.ReadEmployeesFromFormFileCollection(It.IsAny<IFormFileCollection>()))
                .Throws(testException);

            var createFromCsvHandler = new CreateFromCsv.Handler(
                hrContextMoock.Object,
                mapperMock.Object,
                loggerMock.Object,
                csvEntityReaderMock.Object,
                new Application.Services.AnnualncomeCalculatorService());

            // Act
            var result = await createFromCsvHandler.Handle(
                new CreateFromCsv.Command(),
                CancellationToken.None);

            // Assert
            loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Equals($"Could not create the employees from csv, the following error occured: {testException.Message}")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }
    }
}