using UnityEngine;
using Unity.Entities;

public class AsteroidSpawnerComponent : MonoBehaviour
{
    public GameObject AsteroidPrefab;
    public float SpawnInterval = 2f;
    public float SpawnRadius = 10f;
    public Vector2 SpawnAreaSize = new Vector2(20f, 20f);

    class AsteroidSpawnerBaker : Baker<AsteroidSpawnerComponent>
    {
        public override void Bake(AsteroidSpawnerComponent authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            var data = new AsteroidSpawnerData
            {
                AsteroidPrefab = GetEntity(authoring.AsteroidPrefab, TransformUsageFlags.Dynamic),
                SpawnInterval = authoring.SpawnInterval,
                SpawnRadius = authoring.SpawnRadius,
                NextSpawnTime = 0f,
                SpawnAreaSize = authoring.SpawnAreaSize
            };
            AddComponent(entity, data);
        }
    }
}

public struct AsteroidSpawnerData : IComponentData
{
    public Entity AsteroidPrefab;
    public float SpawnInterval;
    public float SpawnRadius;
    public float NextSpawnTime;
    public Vector2 SpawnAreaSize;
}
