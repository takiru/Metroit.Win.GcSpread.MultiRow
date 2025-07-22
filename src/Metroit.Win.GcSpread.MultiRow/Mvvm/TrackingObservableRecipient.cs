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
    /// <typeparam name="T">変更追跡を行うクラス。</typeparam>
    public class TrackingObservableRecipient<T> : ObservableRecipient, IPropertyChangeTracker<TrackingObservableRecipient<T>> where T : class
    {
        private PropertyChangeTracker<TrackingObservableRecipient<T>> _changeTracker;

        /// <summary>
        /// 変更追跡を取得します。
        /// </summary>
        [NoTracking]
        public PropertyChangeTracker<TrackingObservableRecipient<T>> ChangeTracker => _changeTracker;

        /// <summary>
        /// 変更追跡を取得します。
        /// </summary>
        [NoTracking]
        PropertyChangeTracker IPropertyChangeTracker.ChangeTracker => ChangeTracker;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public TrackingObservableRecipient() : base()
        {
            _changeTracker = new PropertyChangeTracker<TrackingObservableRecipient<T>>(this);

            PropertyChanged += ChangesObservableRecipient_PropertyChanged;
        }

        /// <summary>
        /// 変更通知が行われたプロパティまたはフィールドを追跡する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangesObservableRecipient_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _changeTracker.TrackingProperty(e.PropertyName);
        }
    }
}
