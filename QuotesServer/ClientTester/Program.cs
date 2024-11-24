using System;
using System.Net.Sockets;
using System.Text;

class ClientTester
{
    public static void Main (string[] args)
    {
        var client = new TcpClient();
        client.Connect ("127.0.0.1", 9000); // 連接本機 Server

        using NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;

        Console.WriteLine ("Connected to server. Receiving data...");
        while ((bytesRead = stream.Read (buffer, 0, buffer.Length)) > 0)
        {
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine (message);
        }
    }
}