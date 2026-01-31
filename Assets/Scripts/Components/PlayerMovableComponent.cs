using UnityEngine;
using Unity.Entities;

public class PlayerMovableComponent : MonoBehaviour
{
    public float Speed = 5f;
    public float RotationSpeed = 5f;
}

public struct PlayerMovableComponentData : IComponentData
{
    public float Speed;
    public float RotationSpeed;
}

class PlayerMovableBaker : Baker<PlayerMovableComponent>
{
    public override void Bake(PlayerMovableComponent authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        var data = new PlayerMovableComponentData
        {
            Speed = authoring.Speed,
            RotationSpeed = authoring.RotationSpeed
        };
        AddComponent(entity, data);
    }
}
