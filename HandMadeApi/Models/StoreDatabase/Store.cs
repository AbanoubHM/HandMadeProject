using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandMadeApi.Models.StoreDatabase
{
    public class Store
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public string ID { get; set; }
        public string VendorName { get; set; }
        public string Phone { get; set; }
        public string StoreName { get; set; }
        public string? StoreImg { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public virtual ICollection<Product>? Products { get; set; }


    }
}
