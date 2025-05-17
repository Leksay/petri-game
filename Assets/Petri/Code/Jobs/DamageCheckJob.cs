using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Petri.DamageCalculations
{
    [BurstCompile]
    public struct DamageCheckJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<BacteriasData> Bacterias;
        [ReadOnly] public NativeArray<float3> Drops;
        [ReadOnly] public float SquaredRadius;

        [WriteOnly] public NativeArray<BacteriaDamageResult> Results;
        
        public void Execute(int index)
        {
            var bacteriaPosition = Bacterias[index].Position;
            byte intersectionCount = 0;
            
            for (var i = 0; i < Drops.Length; i++)
            {
                var distance = math.distancesq(bacteriaPosition, Drops[i]);
                if (distance < SquaredRadius)
                {
                    intersectionCount++;
                }
            }

            Results[index] = new BacteriaDamageResult()
            {
                BacteriaEntity = Bacterias[index].BacteriaEntity,
                IntersectionCount = intersectionCount
            };
        }
    }

    public struct BacteriaDamageResult
    {
        public int BacteriaEntity;
        public byte IntersectionCount;
    }
    
    public readonly struct BacteriasData
    {
        public readonly float3 Position;
        public readonly int BacteriaEntity;

        public BacteriasData(int bacteriaEntity, float3 position)
        {
            BacteriaEntity = bacteriaEntity;
            Position = position;
        }
    }

    // public readonly struct DropsData
    // {
    //     public readonly float3 Position;
    //     public readonly float Radius;
    //     public readonly int DropEntity;
    // }
}