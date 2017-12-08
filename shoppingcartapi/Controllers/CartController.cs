using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shoppingcartapi.Commands;
using Akka.Actor;
using shoppingcartapi.Model;

namespace shoppingcartapi.Controllers
{
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        private ActorSystem _cartsystem;
        private IActorRef _cartcoordinator;

        public CartController(ActorSystem cartSystem, CartCoordinatorProvider cartcoordinator)
        {
            _cartsystem = cartSystem;
            _cartcoordinator = cartcoordinator.Get();
        }

        [HttpPut("{customerId}")]
        public IActionResult Put(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                return BadRequest("invalid customerId");
            }

            _cartcoordinator.Tell(new CreateCart(customerId));
            return Ok();
        }

        [HttpGet("{customerId}")]
        public async Task<ShoppingCart> Get(string customerId)
        {
            var cart = _cartsystem.ActorSelection($"/user/cartcoordinator/cart-{customerId}");

            var cartdetails = await cart.Ask<ShoppingCart>(new ViewCart(customerId));
            return cartdetails;
        }
        
        [HttpPut("{customerId}/items")]
        public IActionResult Put(string customerId, [FromBody]AddItemToCart cartItem)
        {
            if(cartItem == null)
            {
                return BadRequest("invalid message");
            }
            if(string.IsNullOrEmpty(customerId) || cartItem.UserId != customerId)
            {
                return BadRequest("invalid customerId");
            }

            var cart = _cartsystem.ActorSelection($"/user/cartcoordinator/cart-{customerId}");

            cart.Tell(cartItem);

            return Ok();
        }
    }
}
