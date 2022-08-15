using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Finances.Transactions
{
    public class ResponseGetTransactionsDto
    {
        public bool more { get; set; }
        public int total { get; set; }
        public List<TransactionDto> transaction_list { get; set; }
    }
}
