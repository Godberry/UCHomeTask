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
        var server = new QuoteServer();
        await server.StartAsync (9000);
    }
}
