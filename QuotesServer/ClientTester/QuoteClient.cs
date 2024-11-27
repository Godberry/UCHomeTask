using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class QuoteClient
{
    private readonly INetworkHandler networkHandler;

    public QuoteClient (INetworkHandler networkHandler)
    {
        this.networkHandler = networkHandler;
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

                foreach (var detail in details)
                {
                    Console.WriteLine ($"[{detail.Timestamp}] {detail.Stock}: {detail.Price:C} ({detail.Volume})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine ($"Error deserializing data: {ex.Message}");
            }
        }
    }
}

