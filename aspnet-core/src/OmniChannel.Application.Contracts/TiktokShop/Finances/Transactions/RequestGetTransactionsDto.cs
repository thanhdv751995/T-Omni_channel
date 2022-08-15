using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Finances.Transactions
{
    public class RequestGetTransactionsDto
    {
        public int? request_time_from { get; set; }
        public int? request_time_to { get; set; }
        public List<int> transaction_type { get; set; }
        public int page_size { get; set; }
        public int offset { get; set; }
    }
}
