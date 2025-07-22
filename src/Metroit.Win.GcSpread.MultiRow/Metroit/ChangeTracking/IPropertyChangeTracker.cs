using Metroit.ChangeTracking;

namespace Metroit.Win.GcSpread.MultiRow.Metroit.ChangeTracking
{
    public interface IPropertyChangeTracker
    {
        PropertyChangeTracker ChangeTrackerObject { get; }
    }

    public interface IPropertyChangeTracker<T> : IPropertyChangeTracker where T : class
    {
        PropertyChangeTracker<T> ChangeTracker { get; }
    }
}
