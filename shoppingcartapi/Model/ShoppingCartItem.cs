using System;
namespace shoppingcartapi.Model
{
    public class ShoppingCartItem
    {
        public string ProductId { get; }
        public int Amount { get; set; }

        public ShoppingCartItem(string productId, int amount)
        {
            ProductId = productId;
            Amount = amount;
        }
    }
}
