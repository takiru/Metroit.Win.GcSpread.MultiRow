using CommunityToolkit.Mvvm.ComponentModel;
using Metroit.CommunityToolkit.Mvvm;
using Metroit.Win.GcSpread.MultiRow.Metroit.ChangeTracking;

namespace Metroit.Win.GcSpread.MultiRow.Test
{
    /// <summary>
    /// ObservableObject を使わない場合のレコード
    /// </summary>
    public class PlainRecord : TrackingObjectWithState<PlainRecord>
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

        public PlainRecord() { }
        public PlainRecord(string item1, string item2, string item3, string item4)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
        }
        public override string ToString()
        {
            return $"{Item1}, {Item2}, {Item3}, {Item4}";
        }
    }


    /// <summary>
    /// ObservableObject を使ったレコード
    /// </summary>
    public partial class ObservableRecord : TrackingObservableObjectWithState<ObservableRecord>
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

        public ObservableRecord() : base() { }

        public ObservableRecord(string item1, string item2, string item3, string item4) : base()
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
        }

        public override string ToString()
        {
            return $"{Item1}, {Item2}, {Item3}, {Item4}";
        }
    }
}
