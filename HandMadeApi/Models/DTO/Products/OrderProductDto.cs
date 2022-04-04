using HandMadeApi.Models.StoreDatabase;

namespace HandMadeApi.Models.DTO.Products
{
    public class OrderProductDto
    {
        public DateTime? OrderDateTime { get; set; }= DateTime.Now;
        public string? Phone { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Note { get; set; }
        public bool? Paid { get; set; }= false;
        public int Quantity { get; set; }
        public Product product { get; set; }
    }
}
