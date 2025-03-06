using UnityEngine;

public class LaunchWithDrag : MonoBehaviour
{
    private Rigidbody2D rb;
    private ClickAndDrag clickAndDrag;
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
    [SerializeField] private AudioClip golfHit; // Assign this in the Inspector

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
                };
                // Play the hit sound **only when applying force**
                if (SoundFXManager.instance != null && golfHit != null)
                {
                    SoundFXManager.instance.PlaySoundEffect(golfHit, transform, 1f);
                }
            }
            else
            {
                clickAndDrag.endPos = clickAndDrag.startPos;
                CheckForMovement();
            
            }
            
        }
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
