using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grids
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private Grid grid;
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TilemapCollider2D tilemapCollider2D;
        [SerializeField] private CompositeCollider2D compositeCollider2D;

        private readonly Dictionary<int, Dictionary<int, HexCell>> _grid = new();
        
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

                    AddCell(
                        cellWorldPosition2,
                        new Vector2Int(rowIndex, columnIndex),
                        overlapPoint == null);

                    columnIndex++;
                }

                rowIndex++;
            }
        }

        private void AddCell(Vector3 worldPosition, Vector2Int gridPosition, bool traversable)
        {
            if (!_grid.ContainsKey(gridPosition.x))
            {
                _grid[gridPosition.x] = new Dictionary<int, HexCell>();
            }

            var row = _grid[gridPosition.x];
            
            row[gridPosition.y] = new HexCell(gridPosition, worldPosition, traversable);
        }
        
        public Queue<HexCell> GetPath(Vector3 worldPosition)
        {
            return new Queue<HexCell>();
        }

        private void OnDrawGizmos()
        {
            _grid.Values.SelectMany(d => d.Values).ToList().ForEach(cell =>
            {
                Gizmos.color = cell.Traversable ? Color.green : Color.red;
                Gizmos.DrawWireSphere(cell.WorldPosition, 0.1f);
            });
        }
    }
}
