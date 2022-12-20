using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grids
{
    public class HexGrid
    {
        // public List<List<HexCell>> grid = new List<List<HexCell>>();
        private readonly Dictionary<int, Dictionary<int, HexCell>> _grid = new();

        public void AddCell(Vector3 worldPosition, Vector2Int gridPosition, bool traversable)
        {
            // Debug.Log($"Adding cell at <{gridPosition.x}, {gridPosition.y}>");

            if (!_grid.ContainsKey(gridPosition.x))
            {
                _grid[gridPosition.x] = new Dictionary<int, HexCell>();
            }

            var row = _grid[gridPosition.x];
            
            row[gridPosition.y] = new HexCell(gridPosition, worldPosition, traversable);
        }

        public List<HexCell> AllCells => _grid.Values.SelectMany(d => d.Values).ToList();

        public Queue<HexCell> GetPath(Vector2 worldPosition)
        {
            return new Queue<HexCell>();
        }
    }
}
