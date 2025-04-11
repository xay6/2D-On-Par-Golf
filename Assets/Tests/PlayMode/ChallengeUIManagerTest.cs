using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using TMPro;

public class ChallengeUIManagerTest
{
    private GameObject uiManagerObject;
    private ChallengeUIManger uiManager;

    private CanvasGroup successPanel;
    private CanvasGroup failedPanel;
    private CanvasGroup challengePanel;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create the GameObject and attach the ChallengeUIManger component
        uiManagerObject = new GameObject("ChallengeUIManager");
        uiManager = uiManagerObject.AddComponent<ChallengeUIManger>();

        // Create panels as CanvasGroups
        var successPanelObj = new GameObject("SuccessPanel");
        successPanel = successPanelObj.AddComponent<CanvasGroup>();
        successPanel.alpha = 0;
        successPanel.gameObject.SetActive(false);

        var failedPanelObj = new GameObject("FailedPanel");
        failedPanel = failedPanelObj.AddComponent<CanvasGroup>();
        failedPanel.alpha = 0;
        failedPanel.gameObject.SetActive(false);

        var challengePanelObj = new GameObject("ChallengePanel");
        challengePanel = challengePanelObj.AddComponent<CanvasGroup>();
        challengePanel.alpha = 0;
        challengePanel.gameObject.SetActive(false);

        // Assign panels to UIManager
        uiManager.successPanel = successPanel;
        uiManager.failedPanel = failedPanel;
        uiManager.challengePanel = challengePanel;

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShowSuccessPanel_ShouldDisplaySuccessPanel()
    {
        // Act
        uiManager.ShowSuccessPanel();

        // Wait for panel to activate and become visible
        yield return new WaitForSeconds(uiManager.displayTime / 2f);

        // Assert
        Assert.IsTrue(successPanel.gameObject.activeSelf, "Success panel should be active after calling ShowSuccessPanel.");
        Assert.AreEqual(1f, successPanel.alpha, "Success panel alpha should be 1 when displayed.");
    }

    [UnityTest]
    public IEnumerator ShowFailedPanel_ShouldDisplayFailedPanel()
    {
        // Act
        uiManager.ShowFailedPanel();

        // Wait for panel to activate and become visible
        yield return new WaitForSeconds(uiManager.displayTime / 2f);

        // Assert
        Assert.IsTrue(failedPanel.gameObject.activeSelf, "Failed panel should be active after calling ShowFailedPanel.");
        Assert.AreEqual(1f, failedPanel.alpha, "Failed panel alpha should be 1 when displayed.");
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if (uiManagerObject != null) Object.Destroy(uiManagerObject);
        if (successPanel != null) Object.Destroy(successPanel.gameObject);
        if (failedPanel != null) Object.Destroy(failedPanel.gameObject);
        if (challengePanel != null) Object.Destroy(challengePanel.gameObject);

        yield return null;
    }
}
