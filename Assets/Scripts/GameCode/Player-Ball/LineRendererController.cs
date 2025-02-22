using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private ClickAndDrag clickAndDrag;
    private Rigidbody2D rb;

    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        clickAndDrag = gameObject.GetComponent<ClickAndDrag>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        lineRenderer.enabled = false;

        // Ensure the LineRenderer has at least 2 positions before setting them
        if (lineRenderer.positionCount < 2)
        {
            lineRenderer.positionCount = 2;
        }

        lineRenderer.SetPosition(0, gameObject.transform.position);
        lineRenderer.SetPosition(1, gameObject.transform.position);
    }

    void Update()
    {
        if (lineRenderer != null && clickAndDrag != null && rb != null)
        {
            if (rb.linearVelocity.magnitude == 0 && !clickAndDrag.isDragging)
            {
                if (clickAndDrag.isHovering())
                {
                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, clickAndDrag.startPos);
                    lineRenderer.SetPosition(1, clickAndDrag.endPos);
                }
                else
                {
                    lineRenderer.enabled = false;
                }
            }
            else
            {
                lineRenderer.SetPosition(1, clickAndDrag.endPos);
            }
        }
    }
}
