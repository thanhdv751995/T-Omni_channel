using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.DeleteProducts
{
    public class ReponseDeleteProduct
    {
       public int code { get; set; }
       public string message { get; set; }
       public DataDeleteProductDto data { get; set; }
    }
}
