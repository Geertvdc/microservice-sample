namespace shoppingcartapi.Commands
{
    public class CreateCart
    {
        public string UserId { get; }

        public CreateCart(string userId)
        {
            UserId = userId;
        }
    }
}