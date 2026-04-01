using UnityEngine;
using UnityEngine.InputSystem;

public class UniversalCameraController : MonoBehaviour
{
    [Header("��������� ��������")]
    public float dragSpeed = 0.5f;
    public float smoothSpeed = 10f;

    [Header("��������� ����")]
    public bool enableZoom = true;
    public float zoomSpeed = 0.05f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    [Header("������� �����")]
    public bool useBounds = true;
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -10f;
    public float maxY = 10f;

    private Camera cam;
    private Vector3 targetPosition;
    private Vector2 lastTouchPosition;
    private bool isDragging = false;

    // ��� ����
    private Mouse mouse;
    // ��� �������
    private Touchscreen touch;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
            cam = Camera.main;

        targetPosition = transform.position;

        // �������� ���������� �����
        mouse = Mouse.current;
        touch = Touchscreen.current;
    }

    void Update()
    {
        // ���������, ���� �� ������� (�������)
        if (touch != null && touch.touches.Count > 0)
        {
            HandleTouchInput();
        }
        // ���� ��� �������, ��������� ���� (��)
        else if (mouse != null)
        {
            HandleMouseInput();
        }

        // ������ ������� ������
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // ������������ �������
        if (useBounds)
        {
            float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
            float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
            targetPosition = transform.position;
        }
    }

    void HandleMouseInput()
    {
        // ������ ����� ������ ����
        if (mouse.leftButton.wasPressedThisFrame)
        {
            lastTouchPosition = mouse.position.ReadValue();
            isDragging = true;
        }

        // ������ ����� ������
        if (mouse.leftButton.isPressed && isDragging)
        {
            Vector2 currentMousePos = mouse.position.ReadValue();
            DragCamera(currentMousePos);
        }

        // ��������� ������
        if (mouse.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;
        }

        // ��� ���������
        if (enableZoom)
        {
            float scrollDelta = mouse.scroll.ReadValue().y;
            if (scrollDelta != 0)
            {
                cam.orthographicSize -= scrollDelta * zoomSpeed * 10;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
            }
        }
    }

    void HandleTouchInput()
    {
        if (touch == null) return;

        // ���� ������� - ��������
        if (touch.touches.Count == 1)
        {
            UnityEngine.InputSystem.Controls.TouchControl touchControl = touch.touches[0];

            if (touchControl.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                lastTouchPosition = touchControl.position.ReadValue();
                isDragging = true;
            }
            else if (touchControl.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved && isDragging)
            {
                Vector2 currentTouchPos = touchControl.position.ReadValue();
                DragCamera(currentTouchPos);
            }
            else if (touchControl.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended ||
                     touchControl.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
        // ��� ������� - ���
        else if (touch.touches.Count == 2 && enableZoom)
        {
            var touch1 = touch.touches[0];
            var touch2 = touch.touches[1];

            Vector2 prevPos1 = touch1.position.ReadValue() - touch1.delta.ReadValue();
            Vector2 prevPos2 = touch2.position.ReadValue() - touch2.delta.ReadValue();

            float prevDistance = Vector2.Distance(prevPos1, prevPos2);
            float currentDistance = Vector2.Distance(touch1.position.ReadValue(), touch2.position.ReadValue());

            float delta = currentDistance - prevDistance;

            cam.orthographicSize -= delta * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }

    void DragCamera(Vector2 currentPosition)
    {
        // ������������ �������� ���������� � �������
        Vector3 worldCurrent = cam.ScreenToWorldPoint(currentPosition);
        Vector3 worldLast = cam.ScreenToWorldPoint(lastTouchPosition);

        // ������� ������ � ��������������� �������
        Vector3 delta = worldLast - worldCurrent;
        targetPosition += delta * dragSpeed;

        lastTouchPosition = currentPosition;
    }

    // ������������ ������ � ���������
    void OnDrawGizmosSelected()
    {
        if (useBounds)
        {
            Gizmos.color = Color.yellow;
            Vector3 center = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, 0);
            Vector3 size = new Vector3(maxX - minX, maxY - minY, 0);
            Gizmos.DrawWireCube(center, size);
        }
    }
}