using Domain;

namespace Persistence
{
    public class Seed
    {
        public static async Task SeedData(HrContext context)
        {
            if (context.TaxBands.Any())
            {
                return;
            }

            var taxBands = new List<TaxBand> {
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

            context.TaxBands.AddRange(taxBands);
            await context.SaveChangesAsync();
        }
    }
}
