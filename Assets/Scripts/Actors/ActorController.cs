using UnityEngine;

namespace Actors
{
    public class ActorController : MonoBehaviour
    { 
        [SerializeField] [Range(0f, 5f)] private float speed;

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
    }
}
