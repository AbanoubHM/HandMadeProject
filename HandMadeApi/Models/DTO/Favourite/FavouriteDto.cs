using HandMadeApi.Models.StoreDatabase;

namespace HandMadeApi.Models.DTO.Favourite
{
    public class FavouriteDto
    {
        public int ProductID { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int Price { get; set; }
        public int? Quantity { get; set; }
    }
}
