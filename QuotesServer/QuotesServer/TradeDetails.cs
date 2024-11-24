using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesServer
{
    public struct STradeDetail
    {
        public string Stock { get; init; }
        public DateTime Timestamp { get; init; }
        public decimal Price { get; init; }
        public int Volume { get; init; }
    }

}
