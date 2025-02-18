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
        lineRenderer.SetPosition(0, gameObject.transform.position);
        lineRenderer.SetPosition(1, clickAndDrag.endPos);
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
