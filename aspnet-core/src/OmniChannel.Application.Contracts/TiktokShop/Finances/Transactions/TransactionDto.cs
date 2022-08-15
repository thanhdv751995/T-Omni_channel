using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Finances.Transactions
{
    public class TransactionDto
    {
        public string transaction_amount { get; set; }
        public string transaction_currency { get; set; }
        public int transaction_status { get; set; }
        public long transaction_time { get; set; }
        public int transaction_type { get; set; }
    }
}
