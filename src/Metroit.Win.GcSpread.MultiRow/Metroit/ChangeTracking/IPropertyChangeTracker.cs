using Metroit.ChangeTracking;

namespace Metroit.Win.GcSpread.MultiRow.Metroit.ChangeTracking
{
    public interface IPropertyChangeTracker<T> where T : class
    {
        PropertyChangeTracker<T> ChangeTracker { get; }
    }
}
