using UnityEngine;

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.InputSystem;

partial struct PlayerMovementSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return;
        
        float rotationZ = 0f;
        float vertical = 0f;

        if (keyboard.leftArrowKey.isPressed)
            rotationZ = 1f;
        else if (keyboard.rightArrowKey.isPressed)
            rotationZ = -1f;
            
        if (keyboard.upArrowKey.isPressed)
            vertical = 1f;
        else if (keyboard.downArrowKey.isPressed)
            vertical = -1f;

        foreach (var (transform, movement, boostData) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<PlayerMovableComponentData>, RefRW<BoostData>>())
        {
            transform.ValueRW.Rotation = math.mul(
                transform.ValueRW.Rotation,
                quaternion.EulerXYZ(new float3(0, 0, rotationZ * movement.ValueRO.RotationSpeed * deltaTime))
            );

            float3 forward = math.mul(transform.ValueRW.Rotation, new float3(0, 1, 0));

            if (keyboard.spaceKey.isPressed)
            {
                transform.ValueRW.Position += forward * movement.ValueRO.Speed * boostData.ValueRW.BoostMultiplier * vertical * deltaTime;
                boostData.ValueRW.IsBoostActive = true;
            }
            else
            {
                transform.ValueRW.Position += forward * movement.ValueRO.Speed * vertical * deltaTime;
                boostData.ValueRW.IsBoostActive = false;
            }
        }
    }
}
