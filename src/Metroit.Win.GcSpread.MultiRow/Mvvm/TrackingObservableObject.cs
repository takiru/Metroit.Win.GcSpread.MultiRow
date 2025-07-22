using CommunityToolkit.Mvvm.ComponentModel;
using Metroit.Annotations;
using Metroit.ChangeTracking;
using Metroit.Win.GcSpread.MultiRow.Metroit.ChangeTracking;
using System.ComponentModel;

namespace Metroit.CommunityToolkit.Mvvm
{
    /// <summary>
    /// 変更追跡が可能なオブジェクトを提供します。
    /// </summary>
    public class TrackingObservableObject<T> : ObservableObject, IPropertyChangeTracker<TrackingObservableObject<T>> where T : class
    {
        private PropertyChangeTracker<TrackingObservableObject<T>> _propertyValueTracker;

        /// <summary>
        /// 変更追跡を取得します。
        /// </summary>
        [NoTracking]
        public PropertyChangeTracker<TrackingObservableObject<T>> ChangeTracker => _propertyValueTracker;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public TrackingObservableObject()
        {
            _propertyValueTracker = new PropertyChangeTracker<TrackingObservableObject<T>>(this);

            PropertyChanged += ChangesObservableObject_PropertyChanged;
        }

        /// <summary>
        /// 変更通知が行われたプロパティまたはフィールドを追跡する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangesObservableObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _propertyValueTracker.TrackingProperty(e.PropertyName);
        }
    }
}
