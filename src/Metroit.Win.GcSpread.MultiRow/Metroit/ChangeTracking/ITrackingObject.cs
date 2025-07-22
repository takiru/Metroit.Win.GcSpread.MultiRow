using Metroit.ChangeTracking;

namespace Metroit.Win.GcSpread.MultiRow.Metroit.ChangeTracking
{
    public interface ITrackingObject<T> : IPropertyChangeTracker<T> where T : class
    {

    }
}
