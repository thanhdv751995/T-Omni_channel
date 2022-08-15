using OmniChannel.Channels;
using OmniChannel.Warehouses;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace OmniChannel.ChannelAuthentications
{
    public class ChannelAuthentication : AuditedAggregateRoot<Guid> 
    {
        public string Channel_token { get; set; }
        public string Access_token { get; set; }
        public string Access_token_expire_in { get; set; }
        public string Refresh_token { get; set; }
        public string Refresh_token_expire_in { get; set; }
        public string Open_id { get; set; }
        public string Seller_name { get; set; }
        public string Shop_id { get; set; }
        public string App { get ; set; }
        public string Client_id { get; set; }
        public bool IsActive { get; set; }
        public EChannel E_Channel { get; set; }
        public List<WarehouseDto> Warehouse_list { get; set; }
        private ChannelAuthentication()
        {
            /* This constructor is for deserialization / ORM purpose */
        }
        internal ChannelAuthentication(
               Guid id,
               string channel_token,
               string access_token,
               string access_token_expire_in,
               string refresh_token,
               string refresh_token_expire_in,
              [NotNull] string open_id,
               string seller_name,
               bool isActive,
               string shop_id,
               string app,
               string client_id,
               EChannel e_Channel,
               List<WarehouseDto> warehouse_list
           ) :base(id)
          
        {
            Channel_token = channel_token;
            Access_token = access_token;
            Access_token_expire_in = access_token_expire_in;
            Refresh_token = refresh_token;
            Refresh_token_expire_in = refresh_token_expire_in;
            Open_id = open_id;
            Seller_name = seller_name;
            IsActive = isActive;
            Shop_id = shop_id;
            App = app;
            Client_id = client_id;
            E_Channel = e_Channel;
            Warehouse_list = warehouse_list;
        }
    }
}
