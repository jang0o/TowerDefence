using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class UniversalCameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float dragSpeed = 1.0f;
    public float smoothSpeed = 10f;

    [Header("Zoom Settings")]
    public bool enableZoom = true;
    public float zoomSpeed = 0.01f;
    public float minZoom = 3f;
    public float maxZoom = 15f;

    [Header("Map Bounds (XZ)")]
    public bool useBounds = true;
    public float minX = -12f;
    public float maxX = 12f;
    public float minZ = -15f;
    public float maxZ = 25f;

    private Camera cam;
    private Vector3 targetPosition;
    private bool isDragging = false;
    private Vector2 lastInputPosition;

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null) cam = Camera.main;
        targetPosition = transform.position;
    }

    void Update()
    {
        HandleInput();

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        if (useBounds)
        {
            float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
            float clampedZ = Mathf.Clamp(transform.position.z, minZ, maxZ);
            transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
            
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.z = Mathf.Clamp(targetPosition.z, minZ, maxZ);
        }
    }

    void HandleInput()
    {
        if (ETouch.Touch.activeTouches.Count > 0)
        {
            if (ETouch.Touch.activeTouches.Count == 1)
            {
                var touch = ETouch.Touch.activeTouches[0];
                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                {
                    lastInputPosition = touch.screenPosition;
                    isDragging = true;
                }
                else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved && isDragging)
                {
                    DragCamera(touch.screenPosition);
                }
                else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended || touch.phase == UnityEngine.InputSystem.TouchPhase.Canceled)
                {
                    isDragging = false;
                }
            }
            else if (ETouch.Touch.activeTouches.Count == 2 && enableZoom)
            {
                isDragging = false;
                var t1 = ETouch.Touch.activeTouches[0];
                var t2 = ETouch.Touch.activeTouches[1];

                float prevDist = Vector2.Distance(t1.screenPosition - t1.delta, t2.screenPosition - t2.delta);
                float currDist = Vector2.Distance(t1.screenPosition, t2.screenPosition);
                float delta = currDist - prevDist;

                ZoomCamera(delta);
            }
        }
        else if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                lastInputPosition = Mouse.current.position.ReadValue();
                isDragging = true;
            }
            else if (Mouse.current.leftButton.isPressed && isDragging)
            {
                DragCamera(Mouse.current.position.ReadValue());
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                isDragging = false;
            }

            if (enableZoom)
            {
                float scroll = Mouse.current.scroll.ReadValue().y;
                if (scroll != 0) ZoomCamera(scroll * 10f);
            }
        }
    }

    void DragCamera(Vector2 currentScreenPos)
    {
        if (cam == null) return;
        
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        Ray currentRay = cam.ScreenPointToRay(currentScreenPos);
        Ray lastRay = cam.ScreenPointToRay(lastInputPosition);

        if (groundPlane.Raycast(currentRay, out float dist1) && groundPlane.Raycast(lastRay, out float dist2))
        {
            Vector3 worldCurrent = currentRay.GetPoint(dist1);
            Vector3 worldLast = lastRay.GetPoint(dist2);
            Vector3 delta = worldLast - worldCurrent;
            delta.y = 0;
            targetPosition += delta;
        }
        
        lastInputPosition = currentScreenPos;
    }

    void ZoomCamera(float delta)
    {
        if (cam.orthographic)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - delta * zoomSpeed, minZoom, maxZoom);
        }
        else
        {
            targetPosition.y = Mathf.Clamp(targetPosition.y - delta * zoomSpeed * 10f, minZoom, maxZoom);
        }
    }
}