using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuotesServer
{
    public interface INetworkHandler
    {
        Task InitializeAsync (string address, int port); // 初始化網路層
        Task SendAsync (byte[] data, IPEndPoint remoteEndpoint); // 發送數據
        Task<byte[]> ReceiveAsync (); // 接收數據
        IPEndPoint GetRemoteEndpoint (); // 獲取最後一個接收數據的客戶端地址
        Task CloseAsync (); // 關閉網路層
    }

    class ClientRequest
    {
        public string Type { get; set; } // "Subscribe"
        public List<string> Stocks { get; set; }
    }
}
