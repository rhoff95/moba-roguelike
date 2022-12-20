using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grids
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Grid grid;
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TilemapCollider2D tilemapCollider2D;
        [SerializeField] private CompositeCollider2D compositeCollider2D;

        private HexGrid _hexGrid = new HexGrid();

        private void Start()
        {
            var colliderBounds = compositeCollider2D.bounds;

            var v = colliderBounds.max.x;
            var xBoundsMax = colliderBounds.max.x;
            var xBoundsMin = colliderBounds.min.x;
            
            var yBoundsMax = colliderBounds.max.y;
            var yBoundsMin = colliderBounds.min.y;

            var cellSize = grid.cellSize;
            var xMax = xBoundsMax - (xBoundsMax % (cellSize.x / 2f));
            var yMax = yBoundsMax - (yBoundsMax % (cellSize.y *(3/4f)));

            var rowIndex = 0;
            for (var row = yMax; row > yBoundsMin; row -= cellSize.y * (3/4f))
            {
                var even = row % (cellSize.y * (3 / 2f)) == 0f;

                var offset = even ? 0f : cellSize.x / 2f;

                var columnIndex = 0;
                for (var x = xMax - offset; x > xBoundsMin; x -= cellSize.x)
                {
                    var cellWorldPosition2 = new Vector2(x, row);

                    // TODO Move this to the grid, ask the grid
                    var overlapPoint = Physics2D.OverlapPoint(cellWorldPosition2);

                    _hexGrid.AddCell(
                        cellWorldPosition2,
                        new Vector2Int(rowIndex, columnIndex),
                        overlapPoint == null);

                    columnIndex++;
                }

                rowIndex++;
            }
        }

        public Queue<HexCell> GetPath(Vector3 worldPosition)
        {
            return _hexGrid.GetPath(worldPosition);
        }

        private void OnDrawGizmos()
        {
            _hexGrid.AllCells.ForEach(cell =>
            {
                Gizmos.color = cell.Traversable ? Color.green : Color.red;
                Gizmos.DrawWireSphere(cell.WorldPosition, 0.1f);
            });
        }
    }
}
