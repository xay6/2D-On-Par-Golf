using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 startDragPosition;
    private Vector2 releasePosition;
    private bool isDragging = false;
    private bool isMoving = false;

    public float maxForce = 10f; 
    public LineRenderer lineRenderer; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer.enabled = false; 
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !isMoving) 
        {
            startDragPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            isDragging = true;
            lineRenderer.enabled = true;
        }

        if (isDragging && Mouse.current.leftButton.isPressed) 
        {
            releasePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            DrawAimingLine(startDragPosition, releasePosition);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && isDragging) 
        {
            isDragging = false;
            lineRenderer.enabled = false;
            ShootBall(startDragPosition, releasePosition);
        }
    }

    void ShootBall(Vector2 start, Vector2 end)
    {
        Vector2 forceDirection = (start - end).normalized; 
        float forceMagnitude = Mathf.Clamp(Vector2.Distance(start, end), 0, maxForce);
        rb.AddForce(forceDirection * forceMagnitude, ForceMode2D.Impulse);
        isMoving = true;
    }

    void DrawAimingLine(Vector2 start, Vector2 end)
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    void FixedUpdate()
{
    if (isMoving && rb.linearVelocity.magnitude > 0.1f) // Apply wind only when ball is moving
    {
        Wind wind = FindFirstObjectByType<Wind>();
        if (wind != null)
        {
            Vector2 windForce = wind.windDirection * wind.windStrength;
            rb.AddForce(windForce);
        }
    }

    if (rb.linearVelocity.magnitude < 0.1f) // Stop ball completely when it's almost still
    {
        rb.linearVelocity = Vector2.zero;
        isMoving = false;
    }
}
}


