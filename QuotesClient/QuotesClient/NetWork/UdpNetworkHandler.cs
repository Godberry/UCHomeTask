using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

public class UdpNetworkHandler : INetworkHandler
{
    private UdpClient udpClient;

    public async Task InitializeAsync (string address, int serverPort)
    {
        udpClient = new UdpClient (serverPort);
        await Task.CompletedTask;
    }

    public async Task SendAsync (byte[] data, IPEndPoint remoteEndpoint)
    {
        await udpClient.SendAsync (data, data.Length, remoteEndpoint);
    }

    public async Task<byte[]> ReceiveAsync ()
    {
        var result = await udpClient.ReceiveAsync();
        return result.Buffer;
    }

    public IPEndPoint GetRemoteEndpoint ()
    {
        return null;
    }

    public async Task CloseAsync ()
    {
        udpClient?.Close ();
        await Task.CompletedTask;
    }
}
