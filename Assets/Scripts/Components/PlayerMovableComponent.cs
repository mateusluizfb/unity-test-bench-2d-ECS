using UnityEngine;
using Unity.Entities;

public class PlayerMovableComponent : MonoBehaviour
{
    public float Speed = 5f;

    class PlayerMovableBaker : Baker<PlayerMovableComponent>
    {
        public override void Bake(PlayerMovableComponent authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var data = new PlayerMovableComponentData
            {
                Speed = authoring.Speed
            };
            AddComponent(entity, data);
        }
    }
}

public struct PlayerMovableComponentData : IComponentData
{
    public float Speed;
}
