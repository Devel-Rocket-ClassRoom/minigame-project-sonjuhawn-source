using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;

public class ToastManager : MonoBehaviour
{
    public static ToastManager Instance { get; private set; }

    [SerializeField] private TMP_Text toastText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float displayTime = 1.5f;
    [SerializeField] private float fadeTime = 0.5f;

    private CancellationTokenSource _cts;


    private void Awake()
    {
       Instance = this;
    }

    public void Show(string message)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        ShowRoutineAsync(message, _cts.Token).Forget();
    }

    private async UniTaskVoid ShowRoutineAsync(string message, CancellationToken ct)
    {
        toastText.text = message;
        canvasGroup.alpha = 1f;

        await UniTask.Delay(System.TimeSpan.FromSeconds(displayTime),
            DelayType.UnscaledDeltaTime, cancellationToken: ct);

        float elapsed = 0f;
        while (elapsed < fadeTime)
        {
            if (ct.IsCancellationRequested) return;
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeTime);
            await UniTask.Yield();
        }
        canvasGroup.alpha = 0f;
    }
}