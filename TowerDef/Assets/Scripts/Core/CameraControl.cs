using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    [Header("Настройки движения")]
    public float moveSpeed = 20f;     // Увеличил скорость по умолчанию
    public float zoomSpeed = 10f;     // Вернул старую скорость зума

    private Vector2 lastMousePos;
    private bool isDragging = false;

    void Update()
    {
        HandleMouseMovement();
        HandleMouseZoom();
    }

    void HandleMouseMovement()
    {
        // Проверяем ПРАВУЮ кнопку мыши
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            isDragging = true;
            lastMousePos = Mouse.current.position.ReadValue();
        }

        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 currentMousePos = Mouse.current.position.ReadValue();
            Vector2 delta = currentMousePos - lastMousePos;

            // Если мышь сдвинулась
            if (delta.sqrMagnitude > 0.01f)
            {
                // Рассчитываем движение. Умножаем на 0.01, чтобы числа не были гигантскими
                Vector3 move = new Vector3(-delta.x, 0, -delta.y) * moveSpeed * 0.01f;

                // Двигаем камеру относительно мира
                transform.position += move;

                // Обновляем позицию для следующего кадра
                lastMousePos = currentMousePos;
            }
        }
    }

    void HandleMouseZoom()
    {
        // Возвращаем старую логику зума через изменение Y
        float scroll = Mouse.current.scroll.ReadValue().y;

        if (Mathf.Abs(scroll) > 0.1f)
        {
            // scroll в новой системе обычно равен 120 или -120, поэтому делим на 120
            float scrollAmount = scroll / 120f;
            float newY = transform.position.y - (scrollAmount * zoomSpeed);

            // Ограничение высоты, чтобы не провалиться под землю
            newY = Mathf.Clamp(newY, 5f, 50f);

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}