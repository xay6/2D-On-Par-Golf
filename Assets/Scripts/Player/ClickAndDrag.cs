using UnityEngine;
using UnityEngine.InputSystem;

public class ClickAndDrag : MonoBehaviour {
    private Mouse mouse;
    private GameObject playerBall;
    private Vector2 startPos;
    private Vector2 currentPos;
    private Vector2 offset;

    private void Update()
    {
        mouse = Mouse.current;

        if(mouse.leftButton.IsPressed() && playerBall != null) {
            currentPos = mouse.position.ReadValue();
            offset = currentPos - startPos;
        }

        if(mouse.leftButton.wasPressedThisFrame) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mouse.position.ReadValue()), Vector2.zero);

            if(hit.collider != null)
            {
                SelectBall(hit.transform.gameObject);
            }
        }
    }

    private void SelectBall(GameObject ball)
    {
        playerBall = ball;
        startPos = mouse.position.ReadValue();
    }

    public Vector2 getOffset()
    {
        return offset;
    }
    
}