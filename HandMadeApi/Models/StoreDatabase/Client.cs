using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandMadeApi.Models.StoreDatabase
{
    public class Client
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public string ID { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public int? Age { get; set; }
        public string? Address { get; set; }

        public ICollection<Order>? Orders { get; set; }
        public ICollection<ProductRate>? ProductRates { get; set; }



    }
}
