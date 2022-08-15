using OmniChannel.Attributes;
using OmniChannel.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace OmniChannel.TiktokShop.Attributes
{
    public class AttributeManager : DomainService
    {
        public AttributeManager()
        {
        }
        public Attribute CreateAsync(
               EChannel e_Channel,
               string attribute_Id,
               string name,
               string client_attribute_id
           )
        {
            return new Attribute(
               GuidGenerator.Create(),
               e_Channel,
               attribute_Id,
               name,
               client_attribute_id
            );
        }
    }
}
