using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using GrapeCity.Spreadsheet;
using Metroit.Collections.Generic;
using Metroit.Win.GcSpread.Extensions;

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
            var config = new MultiRowSheetConfiguration<ObservableRecord>(fpSpread1.ActiveSheet, 2);
            config.OddBackColor = System.Drawing.Color.LightCyan;
            config.EvenBackColor = System.Drawing.Color.LightYellow;

            // NOTE: Tag の値がオリジナルのオブジェクトだったとき、この設定で任意のプロパティに設定／取得を可能にする。
            //       TagDelivery を設定しない場合、Tag プロパティにはオブジェクトがそのまま設定される。
            //config.TagDelivery = new RowTagDelivery<ObservableRecord>(
            //    (row, item) => row.Tag = item,
            //    (tag) => (ObservableRecord)tag
            //    );

            _multiRowSheet = MultiRowSheet<ObservableRecord>.Start(config, _list);
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
