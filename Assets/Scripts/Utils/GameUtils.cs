
using UnityEngine;

namespace Utils
{
    public static class GameUtils
    {
        public static bool Drop(int probability)
        {
            return Random.Range(0, 100) < probability;
        }
    }
}