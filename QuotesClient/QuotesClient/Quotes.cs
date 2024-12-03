using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesClient
{
    public class Quotes
    {
        public string Stock { get; init; }           // 商品名稱
        public long Timestamp { get; set; }         // 時間戳記
        public decimal Price { get; set; }          // 價格
        public decimal AskPrice { get; set; }       // 買價
        public decimal BidPrice { get; set; }       // 成交價
        public ulong Volume { get; set; }           // 成交張數
        public ulong SerialNo { get; set; }         // 序列號
        public decimal HighestPrice { get; set; }   // 最高價
        public decimal LowestPrice { get; set; }    // 最低價
        public string TradeTime
        {
            get
            {
                var unixTimestampInSeconds = Timestamp / 1000;
                return DateTimeOffset.FromUnixTimeSeconds (unixTimestampInSeconds).ToLocalTime ().ToString ("yyyy-MM-dd HH:mm:ss");
            }
        }
        public long Latency
        {
            get
            {
                long cur = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds ();
                return cur - Timestamp;
            }
        }
    }
}
