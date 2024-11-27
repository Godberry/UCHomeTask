using System;
using System.Net.Sockets;
using System.Text;

class ClientTester
{
    static async Task Main (string[] args)
    {
        var networkHandler = new UdpNetworkHandler();
        var client = new QuoteClient(networkHandler);

        await client.ConnectAsync ("127.0.0.1", 5000);
        await client.SubscribeToStocks (new List<string> { "Stock1", "Stock2" });
        await client.StartReceiving ();
    }
}