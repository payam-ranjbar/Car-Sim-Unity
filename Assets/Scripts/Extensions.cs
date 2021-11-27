
    using UnityEngine;

    public static class Extensions
    {
        public static Vector3 GetWithXZ(this Vector3 vector, float x, float z)
        {
            return new Vector3(x, vector.y, z);
        }
               
        public static Vector3 GetWithY(this Vector3 vector, float y )
        {
            return new Vector3(vector.x, y, vector.z);
        }
             
        public static Vector3 GetWithX(this Vector3 vector, float x)
        {
            return new Vector3(x, vector.y, vector .z);
        }
                     
        public static Vector3 GetWithZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }                     
        public static Vector2 Vec2FromXZ(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }

        public static bool InRangeFrom2D(this Vector3 vector, Vector3 dest, float distance)
        {
            return Vector2.Distance(vector.Vec2FromXZ(), dest.Vec2FromXZ()) <= distance;
        }

        public static bool InRangeFrom(this Vector3 vector, Vector3 dest, float distance)
        {
            return Vector3.Distance(dest, vector) <= distance;

        }
        public static bool InRangeFrom(this Vector2 vector, Vector2 dest, float distance)
        {
            return Vector2.Distance(dest, vector) <= distance;

        }
                             
        public static float DefaultIfZero(this float f, float z)
        {
            return f > 0 ? f : z;
        }
        
        
        
        
    }
