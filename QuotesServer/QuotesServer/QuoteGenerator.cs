using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesServer
{
    class QuoteGenerator
    {
        private readonly ConcurrentDictionary<string, ConcurrentBag<STradeDetail>> trade_details = new();
        private readonly List<string> stock_list = new();
        private readonly Random random = new();
        private const int MAX_TRADE_COUNT = 101;
        private const int MIN_TRADE_COUNT = 0;

        public QuoteGenerator (in int _product_count)
        {
            var stocks = Enumerable.Range(1, _product_count).Select(i => $"Stock{i}");
            stock_list.AddRange (stocks); // 將生成的商品清單加入到 stocks 中

            foreach (var symbol in stock_list)
            {
                trade_details[symbol] = new ConcurrentBag<STradeDetail>(); // 初始化每個商品成交回報list
            }
        }

        public void StartGenerating (int intervalMilliseconds = 1000)
        {
            Task.Run (async () =>
            {
                while (true)
                {
                    foreach (var stock in stock_list)
                    {
                        int counts = random.Next(MIN_TRADE_COUNT, MAX_TRADE_COUNT);
                        for (int i = 0; i < counts; i++)
                        {
                            var detail = GenerateQuote(stock);
                            trade_details[stock].Add (detail); 
                        }
                    }
                    await Task.Delay (intervalMilliseconds);
                }
            });
        }

        private STradeDetail GenerateQuote (string _stock)
        {
            return new STradeDetail
            {
                Stock = _stock,
                Price = (decimal)Math.Round (random.NextDouble () * 100, 2), // 兩位小數的價格
                Timestamp = DateTime.Now,
                Volume = 1
            };
        }
    }
}
