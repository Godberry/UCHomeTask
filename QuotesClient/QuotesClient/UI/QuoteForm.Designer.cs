namespace QuotesClient
{
    partial class QuoteForm : Form
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
        private void InitializeComponent ()
        {
            dataGridView1 = new DataGridView ();
            stockTextBox = new TextBox ();
            label1 = new Label ();
            button1 = new Button ();
            Stock = new DataGridViewTextBoxColumn ();
            Price = new DataGridViewTextBoxColumn ();
            AskPrice = new DataGridViewTextBoxColumn ();
            BidPrice = new DataGridViewTextBoxColumn ();
            HighPrice = new DataGridViewTextBoxColumn ();
            LowPrice = new DataGridViewTextBoxColumn ();
            Volumn = new DataGridViewTextBoxColumn ();
            Date = new DataGridViewTextBoxColumn ();
            test = new DataGridViewTextBoxColumn ();
            Delete = new DataGridViewButtonColumn ();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit ();
            SuspendLayout ();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange (new DataGridViewColumn[] { Stock, Price, AskPrice, BidPrice, HighPrice, LowPrice, Volumn, Date, test, Delete });
            dataGridView1.Location = new Point (12, 12);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.Size = new Size (743, 426);
            dataGridView1.TabIndex = 0;
            // 
            // stockTextBox
            // 
            stockTextBox.Location = new Point (801, 13);
            stockTextBox.Name = "stockTextBox";
            stockTextBox.Size = new Size (106, 23);
            stockTextBox.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point (761, 16);
            label1.Name = "label1";
            label1.Size = new Size (34, 15);
            label1.TabIndex = 2;
            label1.Text = "商品:";
            // 
            // button1
            // 
            button1.Location = new Point (761, 42);
            button1.Name = "button1";
            button1.Size = new Size (154, 23);
            button1.TabIndex = 3;
            button1.Text = "新增";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Stock
            // 
            Stock.HeaderText = "商品";
            Stock.Name = "Stock";
            Stock.ReadOnly = true;
            Stock.Width = 70;
            // 
            // Price
            // 
            Price.HeaderText = "價格";
            Price.Name = "Price";
            Price.ReadOnly = true;
            Price.Width = 70;
            // 
            // AskPrice
            // 
            AskPrice.HeaderText = "買價";
            AskPrice.Name = "AskPrice";
            AskPrice.ReadOnly = true;
            AskPrice.Width = 70;
            // 
            // BidPrice
            // 
            BidPrice.HeaderText = "賣價";
            BidPrice.Name = "BidPrice";
            BidPrice.ReadOnly = true;
            BidPrice.Width = 70;
            // 
            // HighPrice
            // 
            HighPrice.HeaderText = "最高價";
            HighPrice.Name = "HighPrice";
            HighPrice.ReadOnly = true;
            HighPrice.Width = 70;
            // 
            // LowPrice
            // 
            LowPrice.HeaderText = "最低價";
            LowPrice.Name = "LowPrice";
            LowPrice.ReadOnly = true;
            LowPrice.Width = 70;
            // 
            // Volumn
            // 
            Volumn.HeaderText = "成交量";
            Volumn.Name = "Volumn";
            Volumn.ReadOnly = true;
            // 
            // Date
            // 
            Date.HeaderText = "時間";
            Date.Name = "Date";
            Date.ReadOnly = true;
            Date.Width = 120;
            // 
            // test
            // 
            test.HeaderText = "延遲";
            test.Name = "test";
            test.ReadOnly = true;
            test.Width = 60;
            // 
            // Delete
            // 
            Delete.HeaderText = "";
            Delete.Name = "Delete";
            Delete.ReadOnly = true;
            Delete.Text = "移除";
            Delete.UseColumnTextForButtonValue = true;
            Delete.Visible = false;
            Delete.Width = 50;
            // 
            // QuoteForm
            // 
            AutoScaleDimensions = new SizeF (7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size (921, 450);
            Controls.Add (button1);
            Controls.Add (label1);
            Controls.Add (stockTextBox);
            Controls.Add (dataGridView1);
            Name = "QuoteForm";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit ();
            ResumeLayout (false);
            PerformLayout ();
        }

        #endregion

        private DataGridView dataGridView1;
        private TextBox stockTextBox;
        private Label label1;
        private Button button1;
        private DataGridViewTextBoxColumn Stock;
        private DataGridViewTextBoxColumn Price;
        private DataGridViewTextBoxColumn AskPrice;
        private DataGridViewTextBoxColumn BidPrice;
        private DataGridViewTextBoxColumn HighPrice;
        private DataGridViewTextBoxColumn LowPrice;
        private DataGridViewTextBoxColumn Volumn;
        private DataGridViewTextBoxColumn Date;
        private DataGridViewTextBoxColumn test;
        private DataGridViewButtonColumn Delete;
    }
}
