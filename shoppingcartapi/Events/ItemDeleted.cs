using System;
namespace shoppingcartapi.Events
{
    public class ItemDeleted
    {
        public string UserId { get; }
        public string ProductId { get; }
        public int Amount { get; }

        public ItemDeleted(string userId, string productId, int amount)
        {
            UserId = userId;
            ProductId = productId;
            Amount = amount;
        }
    }
}
