using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesClient
{
    class SubscribeStockMessage : NetWorkMessage
    {
        public List<string> stocks { get; init; }
        public SubscribeStockMessage (List<string> stocks)
            : base("Subscribe")
        {
            this.stocks = stocks;
        }
    }
    class RequestStockDirty : NetWorkMessage
    {
        public string stocks { get; init; }
        public RequestStockDirty (string stocks)
            : base ("RequestStockDirty")
        {
            this.stocks = stocks;
        }
    }
}
