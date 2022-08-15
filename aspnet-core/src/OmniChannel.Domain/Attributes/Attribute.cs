using OmniChannel.Attributes;
using OmniChannel.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OmniChannel.TiktokShop.Attributes
{
    public class Attribute : AuditedAggregateRoot<Guid>
    {
        public EChannel E_Channel { get; set; }
        public string Attribute_id { get; set; }
        public string Name { get; set; }
        public string Client_attribute_id { get; set; }

        private Attribute()
        {
            /* This constructor is for deserialization / ORM purpose */
        }
        internal Attribute(
                Guid id,
                EChannel e_Channel,
                string attribute_Id,
                string name,
                string client_attribute_id
           )
           : base(id)
        {
            E_Channel = e_Channel;
            Attribute_id = attribute_Id;
            Name = name;
            Client_attribute_id = client_attribute_id;
        }
    }
}
