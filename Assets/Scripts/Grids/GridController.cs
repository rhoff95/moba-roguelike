using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grids
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private Grid grid;
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
            var yMax = yBoundsMax - (yBoundsMax % (cellSize.y * (3 / 4f)));

            for (var y = yMax; y > yBoundsMin; y -= cellSize.y * (3 / 4f))
            {
                var even = y % (cellSize.y * (3 / 2f)) == 0f;

                var offset = even ? 0f : cellSize.x / 2f;

                for (var x = xMax - offset; x > xBoundsMin; x -= cellSize.x)
                {
                    var cellWorldPosition2 = new Vector2(x, y);

                    // TODO Move this to the grid, ask the grid
                    var overlapPoint = Physics2D.OverlapPoint(cellWorldPosition2);

                    var q = (Mathf.Sqrt(3) / 3 * x - 1f / 3f * y) / (cellSize.y / 2f);
                    var r = (3f / 3f * y) / (cellSize.y / 2f);

                    var gridPosition = AxialRound(q, r);

                    Debug.Log($"Adding hex at {gridPosition}");

                    AddCell(
                        cellWorldPosition2,
                        gridPosition,
                        overlapPoint == null);
                }
            }
        }

        private void AddCell(Vector3 worldPosition, Vector2Int gridPosition, bool traversable)
        {
            if (!_grid.ContainsKey(gridPosition.x))
            {
                _grid[gridPosition.x] = new Dictionary<int, HexCell>();
            }

            var row = _grid[gridPosition.x];

            if (row.ContainsKey(gridPosition.y))
            {
                throw new Exception($"Grid already has value for {gridPosition}");
            }

            row[gridPosition.y] = new HexCell(gridPosition, worldPosition, traversable);
        }

        public Queue<HexCell> GetPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
        {
            var q = new Queue<HexCell>();

            var startGrid = AxialRound(startWorldPosition);
            var endGrid= AxialRound(endWorldPosition);

            if (startGrid == endGrid)
            {
                return q;
            }

            var endHexRow = _grid[endGrid.x];
            var endHexExists = endHexRow.TryGetValue(endGrid.y, out var endHex);

            if (!endHexExists)
            {
                Debug.LogWarning(
                    $"Column {endGrid.y} does not exist. Valid column values are {string.Join(", ", endHexRow.Keys)}"
                    );

                return q;
            }
            
            var startHexRow = _grid[startGrid.x];
            var startHexExists = startHexRow.TryGetValue(startGrid.y, out var startHex);
            
            if (!startHexExists)
            {
                Debug.LogWarning(
                    $"Column {endGrid.y} does not exist. Valid column values are {string.Join(", ", endHexRow.Keys)}"
                );

                return q;
            }
            
            q.Enqueue(startHex);
            q.Enqueue(endHex);

            return q;
        }

        private static Vector2Int AxialRound(Vector3 position)
        {
            return AxialRound(position.x, position.y);
        }

        private static Vector2Int AxialRound(float x, float y)
        {
            var xGrid = (int)Mathf.Round(x);
            var yGrid = (int)Mathf.Round(y);

            var xRemainder = x - xGrid;
            var yRemainder = y - yGrid;

            return Mathf.Abs(x) > Mathf.Abs(y)
                ? new Vector2Int(xGrid + (int)Mathf.Round(xRemainder + 0.5f * yRemainder), yGrid)
                : new Vector2Int(xGrid, yGrid + (int)Mathf.Round(yRemainder + 0.5f * xRemainder));
        }

        private void OnDrawGizmos()
        {
            const float size = 0.1f;
            const float opacity = 0.5f;
            
            _grid.Values.SelectMany(d => d.Values).ToList().ForEach(cell =>
            {
                if (cell.GridPosition.x >= 0 && cell.GridPosition.y >= 0)
                {
                    Gizmos.color = Color.Lerp(Color.red, Color.clear, opacity);
                }
                else if (cell.GridPosition.x <= 0 && cell.GridPosition.y <= 0)
                {
                    Gizmos.color = Color.Lerp(Color.magenta, Color.clear, opacity);
                }
                else
                {
                    Gizmos.color = Color.Lerp(Color.blue, Color.clear, opacity);
                }

                Gizmos.DrawWireSphere(cell.WorldPosition, size);
            });
        }
    }
}
