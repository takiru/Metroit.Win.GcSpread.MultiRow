using CommunityToolkit.Mvvm.ComponentModel;
using Metroit.Annotations;
using Metroit.ChangeTracking;
using System.ComponentModel;

namespace Metroit.CommunityToolkit.Mvvm
{
    /// <summary>
    /// 変更追跡が可能なオブジェクトを提供します。
    /// </summary>
    public class TrackingObservableValidator : ObservableValidator
    {
        private PropertyChangeTracker<TrackingObservableValidator> _propertyValueTracker;

        /// <summary>
        /// 変更追跡を取得します。
        /// </summary>
        [NoTracking]
        public PropertyChangeTracker<TrackingObservableValidator> ChangeTracker => _propertyValueTracker;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public TrackingObservableValidator()
        {
            _propertyValueTracker = new PropertyChangeTracker<TrackingObservableValidator>(this);

            PropertyChanged += ChangesObservableValidator_PropertyChanged;
        }

        /// <summary>
        /// 変更通知が行われたプロパティまたはフィールドを追跡する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangesObservableValidator_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _propertyValueTracker.TrackingProperty(e.PropertyName);
        }
    }
}
