using System.Collections;
using TMPro;
using UnityEngine;

public class ToastManager : MonoBehaviour
{
    public static ToastManager Instance { get; private set; }

    [SerializeField] private TMP_Text toastText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float displayTime = 1.5f;
    [SerializeField] private float fadeTime = 0.5f;

    private Coroutine current;

    private void Awake()
    {
       Instance = this;
    }

    public void Show(string message)
    {
        if (current != null) StopCoroutine(current);
        current = StartCoroutine(ShowRoutine(message));
    }

    private IEnumerator ShowRoutine(string message)
    {
        toastText.text = message;
        canvasGroup.alpha = 1f;
        yield return new WaitForSecondsRealtime(displayTime);
        float elapsed = 0f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeTime);
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}