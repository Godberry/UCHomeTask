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
        private readonly ConcurrentDictionary<string, HashSet<IPEndPoint>> _subscriptions = new();

        // 客戶端訂閱商品
        public bool Subscribe (string symbol, IPEndPoint clientEndpoint)
        {
            bool result = true;
            _subscriptions.AddOrUpdate (symbol,
                _ => new HashSet<IPEndPoint> { clientEndpoint },
                (_, existingClients) =>
                {
                    if (!existingClients.Contains (clientEndpoint))
                    {
                        existingClients.Add (clientEndpoint); 
                    }
                    else
                    {
                        result = false;
                    }
                    return existingClients;
                });
            return result;
        }
        
        // 客戶端取消訂閱（可選）
        public void Unsubscribe (string symbol, IPEndPoint clientEndpoint)
        {
            if (_subscriptions.TryGetValue (symbol, out var clients))
            {
                clients.Remove (clientEndpoint);
                if (clients.Count == 0)
                    _subscriptions.TryRemove (symbol, out _);
            }
        }

        public void RemoveClient (IPEndPoint clientEndpoint)
        {
            foreach (var symbol in _subscriptions.Keys.ToList ())
            {
                _subscriptions[symbol].Remove (clientEndpoint);
                if (_subscriptions[symbol].Count == 0)
                    _subscriptions.TryRemove (symbol, out _);
            }
        }

        // 取得有訂閱這檔股票的用戶
        public HashSet<IPEndPoint> GetSubscriber (string symbol)
        {
            if (_subscriptions.TryGetValue (symbol, out var streams))
            {
                return streams;
            }
            
            return new HashSet<IPEndPoint> ();    
        }
    }
}
