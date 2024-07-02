using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Assets.Scenes.Connect.Scripts.Systems
{

    public partial struct ServerProcessGameEntryRequestSystem : ISystem
    {
        void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<NetworkTime>();
            var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<ClientRequestRpc, ReceiveRpcCommandRequest>();

            state.RequireForUpdate(state.GetEntityQuery(builder));
            state.RequireForUpdate<PrefabComponent>();

        }
        void OnUpdate(ref SystemState state)
        {

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            var prefabs = SystemAPI.GetSingleton<PrefabComponent>();


            foreach (var (request, requestSource, requestEntity) in SystemAPI.Query<ClientRequestRpc, ReceiveRpcCommandRequest>()
                                          .WithEntityAccess())
            {
                ecb.DestroyEntity(requestEntity);
                ecb.AddComponent<NetworkStreamInGame>(requestSource.SourceConnection);

                var clientId = SystemAPI.GetComponent<NetworkId>(requestSource.SourceConnection).Value;

                Debug.Log($"Server is assigning Client ID: {clientId}");
                float3 SpawnPosition = float3.zero;

              var newPlayer=ecb.Instantiate(prefabs.SodierEntity);
                if (EntityManager.HasComponent<GameObjectComponentData>(newPlayer))
                {

                }

                    ecb.SetName(newPlayer, $"Player({clientId})");

                ecb.SetComponent(newPlayer, LocalTransform.FromPositionRotation(SpawnPosition, quaternion.identity));
                ecb.SetComponent(newPlayer, new GhostOwner { NetworkId = clientId });
                ecb.AppendToBuffer(requestSource.SourceConnection, new LinkedEntityGroup
                {
                    Value = newPlayer
                });



            }
            ecb.Playback(state.EntityManager);

        }
    }

}
