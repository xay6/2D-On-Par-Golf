using UnityEngine;
using UnityEngine.InputSystem;

public class LineRendererController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private Vector3 startPos;
    private Vector3 endPos;
    private ClickAndDrag clickAndDrag;

    void Start()
    {
        clickAndDrag = gameObject.GetComponent<ClickAndDrag>();
        startPos = clickAndDrag.getStartPos();
        endPos = clickAndDrag.getEndPos();
    }

    private void Update()
    {
        if (lineRenderer != null)
        {
            // Changes the state of the line in real time
            if(Mouse.current.leftButton.IsPressed()) {
                endPos = clickAndDrag.getEndPos();
            }

            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }
    }
}
