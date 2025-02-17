using UnityEngine;
using UnityEngine.InputSystem;

public class ClickAndDrag : MonoBehaviour
{
    public Vector3 startPos { get; set; }
    public Vector3 endPos { get; set; }
    public bool isDragging { get; set; }

    void Start()
    {
        startPos = gameObject.transform.position;
        endPos = gameObject.transform.position;
        isDragging = false;
    }

    void Update()
    {
        if (isHovering())
        {
            startPos = gameObject.transform.position;
            endPos = gameObject.transform.position;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && isHovering())
        {
            isDragging = true;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;
        }

        if (Mouse.current.leftButton.IsPressed() && !isHovering() && isDragging)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0f));

            endPos = worldPosition;
        }
    }

    public bool isHovering()
    {
        Vector2 point = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        return gameObject.GetComponent<CircleCollider2D>().OverlapPoint(point);
    }
}