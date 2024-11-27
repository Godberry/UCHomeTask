using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct STradeDetail
{
    public string Stock { get; init; }          // 商品名稱
    public DateTime Timestamp { get; init; }    // 時間戳記
    public decimal Price { get; init; }         // 價格
    public int Volume { get; init; }            // 成交張數
    public ulong SerialNo { get; init; }        // 序列號
}
