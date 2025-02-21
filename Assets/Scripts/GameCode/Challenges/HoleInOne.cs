using UnityEngine;
using UnityEngine.Rendering;

public class HoleInOne : MonoBehaviour
{
    public string objectName;
    private GameObject ball;
    public int strokes = 0;
    public void ballStrokes(){
        ball = GameObject.Find(objectName);
        Debug.Log("Object Found: " + ball);
        
    }

}
