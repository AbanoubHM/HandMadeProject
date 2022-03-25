namespace HandMadeApi.Models.StoreDatabase
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
