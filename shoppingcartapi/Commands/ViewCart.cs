namespace shoppingcartapi.Commands
{
    public class ViewCart
    {
        public string UserId { get; }

        public ViewCart(string userId)
        {
            UserId = userId;
        }
    }
}