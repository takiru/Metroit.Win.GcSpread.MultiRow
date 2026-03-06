using System.ComponentModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Metroit.ChangeTracking;
using Metroit.ChangeTracking.Generic;
using Metroit.CommunityToolkit.Mvvm.ChangeTracking;
using Metroit.Win.GcSpread.MultiRow.Annotations;

namespace Metroit.Win.GcSpread.MultiRow.Test
{
    /// <summary>
    /// ObservableObject を使わない場合のレコード
    /// </summary>
    public class PlainRecord : StatefulTrackingObject<PlainRecord, PropertyChangeTracker<PlainRecord>>
    {
        private string _item1 = null;

        [MultiRow(0, 0)]
        public string Item1 { get => _item1; set => SetProperty(ref _item1, value); }

        private string _item2 = null;

        [MultiRow(0, 1)]
        public string Item2 { get => _item2; set => SetProperty(ref _item2, value); }

        private string _item3 = null;

        [MultiRow(1, 0)]
        public string Item3 { get => _item3; set => SetProperty(ref _item3, value); }

        private string _item4 = null;

        [MultiRow(1, 1)]
        public string Item4 { get => _item4; set => SetProperty(ref _item4, value); }

        public PlainRecord()
        {
            ChangeTracker.Reset();


            ChangeTracker.TrackingPropertyValueChanged += (sender, e) =>
            {
                Debug.WriteLine($"Non argument constructor TrackingPropertyValueChanged:{e.PropertyName}");
            };
        }

        public PlainRecord(string item1, string item2, string item3, string item4)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;

            ChangeTracker.Reset();

            ChangeTracker.TrackingPropertyValueChanged += (sender, e) =>
            {
                Debug.WriteLine($"Has argument constructor TrackingPropertyValueChanged:{e.PropertyName}");
            };

        }
        public override string ToString()
        {
            return $"{Item1}, {Item2}, {Item3}, {Item4}";
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            Debug.WriteLine(e.PropertyName);
        }
    }


    /// <summary>
    /// ObservableObject を使ったレコード
    /// </summary>
    public partial class ObservableRecord : StatefulTrackingObservableObject<ObservableRecord, PropertyChangeTracker<ObservableRecord>>
    {
        [ObservableProperty]
        [property: MultiRow(0, 0)]
        private string _item1 = null;

        [ObservableProperty]
        [property: MultiRow(0, 1)]
        private string _item2 = null;

        [ObservableProperty]
        [property: MultiRow(1, 0)]
        private string _item3 = null;

        [ObservableProperty]
        [property: MultiRow(1, 1)]
        private string _item4 = null;

        public ObservableRecord() : base()
        {
            ChangeTracker.Reset();


            ChangeTracker.TrackingPropertyValueChanged += (sender, e) =>
            {
                Debug.WriteLine($"Non argument constructor TrackingPropertyValueChanged:{e.PropertyName}");
            };
        }

        public ObservableRecord(string item1, string item2, string item3, string item4) : base()
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;

            ChangeTracker.Reset();

            ChangeTracker.TrackingPropertyValueChanged += (sender, e) =>
            {
                Debug.WriteLine($"Has argument constructor TrackingPropertyValueChanged:{e.PropertyName}");
            };
        }

        public override string ToString()
        {
            return $"{Item1}, {Item2}, {Item3}, {Item4}";
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            Debug.WriteLine(e.PropertyName);
        }
    }
}
