using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using Metroit.Collections.Generic;

namespace Metroit.Win.GcSpread.MultiRow.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            DefaultSheetStyleModel styleModel;
            styleModel = (DefaultSheetStyleModel)fpSpread1.ActiveSheet.Models.Style;
            styleModel.AltRowCount = 2;

            var sInfo = new StyleInfo();
            sInfo.BackColor = Color.LightBlue;
            styleModel.SetDirectAltRowInfo(0, sInfo);

            var sInfo2 = new StyleInfo();
            sInfo2.BackColor = Color.LightYellow;
            styleModel.SetDirectAltRowInfo(1, sInfo2);
        }

        private MultiRowSheet<ObservableRecord> _multiRowSheet;
        private TrackingList<ObservableRecord> _list = new TrackingList<ObservableRecord>();
        //private MultiRowSheet<PlainRecord> _multiRowSheet;
        //private TrackingList<PlainRecord> _list = new TrackingList<PlainRecord>();

        private void button1_Click(object sender, EventArgs e)
        {
            _multiRowSheet = new MultiRowSheet<ObservableRecord>(fpSpread1.ActiveSheet, 2, _list);
            //_multiRowSheet = new MultiRowSheet<PlainRecord>(fpSpread1.ActiveSheet, 2, _list);
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
            ((ObservableRecord)fpSpread1.ActiveSheet.ActiveRow.Tag).Item1 = "値変更";
            //((PlainRecord)fpSpread1.ActiveSheet.ActiveRow.Tag).Item1 = "値変更";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _list.Remove((ObservableRecord)fpSpread1.ActiveSheet.ActiveRow.Tag);
            //_list.Remove((PlainRecord)fpSpread1.ActiveSheet.ActiveRow.Tag);
            
            foreach (var removed in _list.Removed)
            {
                MessageBox.Show($"Removed: {removed}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var record = _list[_multiRowSheet.GetItemIndex(fpSpread1.ActiveSheet.ActiveRowIndex)];
            MessageBox.Show($"{record}");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var record = _list[_multiRowSheet.GetItemIndex(fpSpread1.ActiveSheet.ActiveRowIndex)];
            MessageBox.Show($"{record.State}");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            _list.Clear();
        }
    }
}
