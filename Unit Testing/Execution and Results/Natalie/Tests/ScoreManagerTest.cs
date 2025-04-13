using UnityEngine;

public class ScoreManagerTest
{
    // Natalie - ScoreManagerTest.cs
using NUnit.Framework;
using UnityEngine;

public class ScoreManagerTest
{
    [SetUp]
    public void SetUp()
    {
        Object.Instantiate(Resources.Load("ScoreManager"));
        ScoreManager.Instance.strokes = 0;
        ScoreManager.Instance.overallScore = 0;
    }

    [Test]
    public void AddStroke_IncrementsStrokes()
    {
        ScoreManager.Instance.AddStroke();
        Assert.AreEqual(1, ScoreManager.Instance.strokes);
    }

    [Test]
    public void AddToOverallScore_AddsCorrectly()
    {
        ScoreManager.Instance.strokes = 3;
        ScoreManager.Instance.AddToOverallScore(0);
        Assert.AreEqual(3, ScoreManager.Instance.overallScore);
    }

    [Test]
    public void UpdateScoreText_UpdatesUI()
    {
        ScoreManager.Instance.strokes = 5;
        ScoreManager.Instance.ResetStrokes();
        Assert.AreEqual(0, ScoreManager.Instance.strokes);
    }
}

}
