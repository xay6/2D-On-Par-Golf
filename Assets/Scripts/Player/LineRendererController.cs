using UnityEngine;
using UnityEngine.InputSystem;

public class LineRendererController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private ClickAndDrag clickAndDrag;
    private LaunchWithDrag launchWithDrag;
    private Vector3 endPos;

    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        clickAndDrag = gameObject.GetComponent<ClickAndDrag>();
        launchWithDrag = gameObject.GetComponent<LaunchWithDrag>();
        lineRenderer.enabled = false;
        endPos = clickAndDrag.getEndPos();
        lineRenderer.SetPosition(0, gameObject.transform.position);
        lineRenderer.SetPosition(1, endPos);
    }

    void Update()
    {
        if (lineRenderer != null)
        {
            if(!launchWithDrag.isMoving()) {
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
