using Metroit.Annotations;
using Metroit.ChangeTracking;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Metroit.Win.GcSpread.MultiRow.Metroit.ChangeTracking
{
    /// <summary>
    /// 変更追跡が可能なオブジェクトを提供します。
    /// </summary>
    /// <typeparam name="T">変更追跡を行うクラス。</typeparam>
    public class TrackingObject<T> : IPropertyChangeTracker<TrackingObject<T>>, INotifyPropertyChanged where T : class
    {
        private PropertyChangeTracker<TrackingObject<T>> _changeTracker;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 変更追跡を取得します。
        /// </summary>
        [NoTracking]
        public PropertyChangeTracker<TrackingObject<T>> ChangeTracker => _changeTracker;

        PropertyChangeTracker IPropertyChangeTracker.ChangeTracker => ChangeTracker;

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public TrackingObject()
        {
            _changeTracker = new PropertyChangeTracker<TrackingObject<T>>(this);
            PropertyChanged += TrackingObject_PropertyChanged;
        }

        /// <summary>
        /// 変更通知が行われたプロパティまたはフィールドを追跡する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrackingObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _changeTracker.TrackingProperty(e.PropertyName);
        }

        protected bool SetProperty<U>(ref U field, U value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<U>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 値変更の通知を行います。
        /// </summary>
        /// <param name="propertyName">プロパティ名。</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
