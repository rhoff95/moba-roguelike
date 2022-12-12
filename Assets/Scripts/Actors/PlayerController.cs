using System;
using UnityEngine;

namespace Actors
{
    public class PlayerController : MonoBehaviour
    {
        private Controls _controls;
        private Camera _mainCamera;

        private ActorController _actor;

        private bool _recordRightClick = false;

        private void Awake()
        {
            _mainCamera = Camera.main;

            SetupControls();
        }

        private void Start()
        {
            _actor = FindObjectOfType<ActorController>();
        }

        private void Update()
        {
            if (_recordRightClick)
            {
                var mousePosition = Input.mousePosition;
                var worldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);
                var worldPositionAdjusted = new Vector3(worldPosition.x, worldPosition.y, 0f);
                _actor.SetTargetLocation(worldPositionAdjusted);
            }
        }

        private void SetupControls()
        {
            _controls = new Controls();
            var gameActions = _controls.GameActions;
            gameActions.RightClick.performed += _ => _recordRightClick = true;
            gameActions.RightClick.canceled += _ => _recordRightClick = false;
            
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
