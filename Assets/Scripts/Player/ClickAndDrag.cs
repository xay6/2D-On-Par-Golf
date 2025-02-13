using UnityEngine;
using UnityEngine.InputSystem;

public class ClickAndDrag : MonoBehaviour {
    Mouse mouse;
    private GameObject playerBall;
    private Vector2 startPos;
    private Vector2 currentPos;
    public Vector2 offset;
    public bool leftReleased;

    void Update()
    {
        mouse = Mouse.current;

        if(mouse.leftButton.wasPressedThisFrame) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mouse.position.ReadValue()), Vector2.zero);

            if(hit.collider != null)
            {
                SelectBall(hit.transform.gameObject);
            }
            startPos = mouse.position.ReadValue();
        }

        if(mouse.leftButton.wasReleasedThisFrame) {
            DeselectBall(playerBall);
            leftReleased = true;
            offset = currentPos - startPos;
        }

        if(mouse.IsPressed()) {
            currentPos = mouse.position.ReadValue();
        }
    }

    void SelectBall(GameObject ball)
    {
        playerBall = ball;

        // TESTING: Change the ball's color
        SpriteRenderer renderer = playerBall.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = Color.green; // Highlight color
        }
    }

    void DeselectBall(GameObject ball)
    {
        // TESTING: Reset the ball's color
        SpriteRenderer renderer = ball.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = Color.white;
        }
    }
    
}