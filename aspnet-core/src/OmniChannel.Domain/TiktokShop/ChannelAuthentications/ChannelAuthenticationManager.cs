using OmniChannel.Channels;
using OmniChannel.Warehouses;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace OmniChannel.ChannelAuthentications
{
    public class ChannelAuthenticationManager : DomainService
    {
        public ChannelAuthenticationManager()
        {
        }
        public ChannelAuthentication CreateAsync(
              //  string id,
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
          )
        {
            return new ChannelAuthentication(
               GuidGenerator.Create(),
               channel_token,
               access_token,
               access_token_expire_in,
               refresh_token,
               refresh_token_expire_in,
               open_id,
               seller_name,
               isActive,
               shop_id,
               app,
               client_id,
               e_Channel,
               warehouse_list
            );
        }
    }
}
