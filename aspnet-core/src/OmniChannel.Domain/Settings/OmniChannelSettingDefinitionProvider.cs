using Volo.Abp.Settings;

namespace OmniChannel.Settings;

public class OmniChannelSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(OmniChannelSettings.MySetting1));
    }
}
