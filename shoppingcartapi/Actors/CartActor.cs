using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Persistence;
using shoppingcartapi.Commands;
using shoppingcartapi.Events;
using shoppingcartapi.Model;

namespace shoppingcartapi.Actors
{
    class CartState
    {
        public CartState(string userId)
        {
            UserId = userId;
            Items = new List<ShoppingCartItem>();
        }

        public List<ShoppingCartItem> Items { get; set; }
        public string UserId { get;}
    }

    internal class CartActor : ReceivePersistentActor
    {
        
        private CartState _state { get; set; }


        public override string PersistenceId
        {
            get
            {
                return $"cart-{_state.UserId}";
            }
        }  

        public CartActor(string userId)
        {
            _state = new CartState(userId);

            Command<AddItemToCart>(command =>
            {
                var @event = new ItemAdded(command.UserId, command.ProductId, command.Amount);
                Persist(@event, itemAddedEvent =>
                {
                    _state.Items.Add((new ShoppingCartItem(@event.ProductId, @event.Amount)));
                });
            });

            Command<ViewCart>(command =>
            {
                ShoppingCart cart = new ShoppingCart() { Items = _state.Items };
                this.Sender.Tell(cart);
            });

            Recover<ItemAdded>(message =>
            {
                Console.WriteLine($"replaying ItemAdded event from journal");
                _state.Items.Add(new ShoppingCartItem(message.ProductId, message.Amount));
            });

        }
    }
}
