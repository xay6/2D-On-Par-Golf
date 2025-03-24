using UnityEngine;

/*
    This class watches for collisions between the object this script is attatched to, and 
    all objects that share the name specified in objectName(Value set within unity inspector).
    
    * onSuperimposed detects when the objects overlap.
    * onCollision detects when the objects come into contact an any way.
*/
public class CollisionDetection : MonoBehaviour
{
    public string objectName; // Must be set in Unity Inspector.
    private GameObject ball;
    private CircleCollider2D circleCollider2D;
    public bool onSuperimposed;
    public bool onCollision;
    

    void Start() {
        ball = GameObject.Find(objectName);
        circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
        onSuperimposed = false;
        onCollision = false;
    }

    void Update() {
        if(circleCollider2D != null && ball != null) {
            if(circleCollider2D.OverlapPoint(ball.transform.position)) {
                onSuperimposed = true;
                
                //CoinManager.Instance.AddCoins(10);
                HoleInOne checkHole = FindAnyObjectByType<HoleInOne>();
                checkHole.CheckHoleInOne();
                CoinManager.Instance.AddCoins(10);

                return;
            }
            if(circleCollider2D.IsTouching(ball.GetComponent<Collider2D>())) {
                onCollision = true;
            }
        }
    }

    public bool OnSuperimposed
    {
        get { return onSuperimposed; }
        set { onSuperimposed = value; }
    }

    public bool OnCollision
    {
        get { return onCollision; }
        set { onCollision = value; }
    }
}