using UnityEngine;
using TMPro; 
public class HoleIncrementer : MonoBehaviour
{
    public TextMeshProUGUI holeText; 
    public string prefix = "Hole ";

    void Start()
    {
        int holeNumber = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        if (holeText == null)
            holeText = GetComponent<TextMeshProUGUI>();

        holeText.text = prefix + holeNumber;
    }
}

