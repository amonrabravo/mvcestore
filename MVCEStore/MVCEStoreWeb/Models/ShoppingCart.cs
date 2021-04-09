using MvcEStoreData;
using System.Collections.Generic;
using System.Linq;

namespace MVCEStoreWeb.Models
{
    public class ShoppingCart
    {
        public IList<ShoppingCartItem> Items { get; private set; } = new List<ShoppingCartItem>();

        public void Add(Product product, int quantity = 1)
        {
            var item = Items.SingleOrDefault(p => p.ProductId == product.Id);
            if (item != null)
                item.Quantity += quantity;
            else
                Items.Add(new ShoppingCartItem { ProductId = product.Id, Price = product.DiscountedPrice, Quantity = quantity });
        }

        public void Remove(int productId)
        {
            var item = Items.SingleOrDefault(p => p.ProductId == productId);
            Items.Remove(item);
        }

        public void Clear()
        {
            Items.Clear();
        }

        public decimal GrandTotal => Items.Sum(p => p.Amount);
    }
}
