using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Finances.Settlements
{
    public class ResponseGetOrderSettlementsDto
    {
        public List<SettlementDto> settlement_list { get; set; }
    }
}
