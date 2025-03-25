using UnityEngine;
using UnityEngine.InputSystem;

public class LaunchWithDrag : MonoBehaviour
{
    public Rigidbody2D rb;
    public ClickAndDrag clickAndDrag;
    [SerializeField]
    private float forceAmount;

    [SerializeField] private float mass;
    [SerializeField] private float linearDamping;
    [SerializeField] private float angularDamping;

    private Vector3 lastPosition;
    private bool hasCountedStroke = false;
    private bool hasPlayedSound = false;

    private AudioSource audioSource;
    [SerializeField] private AudioClip golfHit;

    private PowerMeterUI powerMeter;

    void Start()
{
    rb = gameObject.GetComponent<Rigidbody2D>();
    clickAndDrag = gameObject.GetComponent<ClickAndDrag>();
    mass = rb.mass;
    linearDamping = rb.linearDamping;
    angularDamping = rb.angularDamping;
    lastPosition = transform.position;

    powerMeter = FindFirstObjectByType<PowerMeterUI>();

    if (powerMeter == null)
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        clickAndDrag = gameObject.GetComponent<ClickAndDrag>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found! Add one to the object.");
        }

        mass = rb.mass;
        linearDamping = rb.linearDamping;
        angularDamping = rb.angularDamping;
        lastPosition = transform.position;

        powerMeter = FindFirstObjectByType<PowerMeterUI>();
        if (powerMeter == null)
        {
            Debug.LogError("PowerMeterUI not found in the scene!");
        }
    }
}

void Update()
{
    if (rb != null && clickAndDrag != null)
    {
        if (!isMoving())
        {
            if (!isMoving())
            {
                hasCountedStroke = false;
                hasPlayedSound = false;

                if (clickAndDrag.isDragging)
                {
                    if (powerMeter != null)
                    {
                        powerMeter.ShowPowerMeter();

                        float dragDistance = Vector3.Distance(clickAndDrag.startPos, clickAndDrag.endPos);
                        powerMeter.UpdatePowerMeter(dragDistance);
                    }
                }
                else
                {
                    if (powerMeter != null)
                    {
                        powerMeter.HidePowerMeter();
                    }

                    rb.linearVelocity = new Vector2(
                        (clickAndDrag.startPos.x - clickAndDrag.endPos.x) * forceAmount,
                        (clickAndDrag.startPos.y - clickAndDrag.endPos.y) * forceAmount
                    );
                }
            }
            else
            {
                clickAndDrag.endPos = clickAndDrag.startPos;
                clickAndDrag.isDragging = false;

                PlayGolfBallSound();
                CheckForMovement();
            }

            if (Mouse.current.leftButton.IsPressed() && !clickAndDrag.isHovering() && clickAndDrag.isDragging)
            {
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(inBoundsVector());
                clickAndDrag.endPos = worldPosition;
            }
        }
        else
        {
            clickAndDrag.endPos = clickAndDrag.startPos;
            clickAndDrag.isDragging = false;
            CheckForMovement();
        }

        if (Mouse.current.leftButton.IsPressed() && !clickAndDrag.isHovering() && clickAndDrag.isDragging)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(inBoundsVector());
            clickAndDrag.endPos = worldPosition;
        }
    }
}

    public Vector3 inBoundsVector()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float x = Mathf.Clamp(mousePosition.x, 0, Screen.width);
        float y = Mathf.Clamp(mousePosition.y, 0, Screen.height);

        return new Vector3(x, y, 0f);
    }

    public bool isMoving()
    {
        return rb.linearVelocity.magnitude > 0.01f;
    }

    public void setForce(float newForceAmount) => forceAmount = newForceAmount;
    public float getForce() => forceAmount;

    public float Mass
    {
        get { return rb.mass; }
        set { rb.mass = value; }
    }

    public float LinearDrag
    {
        get { return rb.linearDamping; }
        set { rb.linearDamping = value; }
    }

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

    private void PlayGolfBallSound()
    {
        if (golfHit == null)
        {
            Debug.LogError("ERROR: golfHit AudioClip is NULL! Assign it in the Inspector.");
            return;
        }
        if (audioSource == null)
        {
            Debug.LogError("ERROR: AudioSource is NULL! Make sure an AudioSource is attached.");
            return;
        }

        if (!hasPlayedSound)
        {
            SoundFXManager.instance.PlaySoundEffect(golfHit, transform, 1f);
            hasPlayedSound = true;
        }
    }
}
