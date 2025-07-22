using Metroit.ChangeTracking;

namespace Metroit.Win.GcSpread.MultiRow.Metroit.ChangeTracking
{
    public interface ITrackingObjectWithState<T> : ITrackingObject<T>, IStateObject where T : class
    {

    }
}
