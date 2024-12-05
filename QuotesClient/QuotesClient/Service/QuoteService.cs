using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QuotesClient
{
    public class QuoteService : IQuoteService
    {
        private readonly TcpNetworkHandler tcpNetworkHandler;
        private readonly UdpNetworkHandler udpNetworkHandler;
        private readonly QuotesFactory quoteFactory = new();
        private readonly QuoteForm quoteForm;

        public event Action<string, List<STradeDetail>> OnTickerReceived;
        public event Action<int, STradeDetail> OnRequestTicker;

        public QuoteService (TcpNetworkHandler tcpHandler, UdpNetworkHandler udpHandler, QuoteForm quoteForm)
        {
            tcpNetworkHandler = tcpHandler;
            udpNetworkHandler = udpHandler;
            this.quoteForm = quoteForm;
        }
        public void Initialize ()
        {
            #region server相關
            OnTickerReceived += QuoteService_OnTickerReceived;
            #endregion
            #region ui相關
            quoteForm.OnAddProductBtnClick += QuoteForm_OnAddProductBtnClick;
            #endregion
        }

        private void QuoteForm_OnAddProductBtnClick (string stock)
        {
            _ = SubscribeAsync (stock);
        }

        public async Task SubscribeAsync (string productId)
        {
            SubscribeStockMessage subscribeStock = new SubscribeStockMessage([productId]);
            string message =JsonSerializer.Serialize (subscribeStock);
            await tcpNetworkHandler.SendAsync (Encoding.UTF8.GetBytes (message), tcpNetworkHandler.GetRemoteEndpoint());
        }

        public async Task RequestQuotesDirtyAsync (string stock)
        {
            RequestStockDirty subscribeStock = new RequestStockDirty(stock);
            string message =JsonSerializer.Serialize (subscribeStock);
            await tcpNetworkHandler.SendAsync (Encoding.UTF8.GetBytes (message), tcpNetworkHandler.GetRemoteEndpoint ());
        }

        public async Task StartReceivingTCPAsync ()
        {
            while (true)
            {
                byte[] data = await tcpNetworkHandler.ReceiveAsync();
                var message = Encoding.UTF8.GetString(data);
                try
                {
                    var request = NetworkMessageParser.ParseMessage(message);

                    switch (request)
                    {
                        case SubscribeStocksSuccessMessage subscribeSuccess:
                            quoteFactory.AddQuotes (subscribeSuccess.quotes);
                            break;
                        default:
                            Console.WriteLine ("未知消息類型");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine ($"消息解析錯誤: {ex.Message}");
                }
            }
        }
        public async Task StartReceivingQuotesAsync ()
        {
            while (true)
            {
                var data = await udpNetworkHandler.ReceiveAsync();
                try
                {
                    var details = JsonSerializer.Deserialize<ReceiveTickersMessage>(Encoding.UTF8.GetString(data));
                    if (details == null) continue;

                    OnTickerReceived?.Invoke (details.stokName, details.tickers);
                }
                catch (Exception ex)
                {
                    Console.WriteLine ($"Error deserializing data: {ex.Message}");
                }
            }
        }
        private void QuoteService_OnTickerReceived (string stockName, List<STradeDetail> tickers)
        {
            bool dirty = false;
            foreach (var ticker in tickers)
            {
                if(!quoteFactory.PushTickers (stockName, ticker))
                {
                    dirty = true;
                }
            }

            Quotes quote = quoteFactory.GetQuotes (stockName);
            int stockIndex = quoteFactory.GetStockIndex(stockName);

            quoteForm.UpdateQuotes (stockIndex, quote);

            if (dirty)
            {
                _ = RequestQuotesDirtyAsync (stockName);
            }
        }
    }
}
