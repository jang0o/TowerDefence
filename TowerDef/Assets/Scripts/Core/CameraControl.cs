using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    [Header("��������� ��������")]
    public float moveSpeed = 20f;
    public float zoomSpeed = 10f;

    private Vector2 lastMousePos;
    private bool isDragging = false;

    void Update()
    {
        HandleMouseMovement();
        HandleMouseZoom();
    }

    void HandleMouseMovement()
    {
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

            if (delta.sqrMagnitude > 0.01f)
            {
                Vector3 move = new Vector3(-delta.x, 0, -delta.y) * moveSpeed * 0.01f;

                transform.position += move;

                lastMousePos = currentMousePos;
            }
        }
    }

    void HandleMouseZoom()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;

        if (Mathf.Abs(scroll) > 0.1f)
        {
            float scrollAmount = scroll / 120f;
            float newY = transform.position.y - (scrollAmount * zoomSpeed);

            newY = Mathf.Clamp(newY, 5f, 50f);

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}
