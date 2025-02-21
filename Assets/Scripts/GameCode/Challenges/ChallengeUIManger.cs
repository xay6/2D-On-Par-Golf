using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengeUIManager : MonoBehaviour
{
    public CanvasGroup challengePanel; // Assign in Inspector
    public CanvasGroup successPanel;   // Assign in Inspector
    public CanvasGroup failedPanel;    // Assign in Inspector
    public float fadeDuration = 1.5f;  // Duration of fade effect
    public float displayTime = 3f;     // How long the challenge panel stays

    void Start()
    {
        // Hide success and fail panels at the start
        successPanel.alpha = 0;
        successPanel.gameObject.SetActive(false);

        failedPanel.alpha = 0;
        failedPanel.gameObject.SetActive(false);

        // Show challenge panel and fade it out
        StartCoroutine(ShowChallengePanel());
        
    }

    IEnumerator ShowChallengePanel()
    {
        // Make sure the challenge panel is visible
        challengePanel.alpha = 1;
        challengePanel.gameObject.SetActive(true);

        // Wait for a few seconds
        yield return new WaitForSeconds(displayTime);

        // Fade out challenge panel
        yield return StartCoroutine(FadeOut(challengePanel));
    }

    public void ShowSuccessPanel()
    {
        StartCoroutine(ShowPanel(successPanel));
    }

    public void ShowFailedPanel()
    {
        StartCoroutine(ShowPanel(failedPanel));
    }

    IEnumerator ShowPanel(CanvasGroup panel)
    {
        panel.alpha = 1;
        panel.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayTime);

        // Fade out after display time
        yield return StartCoroutine(FadeOut(panel));
    }

    IEnumerator FadeOut(CanvasGroup panel)
    {
        float startAlpha = panel.alpha;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            panel.alpha = Mathf.Lerp(startAlpha, 0, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.alpha = 0;
        panel.gameObject.SetActive(false);
    }
}
