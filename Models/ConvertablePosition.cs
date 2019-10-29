using UnityEngine;

namespace RestoreMonarchy.MoreHomes.Models
{
    public class ConvertablePosition
    {
        public ConvertablePosition() { }
        public ConvertablePosition(Vector3 vector)
        {
            X = vector.x;
            Y = vector.y;
            Z = vector.z;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }
    }
}
