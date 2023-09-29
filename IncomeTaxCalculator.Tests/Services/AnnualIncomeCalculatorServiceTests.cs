using Application.Services;
using Domain;

namespace IncomeTaxCalculator.Tests.Services
{
    public class AnnualIncomeCalculatorServiceTests
    {
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
        [InlineData(60000, 41000)]
        [InlineData(1000, 1000)]
        [InlineData(10000, 9000)]
        [InlineData(-1, -1)]
        public void CalculateAnnualIncome_WhenDataIsValid_ReturnsNetAnnualIncome(decimal grossAmmount, decimal netAmmount)
        {
            // Arrange
            var service = new AnnualncomeCalculatorService();

            // Act
            var result = service.CalculateAnnualIncome(grossAmmount, taxBands);

            // Assert
            Assert.Equal(netAmmount, result);
        }
    }
}
