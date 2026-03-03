using UnityEngine;
using UnityEngine.InputSystem;

public class UniversalCameraController : MonoBehaviour
{
    [Header("Настройки движения")]
    public float dragSpeed = 0.5f;
    public float smoothSpeed = 10f;

    [Header("Настройки зума")]
    public bool enableZoom = true;
    public float zoomSpeed = 0.05f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    [Header("Границы карты")]
    public bool useBounds = true;
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -10f;
    public float maxY = 10f;

    private Camera cam;
    private Vector3 targetPosition;
    private Vector2 lastTouchPosition;
    private bool isDragging = false;

    // Для мыши
    private Mouse mouse;
    // Для касаний
    private Touchscreen touch;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
            cam = Camera.main;

        targetPosition = transform.position;

        // Получаем устройства ввода
        mouse = Mouse.current;
        touch = Touchscreen.current;
    }

    void Update()
    {
        // Проверяем, есть ли касания (телефон)
        if (touch != null && touch.touches.Count > 0)
        {
            HandleTouchInput();
        }
        // Если нет касаний, проверяем мышь (ПК)
        else if (mouse != null)
        {
            HandleMouseInput();
        }

        // Плавно двигаем камеру
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Ограничиваем границы
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
        // Нажали левую кнопку мыши
        if (mouse.leftButton.wasPressedThisFrame)
        {
            lastTouchPosition = mouse.position.ReadValue();
            isDragging = true;
        }

        // Держим левую кнопку
        if (mouse.leftButton.isPressed && isDragging)
        {
            Vector2 currentMousePos = mouse.position.ReadValue();
            DragCamera(currentMousePos);
        }

        // Отпустили кнопку
        if (mouse.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;
        }

        // Зум колесиком
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

        // Одно касание - движение
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
        // Два касания - зум
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
        // Конвертируем экранные координаты в мировые
        Vector3 worldCurrent = cam.ScreenToWorldPoint(currentPosition);
        Vector3 worldLast = cam.ScreenToWorldPoint(lastTouchPosition);

        // Двигаем камеру в противоположную сторону
        Vector3 delta = worldLast - worldCurrent;
        targetPosition += delta * dragSpeed;

        lastTouchPosition = currentPosition;
    }

    // Визуализация границ в редакторе
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