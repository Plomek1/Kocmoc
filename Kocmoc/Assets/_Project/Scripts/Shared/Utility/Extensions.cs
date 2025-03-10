using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kocmoc
{
    public static class Extensions
    {
        public static T Previous<T>(this T value, bool skipIndexZero = false) where T : Enum
        {
            T[] values = (T[])Enum.GetValues(value.GetType());
            int index = Array.IndexOf(values, value) - 1;
            if (index < 0 || index == 0 && skipIndexZero) index = values.Length - 1;
            return values[index];
        }

        public static T Next<T>(this T value, bool skipIndexZero = false) where T : Enum
        {
            T[] values = (T[])Enum.GetValues(value.GetType());
            int index = Array.IndexOf(values, value) + 1;
            if (index == values.Length) index = 0;
            if (skipIndexZero && index == 0) index++;
            return values[index];
        }

        public static Rotation Opposite(this Rotation value)
        {
            switch(value)
            {
                case Rotation.Up:
                    return Rotation.Down;
                case Rotation.Down:
                    return Rotation.Up;
                case Rotation.Right:
                    return Rotation.Left;
                case Rotation.Left:
                    return Rotation.Right;
                default:
                    return Rotation.None;
            }
        }

        public static float ToInt(this Rotation value)
        {
            return Utility.TrailingZeroCount((int)value);
        }

        public static float ToAngle(this Rotation value)
        {
            return ToInt(value) * -90;
        }

        public static Vector2 ToVector(this Rotation value) => rotationVectors[value];

        public static Vector2 RightAngleRotate(this Vector2 value, Rotation angle)
        {
            switch (angle)
            {
                case Rotation.Right: return new Vector2( value.y, -value.x);
                case Rotation.Down:  return new Vector2(-value.x, -value.y);
                case Rotation.Left:  return new Vector2(-value.y,  value.x);
                default: return value;
            }
        }

        public static Vector2Int RightAngleRotate(this Vector2Int value, Rotation angle)
        {
            switch (angle)
            {
                case Rotation.Right: return new Vector2Int( value.y, -value.x);
                case Rotation.Down:  return new Vector2Int(-value.x, -value.y);
                case Rotation.Left:  return new Vector2Int(-value.y,  value.x);
                default: return value;
            }
        }

        private static Dictionary<Rotation, Vector2> rotationVectors = new Dictionary<Rotation, Vector2>() 
        {
            { Rotation.None,  Vector2.zero},
            { Rotation.Up,    Vector2.up },
            { Rotation.Right, Vector2.right },
            { Rotation.Down,  Vector2.down },
            { Rotation.Left,  Vector2.left },
        };
    }
}
