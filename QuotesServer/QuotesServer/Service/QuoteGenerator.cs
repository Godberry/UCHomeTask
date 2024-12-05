using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace QuotesServer
{
    class QuoteGenerator
    {
        private readonly ConcurrentDictionary<string, ConcurrentQueue<STradeDetail>> trade_details = new();
        private readonly ConcurrentDictionary<string, Quotes> quotes = new ();
        private readonly List<string> stock_list = new();
        private readonly Dictionary<string, decimal> basePrice = new();
        private readonly Dictionary<string, ulong> stocksSerialNo = new();
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
                basePrice.Add (stock, random.Next (0, 100));
                stocksSerialNo.Add (stock, 0);
            }
        }

        public void StartGenerating (int intervalMilliseconds = 1000)
        {
            Random random = new Random();

            foreach (var stock in stock_list)
            {
                Task.Run (async () =>
                {
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
            // 模擬價格變動
            decimal basePrice = this.basePrice[_stock];
            decimal priceChange = Math.Round((decimal)(random.NextDouble() * 2 - 1), 2); // 隨機變動 [-1, 1]

            basePrice = Math.Max (1, basePrice + priceChange); // 價格不可低於 1
            this.basePrice[_stock] = basePrice; // 同步更新基準價格

            // 模擬買賣價差
            decimal spread = Math.Round((decimal)random.NextDouble() * 0.5m, 2); // 價差 [0, 0.5]
            decimal askPrice = Math.Round(basePrice + spread, 2);
            decimal bidPrice = Math.Round(basePrice - spread, 2);

            // 模擬成交量
            int volume = random.Next(1, 100); // 隨機成交量 [1, 100]

            stocksSerialNo[_stock]++;
            // 返回生成的報價
            return new STradeDetail
            {
                Stock = _stock,
                Price = basePrice,
                AskPrice = askPrice,
                BidPrice = bidPrice,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds (),
                Volume = volume,
                SerialNo = stocksSerialNo[_stock]
            };
        }

        public Dictionary<string, List<STradeDetail>> GetNewestTicker (int maxCount = 100)
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
                        PushTickers (stock, detail);
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
        public void PushTickers (in string stockName, STradeDetail _detail)
        {
            quotes.AddOrUpdate (
                stockName,
                _ =>
                {
                    // 新增邏輯
                    return CreateNewQuote (_detail);
                },
                (_, existingQuotes) =>
                {
                    // 更新邏輯
                    UpdateExistingQuote (existingQuotes, _detail);
                    return existingQuotes;
                });
        }

        // 初始化新的 Quotes
        private Quotes CreateNewQuote (in STradeDetail _detail)
        {
            return new Quotes
            {
                Stock = _detail.Stock,
                Price = _detail.Price,
                AskPrice = _detail.AskPrice,
                BidPrice = _detail.BidPrice,
                Timestamp = _detail.Timestamp,
                Volume = (ulong)_detail.Volume,
                HighestPrice = _detail.Price,
                LowestPrice = _detail.Price,
                SerialNo = _detail.SerialNo
            };
        }

        // 更新現有的 Quotes
        private void UpdateExistingQuote (Quotes existingQuotes, in STradeDetail _detail)
        {
            existingQuotes.Price = _detail.Price;
            existingQuotes.AskPrice = _detail.AskPrice;
            existingQuotes.BidPrice = _detail.BidPrice;
            existingQuotes.Timestamp = _detail.Timestamp;
            existingQuotes.Volume += (ulong)_detail.Volume;
            existingQuotes.HighestPrice = Math.Max (_detail.Price, existingQuotes.HighestPrice);
            existingQuotes.LowestPrice = Math.Min (_detail.Price, existingQuotes.LowestPrice);
            existingQuotes.SerialNo = _detail.SerialNo;
        }
        public bool GetQuotes (string stock, out Quotes result)
        {
            return quotes.TryGetValue(stock, out result);
        }
    }
}
