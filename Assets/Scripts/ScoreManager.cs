using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
<<<<<<< HEAD
    public static ScoreManager Instance;

    public int strokes = 0;
    public int overallScore = 0;

    public TextMeshProUGUI scoreText; 

    private void Start()
    {
        UpdateScoreText();
=======
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI holeInfoText;

    private int strokes = 0;
    private int currentHole = 1;
    private int par = 3;

    void Start()
    {
        
        UpdateScoreText();
       
>>>>>>> origin/SCRUM-49-implement-a-user-interface-to-d
    }

    public void AddStroke()
    {
        strokes++;
        UpdateScoreText();
    }

<<<<<<< HEAD
    public void AddToOverallScore(int score)
    {
        overallScore += score;
        UpdateScoreText();
    }

    public void ResetStrokes()
    {
        strokes = 0;
        UpdateScoreText();
=======
    public void SetHole(int holeNumber, int holePar)
    {
        currentHole = holeNumber;
        par = holePar;
        strokes = 0; // Reset strokes for a new hole
        UpdateScoreText();
       
>>>>>>> origin/SCRUM-49-implement-a-user-interface-to-d
    }

    private void UpdateScoreText()
    {
<<<<<<< HEAD
        if (scoreText != null)
        {
            scoreText.text = $"strokes: {strokes}\noverall score: {overallScore}";
        }
    }
=======
        scoreText.text = $"Score: {strokes}";
    }

   
>>>>>>> origin/SCRUM-49-implement-a-user-interface-to-d
}