namespace Hotel.Client.DTOs
{
    public class PaymentDTOs
    {
        public string PackageName { get; set; }
        public double? TotalPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string HtelCode { get; set; }
        public string Gmail { get; set; }
    }
}
