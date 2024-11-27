﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                trade_details[stock] = new ConcurrentQueue<STradeDetail>(); // 初始化每個商品成交回報list
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
                            trade_details[stock].Enqueue (detail); 
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
                Volume = 1,
                SerialNo = serial_no++
            };
        }

        public Dictionary<string, List<STradeDetail>> GetQuotes (int maxCount = 100)
        {
            var quotes = new Dictionary<string, List<STradeDetail>>();

            foreach (var stock in stock_list)
            {
                if (trade_details.TryGetValue (stock, out var queue))
                {
                    List<STradeDetail> details = new List<STradeDetail> ();
                    for (int i = 0; i < maxCount; i++)
                    {
                        if (queue.TryDequeue (out var detail))
                        {
                            details.Add (detail);
                        }
                        else
                        {
                            break; // 沒有更多報價
                        }
                    }
                    quotes.Add (stock, details);
                }
            }

            return quotes;
        }
    }
}
