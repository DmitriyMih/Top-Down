using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    public class TopDownAim : MonoBehaviour
    {
        [Header("Input Settings")]

        [SerializeField] private StarterAssetsInputs assetsInputs;
        [SerializeField] private PlayerInput playerInput;

        public bool isGamepad;
        [SerializeField] private float playerOffcetY = 1f;
        public float degree = 0f;

        [Header("Screen Settings"), Space]
        [SerializeField] private Camera mainCamera;
        [SerializeField, Min(0)] private Vector2Int deadZoneWidth;
        [SerializeField, Min(0)] private Vector2Int deadZoneHeight;

        [Header("Aim Settings"), Space]
        [SerializeField] public bool aimCircleClamping;

        [SerializeField] public float aimClampRadius;

        [Header("Aim Elements Settings")]
        [SerializeField] private Transform player;
        [SerializeField] private Transform aimTarget;
        [SerializeField] private Transform aimRayTarget;

        [Header("Aim Line Settings")]
        [SerializeField] private LineRenderer aimLineRender;
        [SerializeField] private LayerMask layerMask;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            assetsInputs = GetComponent<StarterAssetsInputs>();
        }

        public void OnDeviceChange(PlayerInput playerInput)
        {
            isGamepad = playerInput.currentControlScheme.Equals("Gamepad") ? true : false;
            SetCursorState(isGamepad);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

        private void Update()
        {
            if (player == null || mainCamera == null)
                return;

            if (playerInput != null)
                OnDeviceChange(playerInput);

            if (assetsInputs.look == Vector2.zero)
                return;

            Vector3 aimOffcetDirection = mainCamera.transform.forward;
            Vector3 aimDirection = new Vector3(assetsInputs.look.x * aimOffcetDirection.x, 0, assetsInputs.look.y * aimOffcetDirection.z);
            aimDirection *= aimClampRadius;

            Vector3 worldPosition = new Vector3();
            Vector3 aimTargetPosition = new Vector3();

            if (isGamepad)
            {
                worldPosition = player.position + aimDirection;
            }
            else
            {
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                mousePosition = new Vector2(Mathf.Clamp(mousePosition.x, 0 + deadZoneWidth.x, Screen.width - deadZoneWidth.y), Mathf.Clamp(mousePosition.y, 0 + deadZoneHeight.x, Screen.height - deadZoneHeight.y));

                Ray ray = mainCamera.ScreenPointToRay(mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, int.MaxValue, layerMask) && hit.collider)
                    worldPosition = CustomFunctions.ClampInCircle(hit.point, player.position, aimClampRadius);
            }

            CheckRayToWorld(ref worldPosition);
            if (aimRayTarget != null)
                aimRayTarget.position = worldPosition;

            aimTargetPosition = new Vector3(worldPosition.x, player.position.y, worldPosition.z);
            if (aimTarget != null)
                aimTarget.position = aimTargetPosition;

            if (aimRayTarget != null)
                aimRayTarget.position = worldPosition + Vector3.up * playerOffcetY;

            if (aimLineRender != null)
            {
                aimLineRender.positionCount = 2;
                if (player != null) aimLineRender.SetPosition(0, player.position);
                if (aimTarget != null) aimLineRender.SetPosition(1, aimRayTarget.position);
            }
        }

        public void CheckRayToWorld(ref Vector3 worldPosition)
        {
            Debug.Log($"Enter Value {worldPosition} | Systen Gamepad {isGamepad}");
            Ray ray = new Ray(mainCamera.transform.position, worldPosition - mainCamera.transform.position);
            Debug.DrawRay(mainCamera.transform.position, worldPosition - mainCamera.transform.position, Color.red);

            if (Physics.Raycast(ray, out RaycastHit hit, int.MaxValue, layerMask) && hit.collider)
            {
                worldPosition = hit.point;
                Debug.DrawLine(mainCamera.transform.position, hit.point, Color.red);
            }

            CustomFunctions.ClampInCircle(ref worldPosition, player.position, aimClampRadius);
            Debug.Log($"Exit Value {worldPosition} | Systen Gamepad {isGamepad}");
        }
    }
}

public static class CustomFunctions
{
    public static void ClampInCircle(ref Vector3 targetPoint, Vector3 center, float clampRadius)
    {
        Vector3 offset = targetPoint - center;
        targetPoint = center + Vector3.ClampMagnitude(offset, clampRadius);
    }

    public static Vector3 ClampInCircle(Vector3 targetPoint, Vector3 center, float clampRadius)
    {
        Vector3 offset = targetPoint - center;
        targetPoint = center + Vector3.ClampMagnitude(offset, clampRadius);
        return targetPoint;
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static Vector2 RadianToVector2(float radian, float length)
    {
        return RadianToVector2(radian) * length;
    }

    public static Vector2 DegreeToVector2(float degree, float length)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad) * length;
    }
}