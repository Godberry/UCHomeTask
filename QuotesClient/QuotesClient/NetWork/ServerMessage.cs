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
    public class UpdateQuotesMessage : NetWorkMessage
    {
        public Quotes quotes { get; set; }
        public UpdateQuotesMessage (Quotes quotes)
            : base ("UpdateQuotes")
        {
            this.quotes = quotes;
        }
    }
}
