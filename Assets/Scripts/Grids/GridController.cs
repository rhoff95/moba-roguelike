using System;
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
            var yMax = yBoundsMax - (yBoundsMax % (cellSize.y * (3 / 4f)));

            var rowIndex = 0;
            for (var y = yMax; y > yBoundsMin; y -= cellSize.y * (3 / 4f))
            {
                // if (Mathf.Abs(y) >= 0.5f)
                // {
                    // continue;
                // }
                
                var even = y % (cellSize.y * (3 / 2f)) == 0f;

                var offset = even ? 0f : cellSize.x / 2f;

                var columnIndex = 0;
                for (var x = xMax - offset; x > xBoundsMin; x -= cellSize.x)
                {
                    // if (Mathf.Abs(x) >= 0.5f)
                    // {
                    //     continue;
                    // }
                    
                    var cellWorldPosition2 = new Vector2(x, y);

                    // TODO Move this to the grid, ask the grid
                    var overlapPoint = Physics2D.OverlapPoint(cellWorldPosition2);

                    var q = (Mathf.Sqrt(3) / 3 * x - 1f / 3f * y) / (cellSize.y / 2f);
                    var r = (3f / 3f * y) / (cellSize.y / 2f);

                    var gridPosition = AxialRound(q, r);
                    // var gridPosition = new Vector2Int(rowIndex, columnIndex);

                    // if (gridPosition.y == 0)
                    // {
                       
                    // }

                    // try
                    // {
                    Debug.Log($"v=<{x:F3}, {y:F3}> <{q:F3}, {r:F3}> => => {gridPosition}");
                    
                        AddCell(
                            cellWorldPosition2,
                            gridPosition,
                            overlapPoint == null);
                        
                      
                    // }
                    // catch (Exception e)
                    // {
                        // Debug.LogError(e);
                        // throw;
                    // }
                    
                   

                    columnIndex++;
                }

                rowIndex++;
            }
        }

        private Vector2Int AxialRound(float x, float y)
        {
            var xGrid = (int) Mathf.Round(x);
            var yGrid = (int) Mathf.Round(y);

            var xRemainder = x - xGrid;
            var yRemainder = y - yGrid;

            return Mathf.Abs(x) > Mathf.Abs(y)
                ? new Vector2Int(xGrid + (int) Mathf.Round(xRemainder + 0.5f * yRemainder), yGrid)
                : new Vector2Int(xGrid, yGrid + (int) Mathf.Round(yRemainder + 0.5f * xRemainder));
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

        public Queue<HexCell> GetPath(Vector3 worldPosition)
        {
            var q = new Queue<HexCell>();

            q.Enqueue(_grid[0][0]);

            return q;
        }

        private void OnDrawGizmos()
        {
            var size = 0.075f;
            var inc = 0.00f;
            
            _grid.Values.SelectMany(d => d.Values).ToList().ForEach(cell =>
            {
                if (cell.GridPosition.x >= 0 && cell.GridPosition.y >= 0)
                {
                    Gizmos.color = Color.blue;
                }
                else if (cell.GridPosition.x <= 0 && cell.GridPosition.y <= 0)
                {
                    Gizmos.color = Color.magenta;
                }
                else
                {
                    Gizmos.color = Color.red;
                }

                Gizmos.DrawWireSphere(cell.WorldPosition, size);
                size += inc;
            });
        }
    }
}
