using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using OmniChannel.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace OmniChannel.General.Connects
{
    [RemoteService(false)]
    public class ConnectMongoDBAppService : OmniChannelAppService, IConnectDBAppService
    {
        private readonly IConfiguration _configuration;
        public ConnectMongoDBAppService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IMongoDatabase ConnectDB()
        {
            var mongoClient = new MongoClient(_configuration["MongoDBConnection:Connection"]);

            return mongoClient.GetDatabase(_configuration["MongoDBConnection:Database"]);
        }
    }
}
