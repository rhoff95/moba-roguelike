using UnityEngine;

namespace Grids
{
    public class HexCell
    {
        public Vector2Int GridPosition { get; }
        public Vector3 WorldPosition { get; }

        public HexCell(Vector2Int gridPosition, Vector3 worldPosition)
        {
            GridPosition = gridPosition;
            WorldPosition = worldPosition;
        }
    }
}
