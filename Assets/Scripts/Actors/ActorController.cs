using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Actors
{
    public class ActorController : MonoBehaviour
    { 
        [SerializeField] [Range(0f, 5f)] private float speed;
        [SerializeField] private Grid grid;
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Collider2D gridCollider;

        [SerializeField] public Vector3? _targetPosition;

        private void Update()
        {
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            if (!_targetPosition.HasValue)
            {
                return;
            }

            var targetPosition = _targetPosition.Value;
            
            var toTarget = targetPosition - transform.position;
            var distanceToTarget = toTarget.magnitude;
            var maxDistanceTravelled = Time.deltaTime * speed;

            if (maxDistanceTravelled >= distanceToTarget)
            {
                transform.position = targetPosition;
                _targetPosition = null;
            }
            else
            {
                transform.position += toTarget.normalized * maxDistanceTravelled;
            }
        }
        
        public void SetTargetLocation(Vector3 position)
        {
            _targetPosition = position;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            
            var colliderBounds = gridCollider.bounds;

            var v = colliderBounds.max.x;
            var xBoundsMax = colliderBounds.max.x;
            var xBoundsMin = colliderBounds.min.x;
            
            var yBoundsMax = colliderBounds.max.y;
            var yBoundsMin = colliderBounds.min.y;

            var xMax = xBoundsMax - (yBoundsMax % (grid.cellSize.x / 2f));
            var yMax = yBoundsMax - (yBoundsMax % (grid.cellSize.y *(3/4f)));


            var yValues = new List<float>();
            
            for (var y = yMax; y > yBoundsMin; y -= grid.cellSize.y * (3/4f))
            {
                yValues.Add(y);

                var even = y % (grid.cellSize.y * (3 / 2f)) == 0f;

                var offset = even ? 0f : 0f;// grid.cellSize.x;// / 2f;
                Gizmos.color = even ? Color.magenta : Color.green;
                
                for (var x = xMax + offset; x > xBoundsMin; x -= grid.cellSize.x)
                {
                    Gizmos.DrawWireSphere(new Vector3(x, y, 0f), 0.1f);     
                }
                
                // Gizmos.DrawLine(
                //     new Vector3(colliderBounds.min.x, y, 0f),
                //     new Vector3(colliderBounds.max.x, y, 0f)
                // );
            }
            
            
         

        }
    }
}
