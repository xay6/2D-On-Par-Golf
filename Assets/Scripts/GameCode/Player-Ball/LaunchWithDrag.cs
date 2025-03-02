using UnityEngine;
using UnityEngine.InputSystem;

public class LaunchWithDrag : MonoBehaviour
{
    public Rigidbody2D rb;
    public ClickAndDrag clickAndDrag;
    [SerializeField]
    private float forceAmount;
    // Added for testing purposes. Makes the variables show up in component view.
    [SerializeField]
    private float mass;
    [SerializeField]
    private float linearDamping;
    [SerializeField]
    private float angularDamping;
    private Vector3 lastPosition;
    private bool hasCountedStroke = false;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        clickAndDrag = gameObject.GetComponent<ClickAndDrag>();
        mass = rb.mass;
        linearDamping = rb.linearDamping;
        angularDamping = rb.angularDamping;
        lastPosition = transform.position;
    }

    void Update()
    {
        if (rb != null)
        {
            
            if (!isMoving())
            {
                hasCountedStroke = false;
                if (!clickAndDrag.isDragging)
                {
                    // Calculate the difference between where the ball starts and ends and uses it to create a vector.
                    rb.linearVelocity = new Vector2((clickAndDrag.startPos.x - clickAndDrag.endPos.x) * forceAmount, (clickAndDrag.startPos.y - clickAndDrag.endPos.y) * forceAmount);
                }
            }
            else
            {
                clickAndDrag.endPos = clickAndDrag.startPos;
                CheckForMovement();
            }

            if (Mouse.current.leftButton.IsPressed() && !clickAndDrag.isHovering() && clickAndDrag.isDragging)
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(inBoundsVector());

                clickAndDrag.endPos = worldPosition;
            }
        }
    }

    // Returns a Vector3.
    // Limits mouse input from extending past the game window.
    public Vector3 inBoundsVector()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float x = mousePosition.x;
        float y = mousePosition.y;

        if (mousePosition.x < 0)
        {
            x = Mathf.Max(0, mousePosition.x);
        }
        else if (mousePosition.x > Screen.width)
        {
            x = Mathf.Min(Screen.width, mousePosition.x);
        }

        if (mousePosition.y < 0)
        {
            y = Mathf.Max(0, mousePosition.y);
        }
        else if (mousePosition.y > Screen.height)
        {
            y = Mathf.Min(Screen.height, mousePosition.y);
        }

        return new Vector3(x, y, 0f);
    }

    // Checks the ball velocity.
    public bool isMoving()
    {
        return rb.linearVelocity.magnitude != 0;
    }

    public void setForce(float newForceAmount)
    {
        forceAmount = newForceAmount;
    }

    public float getForce()
    {
        return forceAmount;
    }

    /*
    Usage for the following:
        get: float var = launchWithDrag.Mass;
        set: launchWithDrag.Mass = *some float val*;
    */
    public float Mass
    {
        get { return rb.mass; }
        set { rb.mass = value; }
    }
    // For simulating objects moving through resistive mediums, like water or air.
    // Stay within 0.1-5
    public float LinearDrag
    {
        get { return rb.linearDamping; }
        set { rb.linearDamping = value; }
    }
    // Controls the resistance to rotational motion. Higher values slow down the rotation faster.
    // Stay within 0.05-1
    public float AngularDrag
    {
        get { return rb.angularDamping; }
        set { rb.angularDamping = value; }
    }

    private void CheckForMovement()
    {
        if (!hasCountedStroke)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddStroke();
                
                
            }
            else
            {
                Debug.LogError("ScoreManager.Instance is NULL!");
            }

            hasCountedStroke = true;
        }

        lastPosition = transform.position;
    }
}
