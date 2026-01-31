using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;

partial struct AsteroidSpawnSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float currentTime = (float)SystemAPI.Time.ElapsedTime;

        foreach (var (spawnerData, entity) in
                 SystemAPI.Query<RefRW<AsteroidSpawnerData>>().WithEntityAccess())
        {
            if (currentTime >= spawnerData.ValueRO.NextSpawnTime)
            {
                // Check if prefab entity is valid
                if (!state.EntityManager.Exists(spawnerData.ValueRO.AsteroidPrefab))
                {
                    UnityEngine.Debug.LogWarning("Asteroid prefab entity is not valid. Make sure the prefab is assigned in the spawner component.");
                    return;
                }

                // Spawn a new asteroid
                var newAsteroid = state.EntityManager.Instantiate(spawnerData.ValueRO.AsteroidPrefab);

                // Random angle for direction (0 to 360 degrees)
                float randomAngle = UnityEngine.Random.Range(0f, 360f);


                // random position within the camera view
                float2 spawnAreaSize = spawnerData.ValueRO.SpawnAreaSize;
                float2 randomPosition2D = new float2(
                    UnityEngine.Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
                    UnityEngine.Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f)
                );
                float3 position = new float3(randomPosition2D.x, randomPosition2D.y, 0f);

                // Set position to the random position within the circle
                var transform = SystemAPI.GetComponentRW<LocalTransform>(newAsteroid);
                transform.ValueRW.Position = position;

                // Set the asteroid's angle for movement direction
                var asteroidData = SystemAPI.GetComponentRW<AsteroidComponentData>(newAsteroid);
                asteroidData.ValueRW.angle = randomAngle;

                // Update next spawn time
                spawnerData.ValueRW.NextSpawnTime = currentTime + spawnerData.ValueRO.SpawnInterval;
            }
        }
    }
}
