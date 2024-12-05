using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesClient
{
    internal class QuotesFactory
    {
        private ConcurrentDictionary<string, Quotes> allQuotes = new ConcurrentDictionary<string, Quotes>();
        public Action<string> OnQuoteUpdate;
        public void AddQuotes (Quotes quotes)
        {
            allQuotes.AddOrUpdate (quotes.Stock, quotes, (key, existing) => quotes);
            OnQuoteUpdate?.Invoke (quotes.Stock);
        }
        public bool PushTickers (in string stockName, in STradeDetail _detail, bool update = false)
        {
            bool result = true;
            if (!allQuotes.TryGetValue (stockName, out Quotes quotes))
            {
                return result;
            }

            // 如果股票名稱已存在，更新 Quotes
            quotes.Price = _detail.Price;
            quotes.AskPrice = _detail.AskPrice;
            quotes.BidPrice = _detail.BidPrice;
            quotes.Timestamp = _detail.Timestamp;
            quotes.Volume += (ulong)_detail.Volume;
            quotes.HighestPrice = Math.Max (_detail.Price, quotes.HighestPrice);
            quotes.LowestPrice = Math.Min (_detail.Price, quotes.LowestPrice);
            if (quotes.SerialNo == _detail.SerialNo || quotes.SerialNo + 1 == _detail.SerialNo)
            {
                quotes.SerialNo = _detail.SerialNo;
            }
            else
            {
                return false;
            }

            if (update)
            {
                OnQuoteUpdate?.Invoke (quotes.Stock);
            }

            return result;
        }

        public Quotes GetQuotes (string _stock)
        {
            return allQuotes[_stock];
        }

        public int GetStockIndex(string _stock)
        {
            return allQuotes.Keys.ToList().IndexOf(_stock);
        }
    }
}
