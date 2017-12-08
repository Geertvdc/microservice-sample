namespace shoppingcartapi.Commands
{
    public class AddItemToCart
    {
        public string UserId { get; }
        public string ProductId { get; }
        public int Amount { get; }

        public AddItemToCart(string userId,string productId,int amount)
        {
            UserId = userId;
            ProductId = productId;
            Amount = amount;
        }
    }
}