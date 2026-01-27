using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.InputSystem;

partial struct ArrowMovementSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return;
        
        float horizontal = 0f;
        float vertical = 0f;
        
        if (keyboard.leftArrowKey.isPressed)
            horizontal = -1f;
        else if (keyboard.rightArrowKey.isPressed)
            horizontal = 1f;
            
        if (keyboard.upArrowKey.isPressed)
            vertical = 1f;
        else if (keyboard.downArrowKey.isPressed)
            vertical = -1f;

        foreach (var (transform, movement) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<PlayerMovableComponentData>>())
        {
            transform.ValueRW.Position += new float3(horizontal, vertical, 0) * movement.ValueRO.Speed * deltaTime;
        }
    }
}
