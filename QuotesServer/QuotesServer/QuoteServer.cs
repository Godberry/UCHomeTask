using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuotesServer
{
    internal class QuoteServer
    {
        private ConcurrentDictionary<string, double> _quotes = new();
        private TcpListener _listener;

        public QuoteServer ()
        {
            InitializeQuotes (100); // 初始化 100 個商品
        }

        private void InitializeQuotes (int count)
        {
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                string symbol = $"Stock{i + 1}";
                _quotes[symbol] = random.NextDouble () * 100;
            }
        }

        public async Task StartAsync (int port)
        {
            _listener = new TcpListener (IPAddress.Any, port);
            _listener.Start ();
            Console.WriteLine ("Server started. Waiting for clients...");

            while (true)
            {
                var client = await _listener.AcceptTcpClientAsync();
                Console.WriteLine ("Client connected.");
                _ = Task.Run (() => HandleClientAsync (client));
            }
        }

        private async Task HandleClientAsync (TcpClient client)
        {
            using NetworkStream stream = client.GetStream();
            var random = new Random();

            while (true)
            {
                foreach (var symbol in _quotes.Keys)
                {
                    double price = _quotes[symbol] + random.NextDouble();
                    var quote = $"{symbol},{price:F2},{DateTime.Now:O}";
                    byte[] data = Encoding.UTF8.GetBytes(quote + "\n");

                    await stream.WriteAsync (data);
                    await Task.Delay (10); // 控制發送頻率
                }
            }
        }

    }
}
