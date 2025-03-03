using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.Services.Multiplay;
using Unity.Services.Multiplay.Apis.GameServer;
using Unity.Services.Multiplay.Apis.Payload;
using Unity.Services.Multiplay.Http;
using Unity.Ucg.Usqp;
using UnityEngine;

namespace Unity.Services.Multiplay.Internal
{
    internal class WrappedMultiplayService : IMultiplayService
    {
        private readonly IMultiplayServiceSdk m_MultiplayServiceSdk;

        public ServerConfig ServerConfig { get; private set; }

        public WrappedMultiplayService(IMultiplayServiceSdk serviceSdk, ServerConfig serverConfig)
        {
            m_MultiplayServiceSdk = serviceSdk;
            ServerConfig = serverConfig;

            //Overwrite localhost to 127.0.0.1:8086 as Multiplay doesn't connect otherwise
            Configuration config = new Configuration("http://127.0.0.1:8086", 10, 4, null);
            var gameServerClient = serviceSdk.GameServerApi as GameServerApiClient;
            var payloadClient = serviceSdk.PayloadApi as PayloadApiClient;

            if (gameServerClient != null)
            {
                gameServerClient.Configuration = config;
            }

            if (payloadClient != null)
            {
                payloadClient.Configuration = config;
            }
        }

        public async Task ReadyServerForPlayersAsync()
        {
            if (string.IsNullOrWhiteSpace(ServerConfig.AllocationId))
            {
                throw new InvalidOperationException("Attempting to be ready for players, but the server has not been allocated yet. You must wait for an allocation.");
            }
            if (!Guid.TryParse(ServerConfig.AllocationId, out var allocationGuid))
            {
                throw new InvalidOperationException($"Unable to parse Allocation ID[{ServerConfig.AllocationId}] from {nameof(ServerConfig)}!");
            }
            var request = new GameServer.ReadyServerRequest(ServerConfig.ServerId, allocationGuid);
            await m_MultiplayServiceSdk.GameServerApi.ReadyServerAsync(request);
        }

        public async Task UnreadyServerAsync()
        {
            var request = new GameServer.UnreadyServerRequest(ServerConfig.ServerId);
            await m_MultiplayServiceSdk.GameServerApi.UnreadyServerAsync(request);
        }

        public async Task<IServerEvents> SubscribeToServerEventsAsync(MultiplayEventCallbacks callbacks)
        {
            var serverId = ServerConfig.ServerId;
            var channel = m_MultiplayServiceSdk.WireDirect.CreateChannel($"ws://127.0.0.1:8086/v1/connection/websocket", new MultiplaySdkDaemonTokenProvider(serverId));
            channel.MessageReceived += (message) => OnMessageReceived(callbacks, message);
            MultiplayServerEvents events = new MultiplayServerEvents(channel, callbacks);
            await channel.SubscribeAsync();
            return events;
        }

        public async Task<string> GetPayloadAllocationAsPlainText()
        {
            if (Guid.TryParse(ServerConfig.AllocationId, out var allocationGuid))
            {
                var request = new Payload.PayloadAllocationRequest(allocationGuid);
                try
                {
                    var response = await m_MultiplayServiceSdk.PayloadApi.PayloadAllocationAsync(request);
                    return response.Result;
                }
                catch (ResponseDeserializationException ex)
                {
                    Debug.Log($"Failed to deserialize response Data[{Encoding.UTF8.GetString(ex.response.Data)}]!");
                    throw;
                }
            }
            throw new InvalidOperationException($"Attempting to get payload allocation when the Allocation ID[{ServerConfig.AllocationId}] is not valid!");
        }

        public async Task<TPayload> GetPayloadAllocationFromJsonAs<TPayload>(bool throwOnMissingMembers = false)
        {
            // Ideally we'd use the SDK Generator to get a response from an application/json endpoint,
            // sadly the SDK Generator does not support multiple content types for the same endpoint
            var payloadAllocation = await GetPayloadAllocationAsPlainText();
            var jsonDeserializerSettings = new JsonSerializerSettings() { MissingMemberHandling = throwOnMissingMembers ? Newtonsoft.Json.MissingMemberHandling.Error : Newtonsoft.Json.MissingMemberHandling.Ignore };
            var payload = JsonConvert.DeserializeObject<TPayload>(payloadAllocation, jsonDeserializerSettings);
            return payload;
        }

