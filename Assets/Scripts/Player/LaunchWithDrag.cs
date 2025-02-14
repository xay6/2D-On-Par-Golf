using UnityEngine;
using UnityEngine.InputSystem;

public class LaunchWithDrag : MonoBehaviour
{
    ClickAndDrag clickAndDrag;
    private Vector3 endPos;
    private Vector3 startPos;

    public float forceAmount;
    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        clickAndDrag = gameObject.GetComponent<ClickAndDrag>();
        startPos = clickAndDrag.getStartPos();
        endPos = clickAndDrag.getEndPos();
    }

    void Update()
    {
        if(rb.linearVelocityX == 0 && rb.linearVelocityY == 0) {
            // Same kind of logic for inputs in LineRenderer.
            if(Mouse.current.leftButton.IsPressed()) {
                endPos = clickAndDrag.getEndPos();
            }

            if(Mouse.current.leftButton.wasPressedThisFrame) {
                startPos = clickAndDrag.getStartPos();
            } else if(Mouse.current.leftButton.wasReleasedThisFrame) {
                startPos = clickAndDrag.getStartPos();
                endPos = clickAndDrag.getEndPos();

                // Calculate the difference between where the ball starts and ends and uses it to create a vector.
                rb.linearVelocity = new Vector2((startPos.x - endPos.x) * forceAmount, (startPos.y - endPos.y) * forceAmount);
            }
        }
    }
}
