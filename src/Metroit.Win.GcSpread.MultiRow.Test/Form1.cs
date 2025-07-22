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

        private MultiRowSheet<Record> _multiRowSheet;
        private TrackingList<Record> _list = new TrackingList<Record>();
        //private MultiRowSheet2<Record> _multiRowSheet;
        //private TrackingList2<Record> _list = new TrackingList2<Record>();

        private void button1_Click(object sender, EventArgs e)
        {
            _multiRowSheet = new MultiRowSheet<Record>(metFpSpread1.ActiveSheet, 2, _list);
            //_multiRowSheet = new MultiRowSheet2<Record>(metFpSpread1.ActiveSheet, 2, _list);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //_multiRowSheet.AddRow(new Record("Item1", "Item2", "Item3", "Item4"));
            var r = new Record("Item1", "Item2", "Item3", "Item4");
            //r.ChangeTracker.Reset();
            _list.Add(r);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //_multiRowSheet.AddRow(new Record());
            _list.AddNew();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //_multiRowSheet.Rows[_multiRowSheet.GetItemIndex(metFpSpread1.ActiveSheet.ActiveRowIndex)].Item1 = "値変更";
            //((Record)metFpSpread1.ActiveSheet.ActiveRow.Tag).Item1 = "値変更";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //_multiRowSheet.RemoveRow(metFpSpread1.ActiveSheet.ActiveRowIndex);
            _list.Remove((Record)metFpSpread1.ActiveSheet.ActiveRow.Tag);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var record = _multiRowSheet.Rows[_multiRowSheet.GetItemIndex(metFpSpread1.ActiveSheet.ActiveRowIndex)];
            MessageBox.Show($"{record}");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var record = _multiRowSheet.Rows[_multiRowSheet.GetItemIndex(metFpSpread1.ActiveSheet.ActiveRowIndex)];
            MessageBox.Show($"{record.State}");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //_list.Clear();
        }
    }
}
