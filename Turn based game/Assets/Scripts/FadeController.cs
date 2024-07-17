using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    private Image blackImage;
    public float fadeDuration = 1.0f;

    private void Awake()
    {
        blackImage = GetComponent<Image>();
    }

    public void FadeToBlack()
    {
        StartCoroutine(FadeImage(true));
    }

    public void FadeFromBlack()
    {
        StartCoroutine(FadeImage(false));
    }

    private IEnumerator FadeImage(bool fadeToBlack)
    {
        float elapsedTime = 0f;
        Color color = blackImage.color;
        float startAlpha = color.a;
        float endAlpha = fadeToBlack ? 1f : 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            blackImage.color = new Color(color.r, color.g, color.b, newAlpha);

            yield return null;
        }
    }

    public void Blink()
    {
        StartCoroutine(BlinkCoroutine());
    }

    private IEnumerator BlinkCoroutine()
    {
        yield return new WaitForSeconds(1);
        FadeToBlack();
        yield return new WaitForSeconds(2);
        FadeFromBlack();
    }
}