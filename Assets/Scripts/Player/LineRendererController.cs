using UnityEngine;
using UnityEngine.InputSystem;

public class LineRendererController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private Vector3 endPos;
    private ClickAndDrag clickAndDrag;
    private LaunchWithDrag launchWithDrag;

    void Start()
    {
        clickAndDrag = gameObject.GetComponent<ClickAndDrag>();
        launchWithDrag = gameObject.GetComponent<LaunchWithDrag>();
        lineRenderer.enabled = false;
        endPos = clickAndDrag.getEndPos();
        lineRenderer.SetPosition(0, gameObject.transform.position);
        lineRenderer.SetPosition(1, endPos);
    }

    private void Update()
    {
        if (lineRenderer != null)
        {
            if(launchWithDrag.rb.linearVelocityX == 0 && launchWithDrag.rb.linearVelocityY == 0) {
                // Changes the state of the line in real time
                if(Mouse.current.leftButton.IsPressed()) {
                    endPos = clickAndDrag.getEndPos();
                }

                // Set the start and end points and visibility of the Line Renderer
                if(Mouse.current.leftButton.wasPressedThisFrame) {
                    lineRenderer.enabled = true;

                    // startPos = clickAndDrag.getStartPos();
                } else if(Mouse.current.leftButton.wasReleasedThisFrame) {
                    lineRenderer.enabled = false;

                    // startPos = clickAndDrag.getStartPos();
                    endPos = clickAndDrag.getEndPos();
                }
                lineRenderer.SetPosition(0, gameObject.transform.position);
                lineRenderer.SetPosition(1, endPos);
            } else {
                lineRenderer.enabled = false;
            }
        }
    }
}
