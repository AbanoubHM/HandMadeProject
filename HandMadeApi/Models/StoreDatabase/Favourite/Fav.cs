using System.ComponentModel.DataAnnotations.Schema;

namespace HandMadeApi.Models.StoreDatabase.Favourite
{
    public class Fav
    {
        public int ID { get; set; }
        [ForeignKey("Client")]
        public string UserID { get; set; }
        public virtual Client? Client { get; set; }
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual Product? Product { get; set; }
    }
}
