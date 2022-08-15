using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmniChannel.TiktokShop.Signal
{
    public class SignalR : Hub
    {
        public async Task SendMessage(string message)
            => await Clients.All.SendAsync("ReceiveMessage", message);
    }
}
