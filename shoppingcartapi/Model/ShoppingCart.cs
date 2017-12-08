using System;
using System.Collections.Generic;

namespace shoppingcartapi.Model
{
    public class ShoppingCart
    {
        public List<ShoppingCartItem> Items { get; set; }
        string UserId { get; set; }
    }
}
