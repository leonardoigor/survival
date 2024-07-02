using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scenes.Connect.Scripts
{


    public struct GameObjectReferenceComponent : IComponentData
    {
        public GameObject GameObject;
    }



    public class GameObjectConversion : MonoBehaviour
    {
        public EntityManager entityManager;
        public Entity entity;

        void Start()
        {
            // Example: Create an ECS entity with required components
            entity = entityManager.CreateEntity(
                ComponentType.ReadOnly<RenderMesh>()
            );

            // Example: Convert ECS entity to a GameObject
            ConvertEntityToGameObject(entity);
        }

        void ConvertEntityToGameObject(Entity entity)
        {
            // Example: Create a new GameObject and attach necessary components
            GameObject go = new GameObject("EntityGameObject");



            if (entityManager.HasComponent<RenderMesh>(entity))
            {
                var renderMesh = entityManager.GetSharedComponentManaged<RenderMesh>(entity);
                var meshFilter = go.AddComponent<MeshFilter>();
                meshFilter.sharedMesh = renderMesh.mesh;

                var meshRenderer = go.AddComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = renderMesh.material;
            }
        }
    }


}
