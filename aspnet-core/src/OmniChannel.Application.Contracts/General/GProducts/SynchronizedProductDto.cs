using OmniChannel.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.General.GProducts
{
    public class SynchronizedProductDto
    {
        /// <summary>
        /// id sản phảm của channel
        /// </summary>
        public string Channel_product_id { get; set; }
        /// <summary>
        /// id sản phẩm của tpos
        /// </summary>
        public object Client_data { get; set; }

    }
}
