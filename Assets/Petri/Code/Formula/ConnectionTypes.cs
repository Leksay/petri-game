using System;
using UnityEngine;

namespace Petri.Formula
{
    [Flags]
    public enum ConnectionTypes
    {
        None = 0,
        Up = 1 << 0,
        Right = 1 << 1,
        Down = 1 << 2,
        Left = 1 << 3,
    }

    public static class ConnectionTypesExtensions
    {
        public static bool IsEmpty(this ConnectionTypes connectionType)
        {
            return connectionType == ConnectionTypes.None;
        }

        public static ConnectionTypes Inverse(this ConnectionTypes connectionType)
        {
            return connectionType switch
            {
                ConnectionTypes.Up => ConnectionTypes.Down,
                ConnectionTypes.Right => ConnectionTypes.Left,
                ConnectionTypes.Down => ConnectionTypes.Up,
                ConnectionTypes.Left => ConnectionTypes.Right,
                ConnectionTypes.None => ConnectionTypes.None,
                _ => throw new ArgumentOutOfRangeException(nameof(connectionType), connectionType, null)
            };
        }

        public static bool IsDown(this ConnectionTypes connectionType)
        {
            return (connectionType & ConnectionTypes.Down) != 0;
        }
        
        public static bool IsUp(this ConnectionTypes connectionType)
        {
            return (connectionType & ConnectionTypes.Up) != 0;
        }
        
        public static bool IsLeft(this ConnectionTypes connectionType)
        {
            return (connectionType & ConnectionTypes.Left) != 0;
        }
        
        public static bool IsRight(this ConnectionTypes connectionType)
        {
            return (connectionType & ConnectionTypes.Right) != 0;
        }

        public static ConnectionTypes CalculateByCoords(Vector2Int start, Vector2Int end)
        {
            var x = end.x - start.x;
            var y = end.y - start.y;

            if (x == 0 && y == 0)
                return ConnectionTypes.None;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                return x > 0 ? ConnectionTypes.Right : ConnectionTypes.Left;
            }

            return y > 0 ? ConnectionTypes.Up : ConnectionTypes.Down;
        }
    }
}