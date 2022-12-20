using System;
using System.Data.SqlTypes;
using UnityEngine;

namespace Grids
{
    public class HexCell
    {
        public Vector2Int GridPosition { get; }
        public Vector3 WorldPosition { get; }
        public bool Traversable { get; }

     
        
        public HexCell(Vector2Int gridPosition, Vector3 worldPosition, bool traversable)
        {
            GridPosition = gridPosition;
            WorldPosition = worldPosition;
            Traversable = traversable;
        }
    }
}
