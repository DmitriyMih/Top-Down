using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    public class TopDownAim : MonoBehaviour
    {
        [Header("Input Settings")]

        [SerializeField] private PlayerInput playerInput;
        public bool isGamepad;

        [Header("Screen Settings")]
        [SerializeField] private Camera camera;
        [SerializeField, Min(0)] private Vector2Int deadZoneWidth;
        [SerializeField, Min(0)] private Vector2Int deadZoneHeight;
        
        [Header("Aim Settings"), Space]
        [SerializeField] private bool isAiming;
        [SerializeField] public bool aimCircleClamping;

        [SerializeField] public float aimClampRadius;

        [Header("Aim Elements Settings")]
        [SerializeField] private Transform player;
        [SerializeField] private Transform aimTarget;
        [SerializeField] private Transform aimEndPoint;

        [Header("Aim Line Settings")]
        [SerializeField] private LineRenderer aimLineRender;
        [SerializeField] private LineRenderer redAimLineRender;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }

        public void OnDeviceChange(PlayerInput playerInput)
        {
            isGamepad = playerInput.currentControlScheme.Equals("Gamepad") ? true : false;
        }

        private void Update()
        {
            if (playerInput != null)
                OnDeviceChange(playerInput);

            if (isAiming)
                if (aimTarget != null && camera != null && aimEndPoint != null)
                {
                    Vector2 mousePosition = Mouse.current.position.ReadValue();
                    mousePosition = new Vector2(Mathf.Clamp(mousePosition.x, 0 + deadZoneWidth.x, Screen.width - deadZoneWidth.y), Mathf.Clamp(mousePosition.y, 0 + deadZoneHeight.x, Screen.height - deadZoneHeight.y));

                    Ray ray = camera.ScreenPointToRay(mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider)
                    {
                        Vector3 newWorldPosition = hit.point;
                        aimEndPoint.position = newWorldPosition;

                        if (aimCircleClamping)
                        {
                            Vector3 offset = newWorldPosition - player.position;
                            newWorldPosition = player.position + Vector3.ClampMagnitude(offset, aimClampRadius);
                        }

                        aimTarget.position = newWorldPosition;
                    }
                }

            if (aimLineRender != null)
            {
                aimLineRender.positionCount = 2;

                if (player != null)
                    aimLineRender.SetPosition(0, player.position);

                if (aimTarget != null)
                    aimLineRender.SetPosition(1, aimTarget.position);
            }

            if(redAimLineRender != null)
            {
                redAimLineRender.positionCount = 2;

                if (aimTarget != null)
                    redAimLineRender.SetPosition(0, aimTarget.position);

                if (aimEndPoint != null)
                    redAimLineRender.SetPosition(1, aimEndPoint.position);
            }
        }
    }
}