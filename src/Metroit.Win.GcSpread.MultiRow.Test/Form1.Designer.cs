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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            button8 = new Button();
            fpSpread1 = new FarPoint.Win.Spread.FpSpread(FarPoint.Win.Spread.LegacyBehaviors.None, resources.GetObject("resource1"));
            fpSpread1_Sheet1 = fpSpread1.GetSheet(0);
            ((System.ComponentModel.ISupportInitialize)fpSpread1).BeginInit();
            SuspendLayout();
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
            // fpSpread1
            // 
            fpSpread1.AccessibleDescription = "";
            fpSpread1.Font = new Font("ＭＳ Ｐゴシック", 11F);
            fpSpread1.Location = new Point(12, 157);
            fpSpread1.Name = "fpSpread1";
            fpSpread1.Size = new Size(546, 326);
            fpSpread1.TabIndex = 9;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(570, 495);
            Controls.Add(fpSpread1);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Margin = new Padding(4);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)fpSpread1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
        private FarPoint.Win.Spread.FpSpread fpSpread1;
        private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
    }
}
