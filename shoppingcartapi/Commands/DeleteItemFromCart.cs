namespace shoppingcartapi.Commands
{
    public class DeleteItemFromCart
    {
        public string UserId { get; }
        public string ProductId { get; }
        public int Amount { get; }

        public DeleteItemFromCart(string userId,string productId,int amount)
        {
            UserId = userId;
            ProductId = productId;
            Amount = amount;
        }
    }
}