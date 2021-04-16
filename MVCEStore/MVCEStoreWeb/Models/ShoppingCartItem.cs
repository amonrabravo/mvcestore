using MVCEStoreData;

namespace MVCEStoreWeb.Models
{
    public class ShoppingCartItem
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Amount => Price * Quantity;
        public Product Product { get; set; }
    }

}