        public async Task<IServerQueryHandler> StartServerQueryHandlerAsync(ushort maxPlayers, string serverName, string gameType, string buildId, string map)
        {
            return await ConnectToServerCheckAsync(maxPlayers, serverName, gameType, buildId, map, ServerConfig.QueryPort);
        }

        public Task<IServerQueryHandler> ConnectToServerCheckAsync(ushort maxPlayers, string serverName, string gameType, string buildId, string map, ushort port)
        {
            var serverCheckManager = new ServerQueryHandler(maxPlayers, serverName, gameType, buildId, map);
            serverCheckManager.Connect(port);
            return Task.FromResult((IServerQueryHandler)serverCheckManager);
        }

        private void OnMessageReceived(MultiplayEventCallbacks callbacks, string message)
        {
            MultiplayServiceLogging.Verbose($"Received Message[{message}]");
            var jObject = JObject.Parse(message);
            var eventTypeJObject = jObject.SelectToken("EventType");
            if (eventTypeJObject == null)
            {
                // Due to a typo in the early versions of PayloadProxy, "EventType" might not exist. Instead, we check for EventTyp.
                // We can probably remove this at a later date.
                MultiplayServiceLogging.Verbose("EventTypeJObject[EventType] not found. Trying for EventTyp!");
                eventTypeJObject = jObject.SelectToken("EventTyp");
            }
            var eventTypeString = eventTypeJObject.ToObject<string>();
            if (Enum.TryParse<MultiplayEventType>(eventTypeString, out var eventType))
            {
                MultiplayServiceLogging.Verbose($"Handling {nameof(MultiplayEventType)}[{eventType}]");
                switch (eventType)
                {
                    case MultiplayEventType.AllocateEventType: callbacks.InvokeAllocate(CreateMultiplayAllocationFromJson(jObject)); break;
                    case MultiplayEventType.DeallocateEventType: callbacks.InvokeDeallocate(CreateMultiplayDeallocationFromJson(jObject)); break;
                    default: Debug.LogError($"Unhandled {nameof(MultiplayEventType)}[{eventType}]"); break;
                }
            }
            else
            {
                Debug.LogError($"Unrecognised {nameof(MultiplayEventType)}[{eventTypeString}]");
            }
        }

        private MultiplayAllocation CreateMultiplayAllocationFromJson(JObject jObject)
        {
            var eventId = jObject.SelectToken("EventID").ToObject<string>();
            var serverId = jObject.SelectToken("ServerID").ToObject<long>();
            var allocationId = jObject.SelectToken("AllocationID").ToObject<string>();
            MultiplayServiceLogging.Verbose($"Allocation Event: eventId[{eventId}] serverId[{serverId}] allocationId[{allocationId}]");
            ServerConfig = m_MultiplayServiceSdk.ServerConfigReader.LoadServerConfig();
            var allocation = new MultiplayAllocation(eventId, serverId, allocationId);
            return allocation;
        }

        private MultiplayDeallocation CreateMultiplayDeallocationFromJson(JObject jObject)
        {
            var eventId = jObject.SelectToken("EventID").ToObject<string>();
            var serverId = jObject.SelectToken("ServerID").ToObject<long>();
            var allocationId = jObject.SelectToken("AllocationID").ToObject<string>();
            MultiplayServiceLogging.Verbose($"Deallocation Event: eventId[{eventId}] serverId[{serverId}] allocationId[{allocationId}]");
            ServerConfig = m_MultiplayServiceSdk.ServerConfigReader.LoadServerConfig();
            var deallocation = new MultiplayDeallocation(eventId, serverId, allocationId);
            return deallocation;
        }
    }
}
