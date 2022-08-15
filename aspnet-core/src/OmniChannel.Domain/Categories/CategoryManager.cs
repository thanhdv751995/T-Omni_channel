using OmniChannel.Channels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace OmniChannel.Categories
{
    public class CategoryManager : DomainService
    {
        public CategoryManager()
        {
        }
        public Category CreateAsync(
                EChannel e_Channel,
                string category_Id,
                string client_category_id,
                [NotNull] string display_name,
                string parent_id
           )
        {
            return new Category(
               GuidGenerator.Create(),
               e_Channel,
               category_Id,
               client_category_id,
               display_name,
               parent_id
                
            ) ;
        }
    }
}
