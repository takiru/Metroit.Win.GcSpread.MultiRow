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

        private void button1_Click(object sender, EventArgs e)
        {
            _multiRowSheet = new MultiRowSheet<Record>(metFpSpread1.ActiveSheet, 2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _multiRowSheet.AddRow(new Record("Item1", "Item2", "Item3", "Item4"));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            _multiRowSheet.AddRow(new Record());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            _multiRowSheet.Rows[_multiRowSheet.GetItemIndex(metFpSpread1.ActiveSheet.ActiveRowIndex)].Item1 = "ílïœçX";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _multiRowSheet.RemoveRow(metFpSpread1.ActiveSheet.ActiveRowIndex);
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
    }
}
