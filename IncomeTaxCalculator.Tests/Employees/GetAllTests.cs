using Application.Employees;
using AutoFixture.Xunit2;
using Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Persistence;

namespace IncomeTaxCalculator.Tests.Employees
{
    public class GetAllTests
    {
        [Theory]
        [AutoData]
        public async Task GetAll_WhenContextHasData_ReturnsTheData(List<Employee> fakeEmployeesList)
        {
            // Arrange
            var hrContextMock = new Mock<HrContext>();
            hrContextMock.Setup<DbSet<Employee>>(a => a.Employees)
                .ReturnsDbSet(fakeEmployeesList);

            // Act
            var getAllHandler = new GetAll.Handler(hrContextMock.Object);
            var result = await getAllHandler.Handle(new GetAll.Query(), CancellationToken.None);

            // Assert
            Assert.Equivalent(fakeEmployeesList, result);
        }
    }
}
