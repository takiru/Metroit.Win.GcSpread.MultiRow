using CommunityToolkit.Mvvm.ComponentModel;
using Metroit.Annotations;
using Metroit.ChangeTracking;
using System.ComponentModel;

namespace Metroit.Win.GcSpread.MultiRow.Metroit.ChangeTracking
{
    /// <summary>
    /// 変更追跡が可能なオブジェクトを提供します。
    /// </summary>
    /// <typeparam name="T">変更追跡を行うクラス。</typeparam>
    public class TrackingObject<T> : ObservableObject, IPropertyChangeTracker<TrackingObject<T>> where T : class
    {
        private PropertyChangeTracker<TrackingObject<T>> _changeTracker;

        /// <summary>
        /// 変更追跡を取得します。
        /// </summary>
        [NoTracking]
        public PropertyChangeTracker<TrackingObject<T>> ChangeTracker => _changeTracker;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public TrackingObject()
        {
            _changeTracker = new PropertyChangeTracker<TrackingObject<T>>(this);
            PropertyChanged += ChangesObservableObject_PropertyChanged;
        }

        /// <summary>
        /// 変更通知が行われたプロパティまたはフィールドを追跡する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangesObservableObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _changeTracker.TrackingProperty(e.PropertyName);
        }
    }
}
