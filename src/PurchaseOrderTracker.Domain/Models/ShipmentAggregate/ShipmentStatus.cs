using System.Collections.Generic;
using System.Dynamic;
using System.Resources;
using Stateless;

// To export to DOT graph format use 'string graph = state.ToDotGraph();'
// then visualise in a graphing tool such as http://www.webgraphviz.com
namespace PurchaseOrderTracker.Domain.Models.ShipmentAggregate
{
    public class ShipmentStatus : ValueObject
    {
        public enum State
        {
            Open,
            AwaitingShipping,
            Shipped,
            Delivered
        }

        public enum Trigger
        {
            AwaitingShipping,
            Shipped,
            Delivered
        }

        private StateMachine<State, Trigger> _machine;
        private State _state;

        public ShipmentStatus()
        {
            ConfigureStateMachine();
        }

        private void ConfigureStateMachine(State initialState = State.Open)
        {
            _state = initialState;
            _machine = new StateMachine<State, Trigger>(
                () => _state,
                s => _state = s);

            _machine.Configure(State.Open)
                .Permit(Trigger.AwaitingShipping, State.AwaitingShipping)
                .Permit(Trigger.Shipped, State.Shipped);

            _machine.Configure(State.AwaitingShipping)
                .Permit(Trigger.Shipped, State.Shipped);

            _machine.Configure(State.Shipped)
                .Permit(Trigger.Delivered, State.Delivered);
        }

        public State CurrentState
        {
            get => _machine.State;
            private set => ConfigureStateMachine(value);
        }

        public IEnumerable<Trigger> PermittedTriggers => _machine.PermittedTriggers;

        public void Fire(Trigger trigger)
        {
            _machine.Fire(trigger);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return CurrentState;
        }
    }
}