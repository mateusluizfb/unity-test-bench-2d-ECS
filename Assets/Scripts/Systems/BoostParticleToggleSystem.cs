using Unity.Entities;
using Unity.Rendering;

partial struct BoostParticleToggleSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        float deltaTime = SystemAPI.Time.DeltaTime;
        float transitionSpeed = 5f; // Speed of transition for particle effect intensity
        
        // Update target intensity for each boost entity
        foreach (var boostData in SystemAPI.Query<RefRO<BoostData>>())
        {
            var particleEntity = boostData.ValueRO.BoostParticlePrefab;
            float targetIntensity = boostData.ValueRO.IsBoostActive ? 1f : 0f;
            
            // Ensure the particle entity has ParticleIntensity component
            if (!state.EntityManager.HasComponent<ParticleIntensity>(particleEntity))
            {
                ecb.AddComponent(particleEntity, new ParticleIntensity { Value = 0f, TargetValue = targetIntensity });
            }
            else
            {
                // Update target intensity
                ecb.SetComponent(particleEntity, new ParticleIntensity 
                { 
                    Value = state.EntityManager.GetComponentData<ParticleIntensity>(particleEntity).Value,
                    TargetValue = targetIntensity 
                });
            }
        }
        
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
        
        // Smoothly interpolate and apply intensity to particle systems
        foreach (var (intensity, entity) in SystemAPI.Query<RefRW<ParticleIntensity>>().WithEntityAccess())
        {
            // Smoothly interpolate current value towards target
            intensity.ValueRW.Value = Unity.Mathematics.math.lerp(
                intensity.ValueRO.Value, 
                intensity.ValueRO.TargetValue, 
                deltaTime * transitionSpeed
            );
            
            // Apply to the actual ParticleSystem component
            var ps = state.EntityManager.GetComponentObject<UnityEngine.ParticleSystem>(entity);
            if (ps != null)
            {
                var emission = ps.emission;
                emission.rateOverTime = intensity.ValueRO.Value * 50f; // Adjust multiplier as needed
            }
        }
    }
}
