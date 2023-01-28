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
        public Vector2 look;

        [Header("Screen Settings"), Space]
        [SerializeField] private Camera mainCamera;
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

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnLook(InputValue value)
        {
                LookInput(value.Get<Vector2>());
            
        }
        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }
#endif

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

            SetCursorState(isGamepad);

            if (isGamepad)
            {
                Vector2 aimDirection = CustomFunctions.DegreeToVector2(mainCamera.transform.eulerAngles.y);
                Vector3 worldDirection = new Vector3((look.x * aimDirection.x), 0, (look.y * aimDirection.y));
               
                Vector3 worldPosition = player.position + worldDirection;
                CustomFunctions.ClampInCircle(ref worldPosition, player.position, aimClampRadius);
        
                if (aimTarget != null)
                    aimTarget.position = worldPosition;
            }
            else
            {
                if (mainCamera == null)
                    return;

               // if() input right mouse button

                Vector2 mousePosition = Mouse.current.position.ReadValue();
                mousePosition = new Vector2(Mathf.Clamp(mousePosition.x, 0 + deadZoneWidth.x, Screen.width - deadZoneWidth.y), Mathf.Clamp(mousePosition.y, 0 + deadZoneHeight.x, Screen.height - deadZoneHeight.y));

                Ray ray = mainCamera.ScreenPointToRay(mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider)
                {
                    Vector3 newWorldPosition = hit.point;
                    //aimEndPoint.position = newWorldPosition;

                    if (aimCircleClamping)
                        CustomFunctions.ClampInCircle(ref newWorldPosition, player.position, aimClampRadius);
                        //ClampInCircle(ref newWorldPosition, player.position, aimClampRadius);
                    
                    if (aimTarget != null)
                        aimTarget.position = newWorldPosition;
                }
            }

            //if (isAiming)
            //    if (aimTarget != null && camera != null && aimEndPoint != null)
            //    {
            //        Vector2 mousePosition = Mouse.current.position.ReadValue();
            //        mousePosition = new Vector2(Mathf.Clamp(mousePosition.x, 0 + deadZoneWidth.x, Screen.width - deadZoneWidth.y), Mathf.Clamp(mousePosition.y, 0 + deadZoneHeight.x, Screen.height - deadZoneHeight.y));

            //        Ray ray = camera.ScreenPointToRay(mousePosition);

            //        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider)
            //        {
            //            Vector3 newWorldPosition = hit.point;
            //            aimEndPoint.position = newWorldPosition;

            //            if (aimCircleClamping)
            //            {
            //                Vector3 offset = newWorldPosition - player.position;
            //                newWorldPosition = player.position + Vector3.ClampMagnitude(offset, aimClampRadius);
            //            }

            //            aimTarget.position = newWorldPosition;
            //        }
            //    }

            if (aimLineRender != null)
            {
                aimLineRender.positionCount = 2;

                if (player != null)
                    aimLineRender.SetPosition(0, player.position);

                if (aimTarget != null)
                    aimLineRender.SetPosition(1, aimTarget.position); 
            }
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