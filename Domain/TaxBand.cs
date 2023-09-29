namespace Domain
{
    public class TaxBand
    {
        public int TaxBandId { get; set; }
        public string TaxBandName { get; set; }
        public int TaxBandLowerLimit { get; set; }
        public int TaxBandUpperLimit { get; set; }
        public int TaxRate { get; set; }
    }
}
