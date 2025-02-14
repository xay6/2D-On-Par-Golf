using UnityEngine;
using UnityEngine.InputSystem;

public class ClickAndDrag : MonoBehaviour {
    public Mouse mouse;
    private GameObject playerBall;
    private Vector3 startPos;
    private Vector3 endPos;

    private void Start() {
        mouse = Mouse.current;

        if(this.gameObject != null)
        {
            playerBall = this.gameObject;
            startPos = playerBall.transform.position;
            endPos = playerBall.transform.position;
        }
    }

    private void Update()
    {
        mouse = Mouse.current;

        if(mouse.leftButton.IsPressed() && playerBall != null) {
            Vector2 mousePosition = mouse.position.ReadValue();
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0f));

            endPos = worldPosition;
        }

        if(mouse.leftButton.wasPressedThisFrame) {
            if(playerBall != null)
            {
                startPos = playerBall.transform.position;
                endPos = playerBall.transform.position;
            }
        }
    }

    public Vector3 getStartPos() {
        return startPos;
    }

    public Vector3 getEndPos() {
        return endPos;
    }
}