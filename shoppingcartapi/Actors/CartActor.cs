using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Persistence;
using shoppingcartapi.Commands;
using shoppingcartapi.Events;
using shoppingcartapi.Model;
using System.Linq;

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
                    AddItemToState(@event);
                });
            });

            Command<DeleteItemFromCart>(command =>
            {
                var @event = new ItemDeleted(command.UserId, command.ProductId, command.Amount);
                Persist(@event, itemDeletedEvent =>
                {
                    DeleteItemFromState(@event);
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
                AddItemToState(message);
            });

            Recover<ItemDeleted>(message =>
            {
                Console.WriteLine($"replaying ItemDeleted event from journal");
                DeleteItemFromState(message);
            });
        }

        public void DeleteItemFromState(ItemDeleted itemDeleted)
        {
            var item = _state.Items.FirstOrDefault(i => i.ProductId == itemDeleted.ProductId);
            if(item == null)
            {
                throw new Exception("Trying to delete a product that does not exist");
            }
            if(item.Amount < itemDeleted.Amount)
            {
                throw new Exception("Trying to delete a larger amount from basket than possible");
            }

            item.Amount -= itemDeleted.Amount;
        }

        public void AddItemToState(ItemAdded itemAdded)
        {
            var item = _state.Items.FirstOrDefault(i => i.ProductId == itemAdded.ProductId);
            if (item == null)
            {
                _state.Items.Add(new ShoppingCartItem(itemAdded.ProductId,itemAdded.Amount));
            }
            else
            {
                item.Amount += itemAdded.Amount;
            }
        }
    }
}
