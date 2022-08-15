using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.Authentications
{
    public class ReponseGetAuthenDto
    {
        public int code { get; set; }
        public string message { get; set; }
        public DataReponseGetAccessTokenDto data { get; set; }
    }
}
