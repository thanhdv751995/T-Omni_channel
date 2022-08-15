using System;
using System.Collections.Generic;
using System.Text;
using OmniChannel.Localization;
using Volo.Abp.Application.Services;

namespace OmniChannel;

/* Inherit your application services from this class.
 */
public abstract class OmniChannelAppService : ApplicationService
{
    protected OmniChannelAppService()
    {
        LocalizationResource = typeof(OmniChannelResource);
    }
}
