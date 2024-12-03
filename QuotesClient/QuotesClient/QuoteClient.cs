using QuotesClient;
using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class QuoteClient
{
    private readonly INetworkHandler networkHandler;
    private readonly QuoteForm quoteForm;
    private readonly QuotesFactory quoteFactory = new();

    public QuoteClient (INetworkHandler networkHandler, QuoteForm quoteForm)
    {
        this.networkHandler = networkHandler;
        this.quoteForm = quoteForm;
    }

    public async Task ConnectAsync (string serverIp, int serverPort)
    {
        await networkHandler.InitializeAsync (serverIp, serverPort);
    }

    public async Task SubscribeToStocks (List<string> stocks)
    {
        var request = JsonSerializer.Serialize(new
        {
            Type = "Subscribe",
            Stocks = stocks
        });

        await networkHandler.SendAsync (Encoding.UTF8.GetBytes (request), networkHandler.GetRemoteEndpoint());
        Console.WriteLine ($"Subscribed to: {string.Join (", ", stocks)}");
    }

    public async Task StartReceiving ()
    {
        while (true)
        {
            var data = await networkHandler.ReceiveAsync();
            try
            {
                var details = JsonSerializer.Deserialize<List<STradeDetail>>(Encoding.UTF8.GetString(data));
                if (details == null) continue;

                bool is_new = false;
                foreach (var ticker in details) {
                    is_new |= quoteFactory.PushTickers (ticker);
                }

                if (is_new)
                {
                    quoteForm.AddQuotes (quoteFactory.GetQuotes (details[0].Stock).Item1);
                }
                else
                {
                    quoteForm.UpdateQuotes (quoteFactory.GetQuotes (details[0].Stock).Item2, quoteFactory.GetQuotes (details[0].Stock).Item1);
                }


                long cur = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                foreach (var detail in details)
                {
                    var latency = cur - detail.Timestamp;
                    Console.WriteLine ($"latency {latency}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine ($"Error deserializing data: {ex.Message}");
            }
        }
    }
}

