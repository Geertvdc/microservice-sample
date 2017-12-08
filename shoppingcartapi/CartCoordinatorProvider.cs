using System;
using Akka.Actor;
using shoppingcartapi.Actors;

namespace shoppingcartapi
{
    public class CartCoordinatorProvider
    {
        private IActorRef CartCoordinatorActor { get; set; }

        public CartCoordinatorProvider(ActorSystem actorsystem)
        {
            this.CartCoordinatorActor = actorsystem.ActorOf<CartCoordinatorActor>("cartcoordinator");
        }

        public IActorRef Get()
        {
            return this.CartCoordinatorActor;
        }
    }
}
