using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Finances
{
    public class ResponseGetSettlementsDto
    {
        public bool more { get; set; }
        public string next_cursor { get; set; }
        public List<SettlementDto> settlement_list { get; set; }
    }
}
