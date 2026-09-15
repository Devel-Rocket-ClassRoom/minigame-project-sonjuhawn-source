using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private GameObject hudCanvas;
    [SerializeField] private GoldSystem gold;

    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text spentText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject buttonsPanel;

    [SerializeField] private float delayBetween = 1f;

    private void OnEnable()
    {
        waveManager.OnAllWavesCleared += ShowClear;
    }

    private void OnDisable()
    {
        waveManager.OnAllWavesCleared -= ShowClear;
    }

    private void ShowClear()
    {
        hudCanvas.SetActive(false);
        panel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        var inputProvider = FindAnyObjectByType<CinemachineCamera>();
        if (inputProvider != null) inputProvider.enabled = false;
        ShowStatsAsync().Forget();
    }

    private async UniTaskVoid ShowStatsAsync()
    {
        buttonsPanel.SetActive(false);

        killsText.gameObject.SetActive(false);
        spentText.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);

        await UniTask.Delay(System.TimeSpan.FromSeconds(delayBetween), DelayType.UnscaledDeltaTime);
        killsText.text = $"Kill: {waveManager.TotalKills}";
        killsText.gameObject.SetActive(true);

        await UniTask.Delay(System.TimeSpan.FromSeconds(delayBetween), DelayType.UnscaledDeltaTime);
        spentText.text = $"Spending Gold: {gold.TotalSpent}";
        spentText.gameObject.SetActive(true);

        await UniTask.Delay(System.TimeSpan.FromSeconds(delayBetween), DelayType.UnscaledDeltaTime);
        int minutes = (int)(waveManager.ElapsedTime / 60);
        int seconds = (int)(waveManager.ElapsedTime % 60);
        timeText.text = $"Clear Time: {minutes:00}:{seconds:00}";
        timeText.gameObject.SetActive(true);

        await UniTask.Delay(System.TimeSpan.FromSeconds(delayBetween), DelayType.UnscaledDeltaTime);
        buttonsPanel.SetActive(true);

        await ClearTimeManager.Instance.SubmitClearTimeAsync(
        waveManager.ElapsedTime, waveManager.TotalKills, gold.TotalSpent);
        }

    public void OnClickRestart()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}