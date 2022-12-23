using System;
using System.Collections.Generic;
using System.Linq;
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

        private Vector3? _currentTarget = null;
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
            if (!_currentTarget.HasValue)
            {
                return;
            }

            var targetPosition = _currentTarget.Value;
            
            var toTarget = targetPosition - transform.position;
            var distanceToTarget = toTarget.magnitude;
            var maxDistanceTravelled = Time.deltaTime * speed;

            if (maxDistanceTravelled >= distanceToTarget)
            {
                transform.position = targetPosition;
                var hasNext = _moveToQueue.TryDequeue(out var result);
                _currentTarget = hasNext ? result.WorldPosition : null;
            }
            else
            {
                transform.position += toTarget.normalized * maxDistanceTravelled;
            }
        }
        
        public void SetTargetLocation(Vector3 position)
        {
            _moveToQueue = _grid.GetPath(transform.position, position);
            
            Debug.Log($"Actor ({name}) going to ({position}), p1 is ({_moveToQueue.Peek()})");
        }

        private void OnDrawGizmos()
        {
            var moves = _moveToQueue.ToList();

            for (var i = 0; i < moves.Count; i++)
            {
                var cell = moves[i];

                var count = (float) Mathf.Max(1, moves.Count - 1);
                
                Gizmos.color = Color.Lerp(Color.green, Color.red, i / count);
                Gizmos.DrawWireSphere(cell.WorldPosition, 0.2f);
            }
            
            _moveToQueue.ToList().ForEach(cell =>
            {
                
            });

            if (_currentTarget.HasValue)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(_currentTarget.Value, 0.1f);
            }
        }
    }
}
