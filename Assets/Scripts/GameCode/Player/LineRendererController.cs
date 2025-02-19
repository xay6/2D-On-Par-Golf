using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private ClickAndDrag clickAndDrag;

    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        clickAndDrag = gameObject.GetComponent<ClickAndDrag>();

        lineRenderer.enabled = false;

        // ✅ Ensure the LineRenderer has at least 2 positions
        lineRenderer.positionCount = 2; 

        lineRenderer.SetPosition(0, gameObject.transform.position);
        lineRenderer.SetPosition(1, gameObject.transform.position); // Set both points to the same position initially
    }

    void Update()
    {
        if (lineRenderer != null)
        {
            if (clickAndDrag.startPos != clickAndDrag.endPos)
            {
                if (clickAndDrag.isDragging)
                {
                    lineRenderer.enabled = true;
                }
                else
                {
                    lineRenderer.enabled = false;
                }

                // ✅ Ensure the LineRenderer has at least 2 positions before setting them
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, gameObject.transform.position);
                lineRenderer.SetPosition(1, clickAndDrag.endPos);
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }
    }
}
