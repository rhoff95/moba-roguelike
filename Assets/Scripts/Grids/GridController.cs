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
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;

            // compositeCollider2D.geometryType = CompositeCollider2D.GeometryType.Polygons;
        
            var colliderBounds = compositeCollider2D.bounds;

            var v = colliderBounds.max.x;
            var xBoundsMax = colliderBounds.max.x;
            var xBoundsMin = colliderBounds.min.x;
            
            var yBoundsMax = colliderBounds.max.y;
            var yBoundsMin = colliderBounds.min.y;

            var cellSize = grid.cellSize;
            var xMax = xBoundsMax - (xBoundsMax % (cellSize.x / 2f));
            var yMax = yBoundsMax - (yBoundsMax % (cellSize.y *(3/4f)));


            var yValues = new List<float>();
            
            for (var y = yMax; y > yBoundsMin; y -= cellSize.y * (3/4f))
            {
                yValues.Add(y);

                var even = y % (cellSize.y * (3 / 2f)) == 0f;

                var offset = even ? 0f : cellSize.x / 2f;
               
                
                for (var x = xMax - offset; x > xBoundsMin; x -= cellSize.x)
                {
                    var cellWorldPosition3 = new Vector3(x, y, 0f);
                    var cellWorldPosition2 = new Vector3(x, y);

                    // var overlapPoint = compositeCollider2D.OverlapPoint(cellWorldPosition2);
                    var overlapPoint = Physics2D.OverlapPoint(cellWorldPosition2);
                    Gizmos.color = !overlapPoint ? Color.green : Color.red;
                    
                    Gizmos.DrawWireSphere(cellWorldPosition2, 0.1f);
                    
                    _hexGrid.AddCell(cellWorldPosition2);
                    
                }
                
                // Gizmos.DrawLine(
                //     new Vector3(colliderBounds.min.x, y, 0f),
                //     new Vector3(colliderBounds.max.x, y, 0f)
                // );
            
                // compositeCollider2D.geometryType = CompositeCollider2D.GeometryType.Outlines;
            }
        }
    }
}
