using UnityEngine;
using Unity.Entities;

public class BoostComponent : MonoBehaviour
{
    public float BoostMultiplier = 2f;
    public GameObject BoostParticlePrefab;
}

public struct BoostData : IComponentData
{
    public float BoostMultiplier;
    public Entity BoostParticlePrefab;
    public bool IsBoostActive;
}

class BoostBaker : Baker<BoostComponent>
{
    public override void Bake(BoostComponent authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        var data = new BoostData
        {
            BoostMultiplier = authoring.BoostMultiplier,
            BoostParticlePrefab = GetEntity(authoring.BoostParticlePrefab, TransformUsageFlags.Dynamic),
            IsBoostActive = false
        };
        AddComponent(entity, data);
    }
}
