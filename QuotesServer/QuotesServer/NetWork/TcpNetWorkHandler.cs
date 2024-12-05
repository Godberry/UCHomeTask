using QuotesServer;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuotesServer
{
    public class TcpNetworkHandler : INetworkHandler
    {
        private TcpListener _tcpListener;
        private ConcurrentDictionary<IPEndPoint, TcpClient> _clients = new();
        public Action<byte[], IPEndPoint> OnDataReceived;

        public async Task InitializeAsync (string address, int port)
        {
            _tcpListener = new TcpListener (IPAddress.Parse (address), port);
            _tcpListener.Start ();
            Console.WriteLine ($"TCP Server started on {address}:{port}");
            await Task.CompletedTask;
        }

        public async Task AcceptClientsAsync ()
        {
            while (true)
            {
                try
                {
                    var tcpClient = await _tcpListener.AcceptTcpClientAsync();
                    var remoteEndpoint = tcpClient.Client.RemoteEndPoint as IPEndPoint;

                    if (remoteEndpoint != null)
                    {
                        _clients[remoteEndpoint] = tcpClient;
                        Console.WriteLine ($"Client connected: {remoteEndpoint}");

                        // 啟動客戶端處理任務
                        _ = HandleClientAsync (tcpClient, remoteEndpoint);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine ($"Error accepting client: {ex.Message}");
                }
            }
        }

        public async Task HandleClientAsync (TcpClient tcpClient, IPEndPoint remoteEndpoint)
        {
            try
            {
                using (tcpClient)
                {
                    var networkStream = tcpClient.GetStream();
                    byte[] buffer = new byte[1024];

                    while (tcpClient.Connected)
                    {
                        int bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);

                        if (bytesRead == 0)
                        {
                            Console.WriteLine ($"Client disconnected: {remoteEndpoint}");
                            break;
                        }

                        byte[] receivedData = new byte[bytesRead];
                        Array.Copy (buffer, receivedData, bytesRead);

                        Console.WriteLine ($"Received from {remoteEndpoint}: {Encoding.UTF8.GetString (receivedData)}");

                        // 呼叫外部事件處理程序
                        OnDataReceived?.Invoke (receivedData, remoteEndpoint);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine ($"Error handling client {remoteEndpoint}: {ex.Message}");
            }
            finally
            {
                _clients.TryRemove (remoteEndpoint, out _);
            }
        }

        public async Task SendAsync (byte[] data, IPEndPoint remoteEndpoint)
        {
            if (_clients.TryGetValue (remoteEndpoint, out TcpClient? tcpClient) && tcpClient.Connected)
            {
                var networkStream = tcpClient.GetStream();
                await networkStream.WriteAsync (data, 0, data.Length);
                Console.WriteLine ($"Data sent to {remoteEndpoint}: {Encoding.UTF8.GetString (data)}");
            }
            else
            {
                throw new InvalidOperationException ($"Client {remoteEndpoint} is not connected.");
            }
        }

        public Task<(byte[], IPEndPoint)> ReceiveAsync ()
        {
            throw new NotImplementedException ("不實作ReceiveAsync");
        }

        public IPEndPoint GetRemoteEndpoint ()
        {
            throw new NotImplementedException ("不實做GetRemoteEndpoint");
        }

        public async Task CloseAsync ()
        {
            foreach (var client in _clients.Values)
            {
                client.Close ();
            }
            _tcpListener.Stop ();
            Console.WriteLine ("TCP server stopped.");
            await Task.CompletedTask;
        }
    }
}
