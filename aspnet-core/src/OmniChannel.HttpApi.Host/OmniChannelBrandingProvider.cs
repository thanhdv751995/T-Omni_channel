using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace OmniChannel;

[Dependency(ReplaceServices = true)]
public class OmniChannelBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "OmniChannel";
}
