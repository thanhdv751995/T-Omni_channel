using OmniChannel.Channels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OmniChannel.Categories
{
    public class Category : AuditedAggregateRoot<Guid>
    {
        public EChannel E_Channel { get; set; }
        public string Category_Id { get; set; }
        public string Client_category_Id { get; set; }
        public string Display_name { get; set; }
        public string Parent_id { get; set; }

        private Category()
        {
            /* This constructor is for deserialization / ORM purpose */
        }
        internal Category(
               Guid id,
               EChannel e_Channel,
               string category_Id,
               string client_category_Id,
               [NotNull] string display_name,
                string parent_id
           )
           : base(id)
        {
            E_Channel = e_Channel;
            Category_Id = category_Id;
            Client_category_Id = client_category_Id;
            Display_name = display_name;
            Parent_id = parent_id;
        }
    }
   
}
