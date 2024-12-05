using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesClient
{
    public interface IQuoteService
    {
        Task SubscribeAsync (string productId); // 訂閱商品報價
        Task RequestQuotesDirtyAsync (string stock); // 請求補報價

        public event Action<string, List<STradeDetail>> OnTickerReceived;   // 接收ticker
        public event Action<int, STradeDetail> OnRequestTicker;             // 接收回補的ticker
    }
}
