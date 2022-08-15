using System.Threading.Tasks;

namespace OmniChannel.Data;

public interface IOmniChannelDbSchemaMigrator
{
    Task MigrateAsync();
}
