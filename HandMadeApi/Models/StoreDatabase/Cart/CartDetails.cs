using System.ComponentModel.DataAnnotations.Schema;

namespace HandMadeApi.Models.StoreDatabase
{
    public class CartDetails
    {
        public int ID { get; set; }
        [ForeignKey("CartHeader")]
        public int CartHeaderID { get; set; }
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
    }
}
