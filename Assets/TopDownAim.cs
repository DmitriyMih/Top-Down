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
        [SerializeField] private LineRenderer lineRenderer;

        [SerializeField] private Transform player;
        [SerializeField] private Transform target;

        private void Update()
        {
            if (target != null && camera != null)
            {
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                mousePosition = new Vector2(Mathf.Clamp(mousePosition.x, 0 + deadZoneWidth.x, Screen.width - deadZoneWidth.y), Mathf.Clamp(mousePosition.y, 0 + deadZoneHeight.x, Screen.height - deadZoneHeight.y));

                Debug.Log("Mouse Position - " + mousePosition);
                Vector3 worldPosition = camera.ScreenToWorldPoint(mousePosition);

                target.position = worldPosition;
            }

            if (lineRenderer != null)
            //if (Input.GetMouseButton(1))
            {
                lineRenderer.positionCount = 2;

                if (player != null)
                    lineRenderer.SetPosition(0, player.position);

                if (target != null)
                    lineRenderer.SetPosition(1, target.position);
            }
        }
    }
}