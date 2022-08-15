using OmniChannel.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.General.GProducts
{
    public class DeleteGProductDto
    {
        public EChannel E_channel { get; set; }
        public List<string> Client_product_ids { get; set; }
    }
}
