using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuotesServer
{
    class SubscriptionManager
    {
        private readonly ConcurrentDictionary<string, List<IPEndPoint>> _subscriptions = new();

        // 客戶端訂閱商品
        public void Subscribe (string symbol, IPEndPoint stream)
        {
            _subscriptions.AddOrUpdate (symbol,
                _ => new List<IPEndPoint> { stream },
                (_, existingStreams) =>
                {
                    existingStreams.Add (stream);
                    return existingStreams;
                });
        }

        // 客戶端取消訂閱（可選）
        public void Unsubscribe (string stock, IPEndPoint clientEndpoint)
        {
            if (_subscriptions.TryGetValue (stock, out var clients))
            {
                clients.Remove (clientEndpoint);
                if (clients.Count == 0)
                    _subscriptions.TryRemove (stock, out _);
            }
        }

        public void RemoveClient (IPEndPoint clientEndpoint)
        {
            foreach (var stock in _subscriptions.Keys.ToList ())
            {
                _subscriptions[stock].Remove (clientEndpoint);
                if (_subscriptions[stock].Count == 0)
                    _subscriptions.TryRemove (stock, out _);
            }
        }

        // 取得有訂閱這檔股票的用戶
        public List<IPEndPoint> GetSubscriber (string symbol)
        {
            if (_subscriptions.TryGetValue (symbol, out var streams))
            {
                return streams;
            }
            
            return new List<IPEndPoint> ();    
        }
    }
}
