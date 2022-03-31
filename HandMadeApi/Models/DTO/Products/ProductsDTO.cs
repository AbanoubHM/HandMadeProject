namespace HandMadeApi.Models.DTO.Products
{
    public class ProductsDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public int? SaleValue { get; set; }
        public int? Quantity { get; set; }
        public int? PreparationDays { get; set; }
        public int CategoryID { get; set; }
        public string StoreID { get; set; }
    }
}

