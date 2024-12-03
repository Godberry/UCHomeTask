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
        public bool PushTickers (in STradeDetail _detail)
        {
            if (!m_all_quotes.ContainsKey(_detail.Stock))
            {
                m_all_quotes.Add (_detail.Stock, new Quotes
                {
                    Stock = _detail.Stock,
                    Price = _detail.Price,
                    AskPrice = _detail.AskPrice,
                    BidPrice = _detail.BidPrice,
                    Timestamp = _detail.Timestamp,
                    Volume = (ulong)_detail.Volume,
                    HighestPrice = _detail.Price,
                    LowestPrice = _detail.Price,
                    SerialNo = _detail.SerialNo
                });
                return true;
            }
            else
            {
                Quotes quotes = m_all_quotes[_detail.Stock];
                quotes.Price = _detail.Price;
                quotes.AskPrice = _detail.AskPrice;
                quotes.BidPrice = _detail.BidPrice;
                quotes.Timestamp = _detail.Timestamp;
                quotes.Volume += (ulong)_detail.Volume;
                quotes.HighestPrice = Math.Max(_detail.Price, quotes.HighestPrice);
                quotes.LowestPrice = Math.Min (_detail.Price, quotes.LowestPrice);
                return false;
            }
        }

        public (Quotes, int) GetQuotes (string _stock)
        {
            return (m_all_quotes[_stock], GetStockIndex(_stock));
        }

        public int GetStockIndex(string _stock)
        {
            return m_all_quotes.Keys.ToList().IndexOf(_stock);
        }
    }
}
