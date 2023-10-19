using System;
using System.Numerics;
using Vortice.Mathematics;

namespace Common.Structures
{
    public enum BaseEntityType { Other, Player, Bot, LootItem, Box, Car, Boss , Bullet}
    public class BaseEntity : MemoryObject
    {
        public Vector3 WordPosition;
        public BaseEntityType Type;
        public string TypeName;
        public string Name;
        public IntPtr PositionAdress;
    }
}
