using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TeamSpeak3.Metrics.Common;
using TeamSpeak3.Metrics.Mapping;
using TeamSpeak3.Metrics.Models;

namespace TeamSpeak3.Metrics
{
    public interface IGateway
    {
        Task<IEnumerable<Client>> GetClientList(int virtualServerPort);

        Task<ServerInfo> GetServerInfo(int virtualServerPort);

        Task<IEnumerable<Server>> GetServerList();
    }

    public class Gateway : IGateway
    {
        private const string ClientListCommand = "clientlist";
        private const string ServerInfoCommand = "serverinfo";
        private const string ServerListCommand = "serverlist";

        private readonly IQueryConnectionFactory _factory;
        private readonly IOptionsMonitor<ServerOptions> _optionMonitor;

        public Gateway(IQueryConnectionFactory factory, IOptionsMonitor<ServerOptions> optionMonitor)
        {
            _factory = factory;
            _optionMonitor = optionMonitor;
        }

        public async Task<IEnumerable<Client>> GetClientList(int virtualServerPort)
        {
            var options = _optionMonitor.CurrentValue;

            using (var connection = await _factory.Create(options.Host, options.QueryPort))
            {
                await Login(connection);
                await SelectVirtualServer(virtualServerPort, connection);

                var response = await connection.SendAndReceive(ClientListCommand);
                var dataResponse = Mapper.ToData<Client>(response);
                if (!dataResponse.IsSuccess)
                {
                    ThrowMetricsException(dataResponse, ClientListCommand);
                }

                return dataResponse.Data;
            }
        }

        public async Task<ServerInfo> GetServerInfo(int virtualServerPort)
        {
            var options = _optionMonitor.CurrentValue;

            using (var connection = await _factory.Create(options.Host, options.QueryPort))
            {
                await Login(connection);
                await SelectVirtualServer(virtualServerPort, connection);

                var response = await connection.SendAndReceive(ServerInfoCommand);
                var dataResponse = Mapper.ToData<ServerInfo>(response);
                if (!dataResponse.IsSuccess)
                {
                    ThrowMetricsException(dataResponse, ServerInfoCommand);
                }

                return dataResponse.Data.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Server>> GetServerList()
        {
            var options = _optionMonitor.CurrentValue;

            using (var connection = await _factory.Create(options.Host, options.QueryPort))
            {
                await Login(connection);

                var response = await connection.SendAndReceive(ServerListCommand);
                var dataResponse = Mapper.ToData<Server>(response);
                if (!dataResponse.IsSuccess)
                {
                    ThrowMetricsException(dataResponse, ServerListCommand);
                }

                return dataResponse.Data;
            }
        }

        private async Task Login(IQueryConnection connection)
        {
            var options = _optionMonitor.CurrentValue;

            var loginCmd = $"login {options.QueryUsername} {options.QueryPassword}";
            var loginResponse = Mapper.ToStatusResponse(await connection.SendAndReceive(loginCmd));
            if (!loginResponse.IsSuccess)
            {
                throw new MetricsException($"Couldn't login with query credentials '{loginResponse.Message}'");
            }
        }

        private static async Task SelectVirtualServer(int vServerPort, IQueryConnection connection)
        {
            var command = $"use port={vServerPort}";
            var useResponse = Mapper.ToStatusResponse(await connection.SendAndReceive(command));
            if (!useResponse.IsSuccess)
            {
                throw new MetricsException($"Couldn't select Server: '{useResponse.Message}'");
            }
        }

        private static void ThrowMetricsException(QueryResponse dataResponse, string command)
        {
            throw new MetricsException($"Received error '{dataResponse.Id}: {dataResponse.Message}' when executing command '{command}'");
        }
    }
}