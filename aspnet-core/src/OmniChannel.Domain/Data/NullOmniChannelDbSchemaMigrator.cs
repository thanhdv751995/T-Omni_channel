using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace OmniChannel.Data;

/* This is used if database provider does't define
 * IOmniChannelDbSchemaMigrator implementation.
 */
public class NullOmniChannelDbSchemaMigrator : IOmniChannelDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
