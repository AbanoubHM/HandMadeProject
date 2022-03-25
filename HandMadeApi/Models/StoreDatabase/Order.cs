using System.ComponentModel.DataAnnotations.Schema;

namespace HandMadeApi.Models.StoreDatabase
{
    public class Order
    {
        public int ID { get; set; }
        [ForeignKey("Client")]
        public string ClientID { get; set; }
        public DateTime OrderDateTime { get; set; }
        public string? Address { get; set; }
        public string? Note { get; set; }
        public ICollection<Product> Products { get; set; }
        public virtual Client Client { get; set; }



    }
}
