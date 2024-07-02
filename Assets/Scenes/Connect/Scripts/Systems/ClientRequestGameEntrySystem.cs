using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scenes.Connect.Scripts.Systems
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
    public partial struct ClientRequestGameEntrySystem : ISystem
    {
        private EntityQuery _pendingNetworkIdQuery;

        void OnCreate(ref SystemState state) {
            var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<NetworkId>()
                .WithNone<NetworkStreamInGame>();

            _pendingNetworkIdQuery = state.GetEntityQuery(builder);
            state.RequireForUpdate(_pendingNetworkIdQuery);
            state.RequireForUpdate<ClientRequest>();
        }
        void OnUpdate(ref SystemState state) {
            var requestClient = SystemAPI.GetSingleton<ClientRequest>();
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var pedingNetworkIds=_pendingNetworkIdQuery.ToEntityArray(Allocator.Temp);


            foreach (var pedingNetoworkId in pedingNetworkIds)
            {
                ecb.AddComponent<NetworkStreamInGame>(pedingNetoworkId);
                var requestEntry = ecb.CreateEntity();
                ecb.AddComponent(requestEntry, new SendRpcCommandRequest
                {
                    TargetConnection = pedingNetoworkId
                });

                ecb.AddComponent<ClientRequestRpc>(requestEntry);
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }

}
