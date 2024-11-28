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
        private readonly INetworkHandler networkHandler;
        private readonly QuoteGenerator quoteGenerator;
        private readonly SubscriptionManager subscriptionManager;
        private readonly int port;

        public QuoteServer (INetworkHandler networkHandler, QuoteGenerator quoteGenerator)
        {
            this.networkHandler = networkHandler;
            this.quoteGenerator = quoteGenerator;
            subscriptionManager = new SubscriptionManager ();
        }

        public async Task StartAsync (string address, int port)
        {
            await networkHandler.InitializeAsync (address, port);

            // 開始產生報價
            quoteGenerator.StartGenerating ();

            // 接收客戶端請求
            _ = Task.Run (ReceiveClientRequests);

            // 推送報價
            await PushQuotes ();
        }

        private async Task ReceiveClientRequests ()
        {
            while (true)
            {
                try
                {
                    var data = await networkHandler.ReceiveAsync();
                    var message = Encoding.UTF8.GetString(data);

                    // 解析客戶端請求
                    var request = JsonSerializer.Deserialize<ClientRequest>(message);
                    var clientEndpoint = networkHandler.GetRemoteEndpoint();

                    if (request.Type == "Subscribe")
                    {
                        // 添加或更新客戶端的訂閱清單
                        foreach (var stock in request.Stocks)
                        {
                            subscriptionManager.Subscribe (stock, clientEndpoint);
                        }

                        Console.WriteLine ($"Client {clientEndpoint} subscribed to: {string.Join (", ", request.Stocks)}");
                    }
                }
                catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionReset)
                {
                    Console.WriteLine ("Client forcibly closed the connection. Removing from subscriptions...");
                    subscriptionManager.RemoveClient (networkHandler.GetRemoteEndpoint ());
                }
                catch (Exception ex)
                {
                    Console.WriteLine ($"Error receiving client request: {ex.Message}");
                }
            }
        }

        private async Task PushQuotes ()
        {
            while (true)
            {
                try
                {
                    var quotes = quoteGenerator.GetQuotes();

                    foreach (var quote in quotes)
                    {
                        var subscribers = subscriptionManager.GetSubscriber(quote.Key);
                        var serializedData = JsonSerializer.Serialize(quote.Value);
                        var data = Encoding.UTF8.GetBytes(serializedData);
                        foreach (var subscriber in subscribers)
                        {
                            await networkHandler.SendAsync (data, subscriber);
                        }
                    }

                    await Task.Delay (1); // 每 10 毫秒推送一次
                }
                catch (Exception ex)
                {
                    Console.WriteLine ($"Error pushing quotes: {ex.Message}");
                }
            }
        }
    }   
}
