using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using QuotesServer;
using static System.Net.Mime.MediaTypeNames;

class Program
{
    public static async Task Main (string[] args)
    {
        try
        {
            // 預設商品數量
            int productCount = 100;

            // 檢查是否有傳入商品數量參數
            if (args.Length > 0 && int.TryParse(args[0], out var count) && count > 0)
            {
                productCount = count;
            }
            else
            {
                Console.WriteLine("使用預設商品數量100");
            }

            var quoteGenerator = new QuoteGenerator(productCount);
            var server = new QuoteServer(quoteGenerator);

            await server.StartAsync("127.0.0.1", 5000);
        }
        catch (Exception ex)
        {
            File.WriteAllText("error.log", ex.ToString());
        }
    }
}
