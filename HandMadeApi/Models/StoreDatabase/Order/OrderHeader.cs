using System.ComponentModel.DataAnnotations.Schema;

namespace HandMadeApi.Models.StoreDatabase
{
    public class OrderHeader
    {
        public int ID { get; set; }
        [ForeignKey("Client")]
        public string ClientID { get; set; }
        public DateTime? OrderDateTime { get; set; }
        public string? Phone { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Note { get; set; }
        public bool? Paid { get; set; }
        public virtual Client? Client { get; set; }
        public virtual ICollection<OrderDetails>? OrderDetails { get; set; }



    }
}
