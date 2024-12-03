using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

public class UdpNetworkHandler : INetworkHandler
{
    private UdpClient udpClient;
    private IPEndPoint serverEndpoint;

    public async Task InitializeAsync (string address, int serverPort)
    {
        udpClient = new UdpClient ();
        serverEndpoint = new IPEndPoint (IPAddress.Parse (address), serverPort);
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
        return serverEndpoint;
    }

    public async Task CloseAsync ()
    {
        udpClient?.Close ();
        await Task.CompletedTask;
    }
}
