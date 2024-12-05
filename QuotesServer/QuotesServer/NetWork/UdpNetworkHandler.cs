using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuotesServer
{
    public class UdpNetworkHandler : INetworkHandler
    {
        private UdpClient udpClient;
        private IPEndPoint lastRemoteEndpoint;

        public async Task InitializeAsync (string address, int port)
        {
            udpClient = new UdpClient (port); // 綁定伺服器的端口
            await Task.CompletedTask;
        }

        public async Task SendAsync (byte[] data, IPEndPoint remoteEndpoint)
        {
            await udpClient.SendAsync (data, data.Length, remoteEndpoint);
        }

        public async Task<(byte[], IPEndPoint)> ReceiveAsync ()
        {
            var result = await udpClient.ReceiveAsync();
            lastRemoteEndpoint = result.RemoteEndPoint; // 記錄最近的客戶端端點
            return (result.Buffer, result.RemoteEndPoint);
        }

        public IPEndPoint GetRemoteEndpoint ()
        {
            return lastRemoteEndpoint;
        }

        public async Task CloseAsync ()
        {
            udpClient?.Close ();
            await Task.CompletedTask;
        }
    }
}
