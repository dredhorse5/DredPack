

using UnityEngine;

namespace DredPack.Help
{
    public static class VectorHelp
    {
        public static float InverseLerp(this Vector3 vec ,Vector3 a, Vector3 b) => Vector3InverseLerp(a, b, vec);
        public static float Vector3InverseLerp(Vector3 a, Vector3 b, Vector3 value)
        {
            Vector3 AB = b - a;
            Vector3 AV = value - a;
            return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
        }
        
        public static float InverseLerp(this Vector2 vec ,Vector2 a, Vector2 b) => Vector2InverseLerp(a, b, vec);
        public static float Vector2InverseLerp(Vector2 a, Vector2 b, Vector2 value)
        {
            Vector2 AB = b - a;
            Vector2 AV = value - a;
            return Vector2.Dot(AV, AB) / Vector2.Dot(AB, AB);
        }
    }
}