using Unity.Entities;
using Unity.NetCode;

namespace Assets.Scenes.Connect.Scripts
{

    public partial struct ClientRequest : IComponentData
    {
        public int Value;
    }
    public partial struct ClientRequestRpc : IRpcCommand
    {
    }


    public partial struct PrefabComponent : IComponentData
    {
        public Entity SodierEntity;
    }

}
