namespace QuotesClient
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            QuoteForm quoteForm = new QuoteForm();

            Task.Run (async () =>
            {
                var networkHandler = new UdpNetworkHandler();
                var client = new QuoteClient(networkHandler, quoteForm);

                await client.ConnectAsync ("127.0.0.1", 5000);
                await client.SubscribeToStocks (new List<string> { "Stock1", "Stock2", "Stock3" });
                await client.StartReceiving ();
            });

            Application.Run(quoteForm);
        }
    }
}