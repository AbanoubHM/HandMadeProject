using System.ComponentModel.DataAnnotations.Schema;

namespace HandMadeApi.Models.StoreDatabase
{
    public class CartHeader
    {
        public int ID { get; set; }
        [ForeignKey("Client")]
        public string? ClientID { get; set; }
        public virtual Client? Client { get; set; }
        public virtual ICollection<CartDetails>? CartDetails { get; set; }

    }
}
