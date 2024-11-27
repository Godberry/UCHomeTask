using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using QuotesServer;

class Program
{
    public static async Task Main (string[] args)
    {
        var networkHandler = new UdpNetworkHandler(); // 可切換為其他實現
        var quoteGenerator = new QuoteGenerator(100); // 100 個商品
        var server = new QuoteServer(networkHandler, quoteGenerator);

        await server.StartAsync ("127.0.0.1", 5000);
    }
}
