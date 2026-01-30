using Unity.Entities;

public struct ParticleIntensity : IComponentData
{
    public float Value;
    public float TargetValue;
}
