using HandMadeApi.Models.StoreDatabase;

namespace HandMadeApi.Models.DTO.Products {
    public class CartProductDto {
        public int Quantity { get; set; }
        public Product product { get; set; }
    }
}
