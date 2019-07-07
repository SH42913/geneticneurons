using System;

namespace FarCo.GeneticNeurons.Utils
{
    public static class RandomExtensions
    {
        public static void FillArrayWithRandom(this Random random, float[] array, float minValue, float maxValue)
        {
            float range = Math.Abs(minValue - maxValue);
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (float) (minValue + random.NextDouble() * range);
            }
        }
    }
}