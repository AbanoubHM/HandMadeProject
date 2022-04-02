namespace HandMadeApi.Models.DTO.Cart
{
    public class CartDetailsDto
    {
        public int CartDetailsID { get; set; }
        public int CartHeaderID { get; set; }
        public virtual CartHeaderDto CartHeader { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
