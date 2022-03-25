using System.ComponentModel.DataAnnotations.Schema;

namespace HandMadeApi.Models.StoreDatabase
{
    public class ProductRate
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
        [ForeignKey("Client")]
        public string ClientID { get; set; }
        public virtual Client Client{ get; set; }

    }
}
