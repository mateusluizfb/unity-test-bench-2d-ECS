using UnityEngine;
using Unity.Entities;

public class BoostComponent : MonoBehaviour
{
    public float BoostMultiplier = 2f;

    class BoostBaker : Baker<BoostComponent>
    {
        public override void Bake(BoostComponent authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var data = new BoostData
            {
                BoostMultiplier = authoring.BoostMultiplier
            };
            AddComponent(entity, data);
        }
    }
}

public struct BoostData : IComponentData
{
    public float BoostMultiplier;
}
