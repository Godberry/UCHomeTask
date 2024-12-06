using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace QuotesServer
{
    internal class QuoteServer
    {
        private readonly UdpNetworkHandler udpNetworkHandler = new();
        private readonly TcpNetworkHandler tcpNetworkHandler = new();
        private readonly SubscriptionManager subscriptionManager = new();
        private readonly QuoteGenerator quoteGenerator;
        private readonly ConcurrentDictionary<IPEndPoint, HashSet<string>> clientQuoteStatus = new(); // 紀錄client是否為dirty狀態

        public QuoteServer (QuoteGenerator quoteGenerator)
        {
            this.quoteGenerator = quoteGenerator;
        }

        public async Task StartAsync (string address, int port)
        {
            await udpNetworkHandler.InitializeAsync (address, port);
            await tcpNetworkHandler.InitializeAsync (address, port);
            tcpNetworkHandler.OnDataReceived = HandleClientTCPMessage;

            // 接聽client的tcp消息
            _ = tcpNetworkHandler.AcceptClientsAsync();

            // 開始產生報價
            quoteGenerator.StartGenerating ();

            // 推送報價(udp)
            await PushQuotes ();
        }
        public void HandleClientTCPMessage (byte[] data, IPEndPoint clientEndpoint)
        {
            var message = Encoding.UTF8.GetString(data);
            try
            {
                var request = NetworkMessageParser.ParseMessage(message);

                switch (request)
                {
                    case SubscribeStockMessage subscribeStockMessage:
                        // 添加或更新客戶端的訂閱清單
                        foreach (var stock in subscribeStockMessage.stocks)
                        {
                            // 註冊商品
                            subscriptionManager.Subscribe (stock, clientEndpoint);
                        }

                        Console.WriteLine ($"Client {clientEndpoint} subscribed to: {string.Join (", ", subscribeStockMessage.stocks)}");
                        break;
                    case RequestStockDirty requestStockDirty:       // 暫無使用，保留ticker回補機制做修改
                        AddClientQuoteStatus (requestStockDirty.stocks, clientEndpoint);
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

        private async Task PushQuotes ()
        {
            while (true)
            {
                try
                {
                    var allTickers = quoteGenerator.GetNewestTicker();

                    foreach (var tickers in allTickers)
                    {
                        var stock = tickers.Key;
                        var subscribers = subscriptionManager.GetSubscriber(stock);
                        // 沒人訂閱就下一檔
                        if (subscribers.Count == 0)
                        {
                            continue;
                        }

                        Quotes quotes = new Quotes();
                        quoteGenerator.GetQuotes (stock, out quotes);
                        UpdateQuotesMessage newStockPackage = new UpdateQuotesMessage(quotes);
                        string messageNewStock = JsonSerializer.Serialize (newStockPackage);
                        var dataNewStock = Encoding.UTF8.GetBytes(messageNewStock);
                        foreach (var subscriber in subscribers)
                        {
                            await udpNetworkHandler.SendAsync (dataNewStock, subscriber);
                        }
                    }

                    await Task.Delay (1);
                }
                catch (Exception ex)
                {
                    Console.WriteLine ($"Error pushing quotes: {ex.Message}");
                }
            }
        }
        #region 暫無使用，保留ticker機制日後可做修改
        private async Task ProcClientQuoteDirty (string stock, IPEndPoint subscriber)
        {
            if (IsDirtyStock (stock, subscriber))
            {
                Quotes quotes = new Quotes();
                quoteGenerator.GetQuotes (stock, out quotes);
                UpdateQuotesMessage newStockPackage = new UpdateQuotesMessage(quotes);
                string messageNewStock = JsonSerializer.Serialize (newStockPackage);
                var dataNewStock = Encoding.UTF8.GetBytes(messageNewStock);
                await tcpNetworkHandler.SendAsync (dataNewStock, subscriber);
                RemoveClientQuotesStatus (stock, subscriber);
            }
        }
        
        private void AddClientQuoteStatus (string symbol, IPEndPoint clientEndpoint)
        {
            clientQuoteStatus.AddOrUpdate (
                            clientEndpoint,
                            _ => new HashSet<string> { symbol }, // 如果 clientEndpoint 不存在，新增
                            (_, subscribedSymbols) =>
                            {
                                subscribedSymbols.Add (symbol); // 新增股票到已訂閱列表
                                return subscribedSymbols;
                            });
        }
        public void RemoveClientQuotesStatus (string symbol, IPEndPoint clientEndpoint)
        {
            HashSet<string> newStocks = new HashSet<string> ();
            clientQuoteStatus.TryGetValue (clientEndpoint, out newStocks);
            newStocks.Remove (symbol);
        }
        public bool IsDirtyStock (string stock, IPEndPoint clientEndpoint)
        {
            return clientQuoteStatus.TryGetValue (clientEndpoint, out var newStocks) && newStocks.Contains (stock);
        }
        #endregion
    }
}
