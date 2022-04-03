namespace HandMadeApi.Models.DTO.Category
{
    public class DTOCategoryProducts
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductImage { get; set; }
        public int Price { get; set; }
        public int? SaleValue { get; set; }
    }
}
