using OmniChannel.MongoDB;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace OmniChannel.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(OmniChannelMongoDbModule),
    typeof(OmniChannelApplicationContractsModule)
    )]
public class OmniChannelDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
