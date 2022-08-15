using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Clients
{
    public class ClientDataDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool Active { get; set; }
        public ClientUomDto Uom { get; set; }
        public ClientCategoryDto Category { get; set; }
        public List<ClientVariantDto> Variants { get; set; }
    }
}
