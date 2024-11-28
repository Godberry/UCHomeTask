using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuotesServer
{
    class QuoteGenerator
    {
        private readonly ConcurrentDictionary<string, ConcurrentQueue<STradeDetail>> trade_details = new();
        private readonly List<string> stock_list = new();
        private readonly Random random = new();
        private const int MAX_TRADE_COUNT = 101;
        private const int MIN_TRADE_COUNT = 0;
        private ulong serial_no = 0;

        public QuoteGenerator (in int _product_count)
        {
            var stocks = Enumerable.Range(1, _product_count).Select(i => $"Stock{i}");
            stock_list.AddRange (stocks); // 將生成的商品清單加入到 stocks 中

            foreach (var stock in stock_list)
            {
                trade_details.TryAdd (stock, new ConcurrentQueue<STradeDetail> ());
            }
        }

        public void StartGenerating (int intervalMilliseconds = 1000)
        {
            Random random = new Random();

            SemaphoreSlim semaphore = new SemaphoreSlim(100);

            foreach (var stock in stock_list)
            {
                Task.Run (async () =>
                {
                    await semaphore.WaitAsync ();
                    while (true)
                    {
                        // 隨機生成該秒的報價數量
                        int targetCount = random.Next(MIN_TRADE_COUNT, MAX_TRADE_COUNT); // 0~100
                        if (targetCount > 0)
                        {
                            // 計算每次報價的間隔時間 (ms)
                            int delayPerTrade = intervalMilliseconds / targetCount;

                            for (int i = 0; i < targetCount; i++)
                            {
                                // 生成報價
                                var detail = GenerateQuote(stock);

                                trade_details[stock].Enqueue (detail);

                                // 等待間隔時間
                                await Task.Delay (delayPerTrade);
                            }
                        }
                        else
                        {
                            // 如果當秒無需報價，直接等待整秒
                            await Task.Delay (intervalMilliseconds);
                        }
                    }
                });
            }
        }

        private STradeDetail GenerateQuote (string _stock)
        {
            return new STradeDetail
            {
                Stock = _stock,
                Price = (decimal)Math.Round (random.NextDouble () * 100, 2), // 兩位小數的價格
                Timestamp = DateTime.Now,
                Volume = 1,
                SerialNo = serial_no++
            };
        }

        public Dictionary<string, List<STradeDetail>> GetQuotes (int maxCount = 100)
        {
            var quotes = new Dictionary<string, List<STradeDetail>>();

            foreach (var kvp in trade_details) // 直接遍歷 trade_details，僅處理有報價的商品
            {
                var stock = kvp.Key;
                var queue = kvp.Value;

                List<STradeDetail> details = new List<STradeDetail>();
                for (int i = 0; i < maxCount; i++)
                {
                    if (queue.TryDequeue (out var detail))
                    {
                        details.Add (detail);
                    }
                    else
                    {
                        break; // 該商品無更多報價
                    }
                }

                if (details.Count > 0) // 僅加入有報價的商品
                {
                    quotes.Add (stock, details);
                }
            }

            return quotes;
        }
    }
}
