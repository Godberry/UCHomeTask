using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Timer = System.Windows.Forms.Timer;

namespace QuotesClient
{
    public partial class QuoteForm : Form
    {
        private List<Quotes> dataList = new List<Quotes>(); // 儲存最新報價
        private HashSet<int> dirtyRows = new HashSet<int>(); // 記錄需要更新的行索引
        private Timer updateTimer;

        public QuoteForm ()
        {
            InitializeComponent ();
            InitializeDataGridView ();
            InitializeTimer ();
        }

        private void InitializeTimer ()
        {
            updateTimer = new Timer
            {
                Interval = 1
            };
            updateTimer.Tick += (s, e) =>
            {
                if (dirtyRows.Count > 0)
                {
                    foreach (var rowIndex in dirtyRows)
                    {
                        dataGridView1.InvalidateRow (rowIndex); // 僅刷新修改過的行
                    }
                    dirtyRows.Clear ();
                }

            };
            updateTimer.Start ();
        }

        private void InitializeDataGridView ()
        {
            dataGridView1.VirtualMode = true; // 啟用虛擬模式
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.CellValueNeeded += DataGridView1_CellValueNeeded;
        }

        private void DataGridView1_CellValueNeeded (object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataList.Count)
            {
                var quote = dataList[e.RowIndex];

                switch (e.ColumnIndex)
                {
                    case 0: e.Value = quote.Stock; break;
                    case 1: e.Value = quote.Price; break;
                    case 2: e.Value = quote.AskPrice; break;
                    case 3: e.Value = quote.BidPrice; break;
                    case 4: e.Value = quote.Volume; break;
                    case 5: e.Value = quote.TradeTime; break;
                    case 6: e.Value = quote.Latency; break;
                    default: e.Value = null; break; 
                }
            }
        }

        public void AddQuotes (Quotes newQuote)
        {
            if (InvokeRequired)
            {
                Invoke (new Action (() => AddQuotes (newQuote)));
                return;
            }

            dataList.Add (newQuote);
            dataGridView1.RowCount = dataList.Count; // 更新行數
        }

        public void UpdateQuotes (int index, Quotes updatedQuote)
        {
            if (InvokeRequired)
            {
                Invoke (new Action (() => UpdateQuotes (index, updatedQuote)));
                return;
            }

            if (index < 0 || index >= dataList.Count) return;

            dataList[index] = updatedQuote;
            lock (dirtyRows)
            {
                dirtyRows.Add (index); // 記錄需要更新的行
            }
        }
    }
}
