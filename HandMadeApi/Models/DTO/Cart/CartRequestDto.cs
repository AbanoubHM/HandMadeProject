namespace HandMadeApi.Models.DTO.Cart {
    public class CartRequestDto {
        public string ClientId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
