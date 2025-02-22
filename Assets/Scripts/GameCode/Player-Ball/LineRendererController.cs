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
                     if (clickAndDrag.startPos != clickAndDrag.endPos) // Ensure a valid shot
                {
                    HoleInOne holeInOneScript = FindAnyObjectByType<HoleInOne>();
                    if (holeInOneScript != null)
                    {
                        holeInOneScript.IncreaseStrokeCount();
                        Debug.Log("Shot taken! Stroke count increased.");
                    }
                    else
                    {
                        Debug.LogError("No HoleInOne script found in the scene!");
                    }
                }
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
