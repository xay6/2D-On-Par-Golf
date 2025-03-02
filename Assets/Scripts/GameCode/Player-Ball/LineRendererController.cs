using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public LaunchWithDrag launchWithDrag;

    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        launchWithDrag = gameObject.GetComponent<LaunchWithDrag>();
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
        if (lineRenderer != null && launchWithDrag != null && launchWithDrag.clickAndDrag != null)
        {
            if (launchWithDrag.clickAndDrag.isHovering() && launchWithDrag.clickAndDrag.isDragging)
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, launchWithDrag.clickAndDrag.startPos);

            }
            
            if (!launchWithDrag.isMoving())
            {
                if (launchWithDrag.clickAndDrag.isDragging)
                {
                    lineRenderer.SetPosition(1, launchWithDrag.clickAndDrag.endPos);
                }
            }
            else
            {
                lineRenderer.enabled = false;
                lineRenderer.SetPosition(0, launchWithDrag.clickAndDrag.startPos);
                lineRenderer.SetPosition(1, launchWithDrag.clickAndDrag.endPos);
            }
        }
    }
}
