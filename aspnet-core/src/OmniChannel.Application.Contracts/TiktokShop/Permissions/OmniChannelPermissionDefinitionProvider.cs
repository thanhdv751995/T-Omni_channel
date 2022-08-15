using OmniChannel.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace OmniChannel.Permissions;

public class OmniChannelPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(OmniChannelPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(OmniChannelPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<OmniChannelResource>(name);
    }
}
