using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuotesClient
{
    public class NetWorkMessage
    {
        public string Type { get; set; } // request 類型
        public NetWorkMessage (string type)
        {
            Type = type;
        }
        public NetWorkMessage () 
        {
        }
    }
    public static class NetworkMessageParser
    {
        public static NetWorkMessage ParseMessage (string json)
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            var type = root.GetProperty("Type").GetString();

            return type switch
            {
                "SubscribeStocksSuccessMessage" => JsonSerializer.Deserialize<SubscribeStocksSuccessMessage> (json),
                _ => throw new InvalidOperationException ($"未知的消息類型: {type}")
            };
        }
    }
}
