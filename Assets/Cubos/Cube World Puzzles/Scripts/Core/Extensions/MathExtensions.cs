using System;
using System.Collections.Generic;

namespace HashGame.CubeWorld.Extensions
{
    public static class MathExtensions
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
        public static void AddArrayToList<T>(this List<T> list, T[] array, bool includeNull = false)
        {
            if (array == null) return;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null && !includeNull)
                {
                    continue;
                }
                list.Add(array[i]);
            }
        }
        public static void ToShuffleArray<T>(this T[] array)
        {
            Random random = new Random();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                Swap<T>(ref array[i], ref array[j]);
            }
        }
    }
}