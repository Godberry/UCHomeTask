using QuotesClient;
using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class QuoteClient
{
    private readonly UdpNetworkHandler udpNetworkHandler = new();
    private readonly TcpNetworkHandler tcpNetworkHandler = new();
    private readonly QuoteService quoteService;

    public QuoteClient (QuoteForm quoteForm)
    {
        quoteService = new QuoteService (tcpNetworkHandler, udpNetworkHandler, quoteForm);
    }
    public async Task StartAsync ()
    {
        // 初始化連線
        await ConnectAsync ("127.0.0.1", 5000);

        // 初始化報價相關的service
        quoteService.Initialize ();

        // 接收TCP訊息
        _ = quoteService.StartReceivingTCPAsync ();
        // 接收報價
        _ = quoteService.StartReceivingQuotesAsync ();
    }
    public async Task ConnectAsync (string serverIp, int serverPort)
    {
        await tcpNetworkHandler.InitializeAsync (serverIp, serverPort);
        await udpNetworkHandler.InitializeAsync (serverIp, tcpNetworkHandler.GetLocalEndPoint());
    }
}

