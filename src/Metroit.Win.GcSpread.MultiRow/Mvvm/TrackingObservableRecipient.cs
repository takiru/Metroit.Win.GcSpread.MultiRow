using CommunityToolkit.Mvvm.ComponentModel;
using Metroit.Annotations;
using Metroit.ChangeTracking;
using System.ComponentModel;

namespace Metroit.CommunityToolkit.Mvvm
{
    /// <summary>
    /// 変更追跡が可能なオブジェクトを提供します。
    /// </summary>
    public class TrackingObservableRecipient : ObservableRecipient
    {
        private PropertyChangeTracker<TrackingObservableRecipient> _propertyValueTracker;

        /// <summary>
        /// 変更追跡を取得します。
        /// </summary>
        [NoTracking]
        public PropertyChangeTracker<TrackingObservableRecipient> ChangeTracker => _propertyValueTracker;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public TrackingObservableRecipient()
        {
            _propertyValueTracker = new PropertyChangeTracker<TrackingObservableRecipient>(this);

            PropertyChanged += ChangesObservableRecipient_PropertyChanged;
        }

        /// <summary>
        /// 変更通知が行われたプロパティまたはフィールドを追跡する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangesObservableRecipient_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _propertyValueTracker.TrackingProperty(e.PropertyName);
        }
    }
}
