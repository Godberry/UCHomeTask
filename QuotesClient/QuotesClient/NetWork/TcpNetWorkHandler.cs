using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class TcpNetworkHandler : INetworkHandler
{
    private TcpClient tcpClient;
    private NetworkStream networkStream;
    private IPEndPoint serverEndpoint;

    public async Task InitializeAsync (string address, int port)
    {
        tcpClient = new TcpClient ();
        serverEndpoint = new IPEndPoint (IPAddress.Parse (address), port);
        await tcpClient.ConnectAsync (address, port);
        networkStream = tcpClient.GetStream ();
    }

    public async Task SendAsync (byte[] data, IPEndPoint remoteEndpoint)
    {
        if (networkStream == null || !tcpClient.Connected)
        {
            throw new InvalidOperationException ("TCP client is not connected.");
        }

        await networkStream.WriteAsync (data, 0, data.Length);
    }

    public async Task<byte[]> ReceiveAsync ()
    {
        if (networkStream == null || !tcpClient.Connected)
        {
            throw new InvalidOperationException ("TCP client is not connected.");
        }

        byte[] buffer = new byte[1024]; // Adjust buffer size as needed
        int bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);

        if (bytesRead == 0)
        {
            throw new InvalidOperationException ("Connection closed by the server.");
        }

        byte[] receivedData = new byte[bytesRead];
        Array.Copy (buffer, receivedData, bytesRead);
        return receivedData;
    }

    public int GetLocalEndPoint ()
    {
        var localEndPoint = tcpClient.Client.LocalEndPoint as IPEndPoint;
        if (localEndPoint != null)
        {
            return localEndPoint.Port;
        }
        return 8888;
    }

    public IPEndPoint GetRemoteEndpoint ()
    {
        return serverEndpoint;
    }

    public async Task CloseAsync ()
    {
        if (networkStream != null)
        {
            await networkStream.DisposeAsync ();
        }

        tcpClient?.Close ();
    }
}