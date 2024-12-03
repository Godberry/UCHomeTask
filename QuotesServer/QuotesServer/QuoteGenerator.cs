﻿using System;
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
        private readonly List<string> stock_list = new();
        private readonly Dictionary<string, decimal> basePrice = new();
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

            // 返回生成的報價
            return new STradeDetail
            {
                Stock = _stock,
                Price = basePrice,
                AskPrice = askPrice,
                BidPrice = bidPrice,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds (),
                Volume = volume,
                SerialNo = Interlocked.Increment (ref serial_no)
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
