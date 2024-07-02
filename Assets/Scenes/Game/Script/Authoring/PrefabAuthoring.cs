using Assets.Scenes.Connect.Scripts;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine;






public class PrefabAuthoring : MonoBehaviour
{
    public GameObject SodierPrefab;
    class AuthoringBaker : Baker<PrefabAuthoring>
    {
        public override void Bake(PrefabAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PrefabComponent
            {
                SodierEntity = GetEntity(authoring.SodierPrefab, TransformUsageFlags.Dynamic)
            });
        }
    }
}
