using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    public class TopDownAim : MonoBehaviour
    {
        [Header("Screen Settings")]
        [SerializeField] private Camera camera;
        [SerializeField, Min(0)] private Vector2Int deadZoneWidth;
        [SerializeField, Min(0)] private Vector2Int deadZoneHeight;

        [Header("Aim Settings")]
        [SerializeField] private bool isAiming = false;

        [SerializeField] public float aimClampRadius;

        [Header("Aim Elements Settings")]
        [SerializeField] private LineRenderer lineRenderer;

        [SerializeField] private Transform player;
        [SerializeField] private Transform aimTarget;


        private void Update()
        {
            if (isAiming)
                if (aimTarget != null && camera != null)
                {
                    Vector2 mousePosition = Mouse.current.position.ReadValue();
                    mousePosition = new Vector2(Mathf.Clamp(mousePosition.x, 0 + deadZoneWidth.x, Screen.width - deadZoneWidth.y), Mathf.Clamp(mousePosition.y, 0 + deadZoneHeight.x, Screen.height - deadZoneHeight.y));

                    Ray ray = camera.ScreenPointToRay(mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider)
                    {
                        aimTarget.position = hit.point;
                    }
                }

            if (lineRenderer != null)
            {
                lineRenderer.positionCount = 2;

                if (player != null)
                    lineRenderer.SetPosition(0, player.position);

                if (aimTarget != null)
                    lineRenderer.SetPosition(1, aimTarget.position);
            }
        }
    }
}