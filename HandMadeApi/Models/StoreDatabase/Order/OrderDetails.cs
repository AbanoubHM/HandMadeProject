using System.ComponentModel.DataAnnotations.Schema;

namespace HandMadeApi.Models.StoreDatabase
{
    public class OrderDetails
    {
        public int ID { get; set; }
        [ForeignKey("OrderHeader")]
        public int OrderHeaderID { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual Product? Product { get; set; }
    }
}
