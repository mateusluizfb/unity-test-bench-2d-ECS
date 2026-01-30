using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

partial struct CameraFollowSystem : ISystem
{
    private const float SmoothSpeed = 5f; // How smoothly the camera follows (higher = faster)
    private const float ZoomOutThreshold = 50f; // If camera size is above this, we're zoomed out
    
    public void OnUpdate(ref SystemState state)
    {
        var camera = Camera.main;
        if (camera == null)
            return;

        // Don't follow when zoomed out (viewing full map)
        if (camera.orthographicSize > ZoomOutThreshold)
            return;

        // Find the player entity with PlayerMovableComponentData
        foreach (var (transform, _) in 
                 SystemAPI.Query<RefRO<LocalTransform>, RefRO<PlayerMovableComponentData>>())
        {
            // Get player position
            var targetPosition = transform.ValueRO.Position;
            
            // Calculate desired camera position (keep Z unchanged for 2D)
            var desiredPosition = new Vector3(targetPosition.x, targetPosition.y, camera.transform.position.z);
            
            // Smoothly move camera to target position
            camera.transform.position = Vector3.Lerp(
                camera.transform.position,
                desiredPosition,
                SmoothSpeed * SystemAPI.Time.DeltaTime
            );
            
            // Only follow the first player found
            break;
        }
    }
}
