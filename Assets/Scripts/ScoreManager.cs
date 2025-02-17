using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI holeInfoText;

    private int strokes = 0;
    private int currentHole = 1;
    private int par = 3;

    void Start()
    {
        
        UpdateScoreText();
       
    }

    public void AddStroke()
    {
        strokes++;
        UpdateScoreText();
    }

    public void SetHole(int holeNumber, int holePar)
    {
        currentHole = holeNumber;
        par = holePar;
        strokes = 0; // Reset strokes for a new hole
        UpdateScoreText();
       
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score: {strokes}";
    }

   
}