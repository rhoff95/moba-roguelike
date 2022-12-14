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
        [SerializeField] private TilemapCollider2D tilemapCollider2D;
        [SerializeField] private CompositeCollider2D compositeCollider2D;

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
           
            
            
         

        }
    }
}
