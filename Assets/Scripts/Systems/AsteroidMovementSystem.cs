using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.InputSystem;

partial struct AsteroidMovementSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (transform, movement) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<AsteroidComponentData>>())
        {
            float angleInRadians = math.radians(movement.ValueRO.angle);
            float3 direction = new float3(
                math.cos(angleInRadians),
                math.sin(angleInRadians),
                0
            );
            transform.ValueRW.Position += direction * movement.ValueRO.Speed * deltaTime;
        }
    }
}
