using System;
using System.Collections.Generic;
using Grids;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
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

        private GridController _grid;
        
        // [SerializeField] public Vector3? _targetPosition;

        [CanBeNull] private HexCell _currentTarget = null;
        private Queue<HexCell> _moveToQueue = new();

        private void Awake()
        {
            _grid = FindObjectOfType<GridController>();
        }

        private void Update()
        {
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            if (_currentTarget == null)
            {
                return;
            }

            var targetPosition = _currentTarget.WorldPosition;
            
            var toTarget = targetPosition - transform.position;
            var distanceToTarget = toTarget.magnitude;
            var maxDistanceTravelled = Time.deltaTime * speed;

            if (maxDistanceTravelled >= distanceToTarget)
            {
                transform.position = targetPosition;
                var hasNext = _moveToQueue.TryDequeue(out var result);
                _currentTarget = hasNext ? result : null;
            }
            else
            {
                transform.position += toTarget.normalized * maxDistanceTravelled;
            }
        }
        
        public void SetTargetLocation(Vector3 position)
        {
            Debug.Log($"Actor ({name}) going to ({position})");
            
            _moveToQueue = _grid.GetPath(position);
        }

        private void OnDrawGizmos()
        {
           
            
            
         

        }
    }
}
