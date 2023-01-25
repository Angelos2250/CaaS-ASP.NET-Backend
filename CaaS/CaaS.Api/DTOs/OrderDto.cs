namespace CaaS.Api.DTOs
{
    public class OrderDto
    {
        public int idOrder { get; set; }
        public DateTime dateOfOrder { get; set; }
        public float sumOfDiscount { get; set; }
        public int idCustomer { get; set; }
        public float sumAmount { get; set; }

    }
}
