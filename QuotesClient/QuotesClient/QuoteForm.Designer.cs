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
            Stock = new DataGridViewTextBoxColumn ();
            Price = new DataGridViewTextBoxColumn ();
            AskPrice = new DataGridViewTextBoxColumn ();
            BidPrice = new DataGridViewTextBoxColumn ();
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
            dataGridView1.Columns.AddRange (new DataGridViewColumn[] { Stock, Price, AskPrice, BidPrice, Volumn, Date, test, Delete });
            dataGridView1.Location = new Point (12, 12);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.Size = new Size (793, 426);
            dataGridView1.TabIndex = 0;
            // 
            // Stock
            // 
            Stock.HeaderText = "商品";
            Stock.Name = "Stock";
            Stock.ReadOnly = true;
            // 
            // Price
            // 
            Price.HeaderText = "價格";
            Price.Name = "Price";
            Price.ReadOnly = true;
            // 
            // AskPrice
            // 
            AskPrice.HeaderText = "買價";
            AskPrice.Name = "AskPrice";
            AskPrice.ReadOnly = true;
            // 
            // BidPrice
            // 
            BidPrice.HeaderText = "賣價";
            BidPrice.Name = "BidPrice";
            BidPrice.ReadOnly = true;
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
            // 
            // test
            // 
            test.HeaderText = "延遲";
            test.Name = "test";
            test.ReadOnly = true;
            // 
            // Delete
            // 
            Delete.HeaderText = "";
            Delete.Name = "Delete";
            Delete.ReadOnly = true;
            Delete.Text = "移除";
            Delete.UseColumnTextForButtonValue = true;
            Delete.Width = 50;
            // 
            // QuoteForm
            // 
            AutoScaleDimensions = new SizeF (7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size (902, 450);
            Controls.Add (dataGridView1);
            Name = "QuoteForm";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit ();
            ResumeLayout (false);
        }

        #endregion

        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn Stock;
        private DataGridViewTextBoxColumn Price;
        private DataGridViewTextBoxColumn AskPrice;
        private DataGridViewTextBoxColumn BidPrice;
        private DataGridViewTextBoxColumn Volumn;
        private DataGridViewTextBoxColumn Date;
        private DataGridViewTextBoxColumn test;
        private DataGridViewButtonColumn Delete;
    }
}
