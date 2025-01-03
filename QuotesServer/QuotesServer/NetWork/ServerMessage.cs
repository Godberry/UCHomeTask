﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesServer
{
    public class ReceiveTickersMessage : NetWorkMessage
    {
        public string stokName { get; set; }
        public List<STradeDetail> tickers { get; set; }

        public ReceiveTickersMessage (string stokName, List<STradeDetail> tickers)
        {
            this.stokName = stokName;
            this.tickers = tickers;
        }
    }
    public class UpdateQuotesMessage : NetWorkMessage
    {
        public Quotes quotes { get; set; }
        public UpdateQuotesMessage (Quotes quotes)
            : base ("UpdateQuotes")
        {
            this.quotes = quotes;
        }
    }
}
