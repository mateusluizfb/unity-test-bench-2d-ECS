using UnityEngine;
using Unity.Entities;

public class ParticleIntensityComponent : MonoBehaviour
{
    public float InitialValue = 0f;
    public float TargetValue = 0f;
}

public struct ParticleIntensityData : IComponentData
{
    public float Value;
    public float TargetValue;
}

class ParticleIntensityBaker : Baker<ParticleIntensityComponent>
{
    public override void Bake(ParticleIntensityComponent authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        var data = new ParticleIntensityData
        {
            Value = authoring.InitialValue,
            TargetValue = authoring.TargetValue
        };
        AddComponent(entity, data);
    }
}
