using Domain;

namespace Application.Services
{
    public class AnnualncomeCalculatorService
    {
        private decimal CalculateAnnualIncomeTax(decimal grossAnnualSalary, List<TaxBand> taxBands)
        {
            decimal taxToPay = 0;
            foreach (var taxBand in taxBands)
            {
                var taxableValueFromTaxBand = taxBand.TaxBandUpperLimit - taxBand.TaxBandLowerLimit;
                if (grossAnnualSalary <= 0)
                {
                    break;
                }
                taxToPay += grossAnnualSalary > taxBand.TaxBandUpperLimit ? (taxableValueFromTaxBand * taxBand.TaxRate) / 100 : (grossAnnualSalary * taxBand.TaxRate) / 100;
                grossAnnualSalary -= taxableValueFromTaxBand;
            }

            return taxToPay;
        }

        public decimal CalculateAnnualIncome(decimal grossAnnualSalary, List<TaxBand> taxBands)
        {
            return grossAnnualSalary - CalculateAnnualIncomeTax(grossAnnualSalary, taxBands);
        }
    }
}
