using System;
namespace shoppingcartapi.Events
{
    public class ItemAdded
    {
        public string UserId { get; }
        public string ProductId { get; }
        public int Amount { get; }

        public ItemAdded(string userId, string productId, int amount)
        {
            UserId = userId;
            ProductId = productId;
            Amount = amount;
        }
    }
}
