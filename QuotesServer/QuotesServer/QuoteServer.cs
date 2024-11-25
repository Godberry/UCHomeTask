using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuotesServer
{
    internal class QuoteServer
    {
        private readonly QuoteGenerator quote_generator;
        private readonly int port;
        public QuoteServer (int _product_count, int port)
        {
            this.port = port;
            quote_generator = new QuoteGenerator(_product_count);
        }
        public void Start()
        {
            // 產生報價
            quote_generator.StartGenerating();

            // 接聽client發送的事件
            // udp server handler?? 或許處理heartbeat?

            // 傳送報價的工作
            // 從報價產生器那邊取得新增的報價，紀錄報價到Stock的history中，並發送封包給有註冊對應商品的client
        }
    }
}
