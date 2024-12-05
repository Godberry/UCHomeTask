using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesClient
{
    internal class QuotesFactory
    {
        private Dictionary<string, Quotes> m_all_quotes = new Dictionary<string, Quotes>();
        public void AddQuotes (Quotes quotes)
        {
            m_all_quotes.TryAdd (quotes.Stock, quotes);
        }
        public bool PushTickers (in string stockName, in STradeDetail _detail)
        {
            bool result = true;
            if (!m_all_quotes.TryGetValue (stockName, out Quotes quotes))
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

            return result;
        }

        public Quotes GetQuotes (string _stock)
        {
            return m_all_quotes[_stock];
        }

        public int GetStockIndex(string _stock)
        {
            return m_all_quotes.Keys.ToList().IndexOf(_stock);
        }
    }
}
