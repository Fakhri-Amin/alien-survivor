using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public struct CharacterMoveDirection : IComponentData
{
    public float2 Value;
}

public struct CharacterMoveSpeed : IComponentData
{
    public float Value;
}

public class CharacterAuthoring : MonoBehaviour
{
    public float MoveSpeed;

    public class Baker : Baker<CharacterAuthoring>
    {
        public override void Bake(CharacterAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<CharacterMoveDirection>(entity);
            AddComponent(entity, new CharacterMoveSpeed()
            {
                Value = authoring.MoveSpeed
            });
        }
    }
}

public partial struct CharacterMoveSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (velocity, direction, speed) in SystemAPI.Query<RefRW<PhysicsVelocity>, CharacterMoveDirection, CharacterMoveSpeed>())
        {
            var moveStep2d = speed.Value * direction.Value;
            velocity.ValueRW.Linear = new float3(moveStep2d, 0f);
        }
    }
}
