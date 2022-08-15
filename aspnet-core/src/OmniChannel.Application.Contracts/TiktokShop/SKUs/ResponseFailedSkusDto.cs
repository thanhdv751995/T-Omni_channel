using System;
using System.Collections.Generic;
using System.Text;
using OmniChannel.Warehouses;

namespace OmniChannel.SKUs
{
    public class ResponseFailedSkusDto
    {
        public List<FailedWarehouseIdsDto> failed_skus { get; set; }
    }
}
