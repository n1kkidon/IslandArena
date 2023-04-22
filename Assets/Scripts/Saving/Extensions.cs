using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Saving
{
    public static class Extensions
    {
        public static Vector3 ToVector3(this float[] position)
        {
            return new Vector3(position[0], position[1], position[2]);
        }
        public static float[] ToFloatArray(this Vector3 position)
        {
            var pos = new float[3];
            pos[0] = position.x;
            pos[1] = position.y;
            pos[2] = position.z;
            return pos;
        }
    }
}
