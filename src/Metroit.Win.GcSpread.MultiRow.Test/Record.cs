using CommunityToolkit.Mvvm.ComponentModel;
using Metroit.Annotations;
using Metroit.ChangeTracking;
using Metroit.CommunityToolkit.Mvvm;
using Metroit.Win.GcSpread.MultiRow.Metroit.ChangeTracking;
using System.ComponentModel;

namespace Metroit.Win.GcSpread.MultiRow.Test
{
    //public partial class Record : TrackingObservableObjectEx<Record>
    //public partial class Record : TrackingObservableObject<Record>, IStateObject
    //public partial class Record : ObservableObject, IPropertyChangeTracker<Record>, IStateObject
    //public partial class Record : ObservableObject, ITrackingObject<Record>
    //public partial class Record : TrackingObject<Record>
    //public partial class Record : TrackingObservableObject<Record>
    public partial class Record : TrackingObservableObjectWithState<Record>
    {



        //private ItemState _state = ItemState.New;

        //[NoTracking]
        //public ItemState State => _state;

        //private PropertyChangeTracker<Record> _propertyValueTracker;

        //public PropertyChangeTracker<Record> ChangeTracker => _propertyValueTracker;

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

        //public Record()
        //{
        //    _propertyValueTracker = new PropertyChangeTracker<Record>(this);
        //    PropertyChanged += ChangesObservableObject_PropertyChanged;
        //}

        ///// <summary>
        ///// 変更通知が行われたプロパティまたはフィールドを追跡する。
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void ChangesObservableObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    _propertyValueTracker.TrackingProperty(e.PropertyName);
        //}


        public Record() : base()
        {

        }


        public Record(string item1, string item2, string item3, string item4) : base()
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            //NotModifedItem();
        }

        //public void ChangeState(ItemState state)
        //{
        //    _state = state;
        //}

        public override string ToString()
        {
            return $"{Item1}, {Item2}, {Item3}, {Item4}";
        }
    }
}
