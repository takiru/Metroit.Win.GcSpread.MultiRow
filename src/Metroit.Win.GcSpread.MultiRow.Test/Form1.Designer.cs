namespace Metroit.Win.GcSpread.MultiRow.Test
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer1 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer2 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            metFpSpread1 = new MetFpSpread();
            metFpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            button8 = new Button();
            ((System.ComponentModel.ISupportInitialize)metFpSpread1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)metFpSpread1_Sheet1).BeginInit();
            SuspendLayout();
            // 
            // metFpSpread1
            // 
            metFpSpread1.AccessibleDescription = "metFpSpread1, Sheet1, Row 0, Column 0";
            metFpSpread1.AutoOpenDropDown = true;
            metFpSpread1.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            metFpSpread1.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.ArrowColor = Color.FromArgb(121, 121, 121);
            flatScrollBarRenderer1.BackColor = Color.FromArgb(255, 255, 255);
            flatScrollBarRenderer1.BorderActiveColor = Color.FromArgb(171, 171, 171);
            flatScrollBarRenderer1.BorderColor = Color.FromArgb(171, 171, 171);
            flatScrollBarRenderer1.TrackBarBackColor = Color.FromArgb(219, 219, 219);
            metFpSpread1.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            metFpSpread1.Location = new Point(12, 158);
            metFpSpread1.Margin = new Padding(4);
            metFpSpread1.MessageBoxCaption = "SPREADデザイナ";
            metFpSpread1.Name = "metFpSpread1";
            metFpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] { metFpSpread1_Sheet1 });
            metFpSpread1.Size = new Size(542, 328);
            metFpSpread1.TabIndex = 0;
            metFpSpread1.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            metFpSpread1.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.ArrowColor = Color.FromArgb(121, 121, 121);
            flatScrollBarRenderer2.BackColor = Color.FromArgb(255, 255, 255);
            flatScrollBarRenderer2.BorderActiveColor = Color.FromArgb(171, 171, 171);
            flatScrollBarRenderer2.BorderColor = Color.FromArgb(171, 171, 171);
            flatScrollBarRenderer2.TrackBarBackColor = Color.FromArgb(219, 219, 219);
            metFpSpread1.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            // 
            // metFpSpread1_Sheet1
            // 
            metFpSpread1_Sheet1.Reset();
            metFpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            metFpSpread1_Sheet1.ColumnCount = 2;
            metFpSpread1_Sheet1.RowCount = 0;
            metFpSpread1_Sheet1.SheetName = "Sheet1";
            metFpSpread1_Sheet1.ActiveColumnIndex = -1;
            metFpSpread1_Sheet1.ActiveRowIndex = -1;
            metFpSpread1_Sheet1.AlternatingRows.Get(0).BackColor = Color.LightBlue;
            metFpSpread1_Sheet1.AlternatingRows.Get(1).BackColor = Color.MistyRose;
            metFpSpread1_Sheet1.ColumnFooter.DefaultStyle.Locked = false;
            metFpSpread1_Sheet1.ColumnFooterSheetCornerStyle.Locked = false;
            metFpSpread1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerDefaultEnhanced";
            metFpSpread1_Sheet1.ColumnHeader.DefaultStyle.Locked = false;
            metFpSpread1_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            metFpSpread1_Sheet1.DefaultStyle.Locked = false;
            metFpSpread1_Sheet1.DefaultStyle.Parent = "";
            metFpSpread1_Sheet1.FilterBar.DefaultStyle.Locked = false;
            metFpSpread1_Sheet1.FilterBar.DefaultStyle.Parent = "";
            metFpSpread1_Sheet1.FilterBarHeaderStyle.Locked = false;
            metFpSpread1_Sheet1.FilterBarHeaderStyle.Parent = "";
            metFpSpread1_Sheet1.Protect = true;
            metFpSpread1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            metFpSpread1_Sheet1.RowHeader.DefaultStyle.Locked = false;
            metFpSpread1_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            metFpSpread1_Sheet1.SheetCornerStyle.Locked = false;
            metFpSpread1_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            metFpSpread1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(167, 23);
            button1.TabIndex = 1;
            button1.Text = "MultiRowSheetとして初期化";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(93, 41);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 2;
            button2.Text = "行追加";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(12, 128);
            button3.Name = "button3";
            button3.Size = new Size(123, 23);
            button3.TabIndex = 3;
            button3.Text = "行のオブジェクトを取得";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(12, 99);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 4;
            button4.Text = "行削除";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(141, 128);
            button5.Name = "button5";
            button5.Size = new Size(123, 23);
            button5.TabIndex = 5;
            button5.Text = "行の状態を取得";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new Point(12, 41);
            button6.Name = "button6";
            button6.Size = new Size(75, 23);
            button6.TabIndex = 6;
            button6.Text = "空の行追加";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.Location = new Point(12, 70);
            button7.Name = "button7";
            button7.Size = new Size(75, 23);
            button7.TabIndex = 7;
            button7.Text = "値変更";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.Location = new Point(93, 99);
            button8.Name = "button8";
            button8.Size = new Size(75, 23);
            button8.TabIndex = 8;
            button8.Text = "リセット";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(570, 495);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(metFpSpread1);
            Margin = new Padding(4);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)metFpSpread1).EndInit();
            ((System.ComponentModel.ISupportInitialize)metFpSpread1_Sheet1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private MetFpSpread metFpSpread1;
        private FarPoint.Win.Spread.SheetView metFpSpread1_Sheet1;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
    }
}
