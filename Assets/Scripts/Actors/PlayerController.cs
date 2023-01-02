using UnityEngine;

namespace Actors
{
    public class PlayerController : MonoBehaviour
    {
        private Controls _controls;
        private Camera _mainCamera;

        private ActorController _actor;

        private void Awake()
        {
            _mainCamera = Camera.main;

            SetupControls();
        }

        private void Start()
        {
            _actor = FindObjectOfType<ActorController>();
        }

        private void SetTarget()
        {
            var mousePosition = Input.mousePosition;
            var worldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);
            var worldPositionAdjusted = new Vector3(worldPosition.x, worldPosition.y, 0f);
            _actor.SetTargetLocation(worldPositionAdjusted);
        }

        private void SetupControls()
        {
            _controls = new Controls();
            var gameActions = _controls.GameActions;
            gameActions.RightClick.started += _ =>  SetTarget();
        }

        private void OnEnable()
        {
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls.Disable();
        }
    }
}
