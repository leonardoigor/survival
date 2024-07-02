using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using UnityEngine;

public class ConnectionHander : MonoBehaviour
{
    [SerializeField] private string _portField = "7979";
    [SerializeField] private string _addressField = "127.0.0.1";
    [SerializeField] private string _worldName= "123";
    private ushort Port => ushort.Parse(_portField);
    private string Address => _addressField;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Host()
    {
        DestroyLocalSimulationWorld();
        StartServer();
        StartClient();
    }

    private static void DestroyLocalSimulationWorld()
    {
        foreach (var world in World.All)
        {
            if (world.Flags == WorldFlags.Game)
            {
                world.Dispose();
                break;
            }
        }
    }
    private void StartServer()
    {
        var serverWorld = ClientServerBootstrap.CreateServerWorld($"{_worldName} Server World");

        var serverEndpoint = NetworkEndpoint.AnyIpv4.WithPort(Port);
        {
            using var networkDriverQuery = serverWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
            networkDriverQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(serverEndpoint);
        }
    }
    private void StartClient()
    {
        var clientWorld = ClientServerBootstrap.CreateClientWorld($"{_worldName} Client World");

        var connectionEndpoint = NetworkEndpoint.Parse(Address, Port);
        {
            using var networkDriverQuery = clientWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
            networkDriverQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Connect(clientWorld.EntityManager, connectionEndpoint);
        }

        World.DefaultGameObjectInjectionWorld = clientWorld;
        SendPlayerLoginRequest.Singleton.isLogger=true;

    }
}
