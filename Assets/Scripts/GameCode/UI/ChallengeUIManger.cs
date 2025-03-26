using UnityEngine;
using System.Collections;

public class ChallengeUIManger : MonoBehaviour
{
    public CanvasGroup challengePanel;
    public CanvasGroup successPanel;
    public CanvasGroup failedPanel;
    public float fadeDuration = 1.5f;
    public float displayTime = 3f;
    void Start()
    {

        successPanel.alpha = 0;
        successPanel.gameObject.SetActive(false);

        failedPanel.alpha = 0;
        failedPanel.gameObject.SetActive(false);

        StartCoroutine(ShowChallengePanel());
        
    }

    IEnumerator ShowChallengePanel(){
        challengePanel.alpha = 1;
        challengePanel.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        yield return StartCoroutine(FadeOut(challengePanel));
    }

    public void ShowSuccessPanel(){
        StartCoroutine(ShowPanel(successPanel));
    }

    public void ShowFailedPanel(){
        StartCoroutine(ShowPanel(failedPanel));
    }

    IEnumerator ShowPanel(CanvasGroup panel){
        panel.alpha = 1;
        panel.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayTime);

        yield return StartCoroutine(FadeOut(panel));
    }

    IEnumerator FadeOut(CanvasGroup panel){
        float startAlpha = panel.alpha;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration){
            panel.alpha = Mathf.Lerp(startAlpha,0, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.alpha = 0;
        panel.gameObject.SetActive(false);
    }
}
