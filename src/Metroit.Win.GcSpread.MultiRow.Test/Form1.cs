using Metroit.Collections.Generic;
using Metroit.Win.GcSpread.MultiRow.Collections.Generic;

namespace Metroit.Win.GcSpread.MultiRow.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private MultiRowSheet<ObservableRecord> _multiRowSheet;
        private TrackingList<ObservableRecord> _list = new TrackingList<ObservableRecord>();
        //private MultiRowSheet<PlainRecord> _multiRowSheet;
        //private TrackingList<PlainRecord> _list = new TrackingList<PlainRecord>();

        private void button1_Click(object sender, EventArgs e)
        {
            _multiRowSheet = new MultiRowSheet<ObservableRecord>(metFpSpread1.ActiveSheet, 3, _list);
            //_multiRowSheet = new MultiRowSheet<PlainRecord>(metFpSpread1.ActiveSheet, 2, _list);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var r = new ObservableRecord("Item1", "Item2", "Item3", "Item4");
            //var r = new PlainRecord("Item1", "Item2", "Item3", "Item4");
            _list.Add(r);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            _list.AddNew();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ((ObservableRecord)metFpSpread1.ActiveSheet.ActiveRow.Tag).Item1 = "値変更";
            //((PlainRecord)metFpSpread1.ActiveSheet.ActiveRow.Tag).Item1 = "値変更";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _list.Remove((ObservableRecord)metFpSpread1.ActiveSheet.ActiveRow.Tag);
            //_list.Remove((PlainRecord)metFpSpread1.ActiveSheet.ActiveRow.Tag);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var record = _list[_multiRowSheet.GetItemIndex(metFpSpread1.ActiveSheet.ActiveRowIndex)];
            MessageBox.Show($"{record}");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var record = _list[_multiRowSheet.GetItemIndex(metFpSpread1.ActiveSheet.ActiveRowIndex)];
            MessageBox.Show($"{record.State}");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            _list.Clear();
        }
    }
}
