using UnityEngine;
using UnityEngine.InputSystem;

public class ClickAndDrag : MonoBehaviour {
    private GameObject playerBall;
    private LaunchWithDrag launchWithDrag;
    private Vector3 startPos;
    private Vector3 endPos;
    [SerializeField]
    private bool hovered;

    void Start() {
        playerBall = gameObject;
        launchWithDrag = gameObject.GetComponent<LaunchWithDrag>();
        startPos = playerBall.transform.position;
        endPos = playerBall.transform.position;
    }

    void Update()
    {
        if(playerBall != null & launchWithDrag != null) {
            if(!launchWithDrag.isMoving()) {
                if(Mouse.current.leftButton.wasPressedThisFrame) 
                {
                    startPos = playerBall.transform.position;
                    endPos = playerBall.transform.position;
                }

                if(Mouse.current.leftButton.IsPressed()) {
                    Vector2 mousePosition = Mouse.current.position.ReadValue();
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0f));

                    endPos = worldPosition;
                }
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