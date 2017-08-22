using System.Collections.Generic;
using Stateless;

// To export to DOT graph format use 'string graph = state.ToDotGraph();'
// then visualise in a graphing tool such as http://www.webgraphviz.com
namespace PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate
{
    public class PurchaseOrderStatus : ValueObject
    {
        public enum State
        {
            Draft,
            PendingApproval,
            Approved,
            Shipped,
            Delivered,
            Cancelled
        }

        public enum Trigger
        {
            PendingApproval,
            Approved,
            Shipped,
            Delivered,
            Cancelled
        }

        private StateMachine<State, Trigger> _machine;
        private State _state;

        public PurchaseOrderStatus()
        {
            ConfigureStateMachine();
        }

        public bool IsStateBeforeApproved => CurrentState == State.Draft
                                             || CurrentState == State.PendingApproval;

        public State CurrentState
        {
            get => _machine.State;
            private set => ConfigureStateMachine(value);
        }

        public IEnumerable<Trigger> PermittedTriggers => _machine.PermittedTriggers;

        private void ConfigureStateMachine(State initialState = State.Draft)
        {
            _state = initialState;
            _machine = new StateMachine<State, Trigger>(
                () => _state,
                s => _state = s);

            _machine.Configure(State.Draft)
                .Permit(Trigger.PendingApproval, State.PendingApproval)
                .Permit(Trigger.Approved, State.Approved)
                .Permit(Trigger.Cancelled, State.Cancelled);

            _machine.Configure(State.PendingApproval)
                .Permit(Trigger.Approved, State.Approved)
                .Permit(Trigger.Cancelled, State.Cancelled);

            _machine.Configure(State.Approved)
                .Permit(Trigger.Shipped, State.Shipped)
                .Permit(Trigger.Cancelled, State.Cancelled);

            _machine.Configure(State.Shipped)
                .Permit(Trigger.Delivered, State.Delivered)
                .Permit(Trigger.Cancelled, State.Cancelled);
        }

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