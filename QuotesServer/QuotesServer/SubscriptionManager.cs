using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuotesServer
{
    class SubscriptionManager
    {
        private readonly ConcurrentDictionary<string, List<NetworkStream>> _subscriptions = new();

        // 客戶端訂閱商品
        public void Subscribe (string symbol, NetworkStream stream)
        {
            _subscriptions.AddOrUpdate (symbol,
                _ => new List<NetworkStream> { stream },
                (_, existingStreams) =>
                {
                    existingStreams.Add (stream);
                    return existingStreams;
                });
        }

        // 客戶端取消訂閱（可選）
        public void Unsubscribe (string symbol, NetworkStream stream)
        {
            if (_subscriptions.TryGetValue (symbol, out var streams))
            {
                streams.Remove (stream);
                if (streams.Count == 0)
                    _subscriptions.TryRemove (symbol, out _);
            }
        }

        // 推送報價給訂閱者
        public async Task NotifySubscribersAsync (string symbol, string quote)
        {
            if (_subscriptions.TryGetValue (symbol, out var streams))
            {
                var data = Encoding.UTF8.GetBytes(quote + "\n");
                foreach (var stream in streams)
                {
                    try
                    {
                        await stream.WriteAsync (data); // 主動推送給客戶端
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine ($"Failed to notify client: {ex.Message}");
                    }
                }
            }
        }
    }
}
