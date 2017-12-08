using System;
using Akka.Actor;
using Akka.Persistence;
using shoppingcartapi.Commands;

namespace shoppingcartapi.Actors
{
    internal class CartCoordinatorActor : ReceivePersistentActor
    {
        public override string PersistenceId { get; } = "cartcoordinator";

        public CartCoordinatorActor()
        {
            Command<CreateCart>(command =>
            {
                var CartActor = Context.Child($"cart-{command.UserId}");
                if(CartActor is Nobody)
                {
                    Context.ActorOf(Props.Create(() => new CartActor(command.UserId)),$"cart-{command.UserId}");
                }
            });
        }
    }
}