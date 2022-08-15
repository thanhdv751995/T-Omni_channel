using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Services;

namespace OmniChannel.General.Connects
{
    public interface IConnectDBAppService: IApplicationService
    {
        IMongoDatabase ConnectDB();
    }
}
