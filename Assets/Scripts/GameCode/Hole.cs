using UnityEngine;

public class Hole : MonoBehaviour
{
    CollisionDetection collisionDetection;
    private GameObject ball;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collisionDetection = gameObject.GetComponent<CollisionDetection>();
        ball = GameObject.Find(collisionDetection.objectName);
    }

    // Update is called once per frame
    void Update()
    {
        if(collisionDetection.onSuperimposed) {
            ball.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
