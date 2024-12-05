using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesClient
{
    class ReceiveTickersMessage : NetWorkMessage
    {
        public string stokName { get; set; }
        public List<STradeDetail> tickers { get; set; }
    }
    public class SubscribeStocksSuccessMessage : NetWorkMessage
    {
        public Quotes quotes { get; set; }
        public SubscribeStocksSuccessMessage (Quotes quotes)
            : base ("SubscribeStocksSuccessMessage")
        {
            this.quotes = quotes;
        }
    }
}
