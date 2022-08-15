using OmniChannel.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniChannel.General.Authenticatios
{
    public class GCreateAuthenticationDto
    {
        public EChannel E_Channel { get; set; }
        public string App_key { get; set; }
        public string App_secret { get; set; }
        public string Auth_code { get; set; }
        public string App { get; set; }
        public string Client_id { get; set; }
    }
}
