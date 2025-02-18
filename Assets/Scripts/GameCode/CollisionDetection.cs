using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public string objectName; // Must be set in Unity Inspector.
    private GameObject ball;
    private CircleCollider2D circleCollider2D;
    public bool onCollisionDetected;
    

    void Start() {
        ball = GameObject.Find(objectName);
        circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
        onCollisionDetected = false;
    }

    void Update() {
        if(circleCollider2D != null && ball != null) {
            if(circleCollider2D.OverlapPoint(ball.transform.position)) {
                onCollisionDetected = true;
                return;
            }
        }
    }

    public bool OnCollision
    {
        get { return onCollisionDetected; }
        set { onCollisionDetected = value; }
    }
}