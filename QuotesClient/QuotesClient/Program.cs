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
            try
            {
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();

                QuoteForm quoteForm = new QuoteForm();
                var client = new QuoteClient(quoteForm);

                _ = client.StartAsync();

                Application.Run(quoteForm);
            }
            catch (Exception ex)
            {
                File.WriteAllText("error.log", ex.ToString());
            }
        }
    }
}