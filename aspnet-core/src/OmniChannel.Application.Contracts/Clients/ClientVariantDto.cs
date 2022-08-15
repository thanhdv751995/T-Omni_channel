using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Clients
{
    public class ClientVariantDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public long Weight { get; set; }
        public string ImageUrl { get; set; }
        public string Code { get; set; }
        public string Price { get; set; }
        public bool Active { get; set; }
        public List<AttributeValueDto> AttributeValues { get; set; }

    }
}
