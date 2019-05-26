using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TeamSpeak3.Metrics.Configuration;

namespace TeamSpeak3.Metrics.v2
{
    public class Gateway
    {
        private const string ClientlistCommand = "clientlist";
        private const string ServerInfoCommand = "serverinfo";
        private const string ServerListCommand = "serverlist";

        private const string NewLine = "\n\r";

        private static readonly string[] Separator = { NewLine };

        private readonly IQueryConnectionFactory _factory;
        private readonly ILogger<Gateway> _logger;
        private readonly ServerConfiguration _config;

        public Gateway(IQueryConnectionFactory factory, ILogger<Gateway> logger, ServerConfiguration config)
        {
            _factory = factory;
            _logger = logger;
            _config = config;
        }

        public async Task<IEnumerable<Client>> GetClientList(int virtualServerPort)
        {
            using (var connection = await _factory.Create(_config.Host, _config.QueryPort))
            {
                await Login(connection);
                await SelectVirtualServer(virtualServerPort, connection);

                var response = await connection.SendAndReceive(ClientlistCommand);
                return Parser.ToData<Client>(response);
            }
        }

        public async Task<ServerInfo> GetServerInfo(int virtualServerPort)
        {
            using (var connection = await _factory.Create(_config.Host, _config.QueryPort))
            {
                await Login(connection);
                await SelectVirtualServer(virtualServerPort, connection);

                var response = await connection.SendAndReceive(ServerInfoCommand);
                return Parser.ToData<ServerInfo>(response).FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Server>> GetServerList()
        {
            using (var connection = await _factory.Create(_config.Host, _config.QueryPort))
            {
                await Login(connection);

                var response = await connection.SendAndReceive(ServerListCommand);
                return Parser.ToData<Server>(response);
            }
        }

        private async Task SelectVirtualServer(int vServerPort, IQueryConnection connection)
        {
            var command = $"use port={vServerPort}";
            var useResponse = Parser.ToBooleanResponse(await connection.SendAndReceive(command));
            if (!useResponse.IsSuccess)
            {
                throw new Exception($"Couldn't select Server: '{useResponse.Message}'");
            }
        }

        private async Task Login(IQueryConnection connection)
        {
            var loginCmd = $"login {_config.QueryUsername} {_config.QueryPassword}";
            var loginResponse = Parser.ToBooleanResponse(await connection.SendAndReceive(loginCmd));
            if (!loginResponse.IsSuccess)
            {
                throw new Exception($"Couldn't login with query credentials '{loginResponse.Message}'");
            }
        }
    }
}
