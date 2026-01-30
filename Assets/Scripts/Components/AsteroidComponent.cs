using UnityEngine;
using Unity.Entities;

public class AsteroidComponent : MonoBehaviour
{
    public float Speed = 5f;
}

public struct AsteroidComponentData : IComponentData
{
    public float Speed;
    public float angle;
}

public class AsteroidBaker : Baker<AsteroidComponent>
{
    public override void Bake(AsteroidComponent authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new AsteroidComponentData 
        { 
            Speed = authoring.Speed,
            angle = 0f // Will be set when spawned
        });
    }
}
