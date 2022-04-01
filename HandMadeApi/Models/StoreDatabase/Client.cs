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
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Phone { get; set; }

        public ICollection<OrderHeader>? Orders { get; set; }
        public ICollection<ProductRate>? ProductRates { get; set; }



    }
}
