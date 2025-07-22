using Metroit.Annotations;
using Metroit.CommunityToolkit.Mvvm;
using System.Linq;

namespace Metroit.Win.GcSpread.MultiRow.Collections.Generic
{
    public class TrackingObservableObjectEx<T> : TrackingObservableObject<T>, IStateObject where T : class
    {
        private ItemState _state = ItemState.New;

        [NoTracking]
        public ItemState State => _state;

        public void ChangeState(ItemState state)
        {
            _state = state;
        }

        [NoTracking]
        public bool IsChanged { get; set; }

        public TrackingObservableObjectEx()
        {
            ChangeTracker.SomethingValueChanged = (isChanged) => IsChanged = isChanged;
        }
    }
}
