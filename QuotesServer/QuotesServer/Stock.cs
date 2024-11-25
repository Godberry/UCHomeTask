using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesServer
{
    internal class CStock
    {
        public string StockName;
        public ConcurrentBag<STradeDetail> TradeDetails = new ConcurrentBag<STradeDetail>();
    }
}
