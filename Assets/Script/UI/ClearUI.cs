using System.Collections;
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
        StartCoroutine(ShowStats());
    }

    private IEnumerator ShowStats()
    {
        buttonsPanel.SetActive(false);

        killsText.gameObject.SetActive(false);
        spentText.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(delayBetween);
        killsText.text = $"Kill: {waveManager.TotalKills}";
        killsText.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(delayBetween);
        spentText.text = $"Spending Gold: {gold.TotalSpent}";
        spentText.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(delayBetween);
        int minutes = (int)(waveManager.ElapsedTime / 60);
        int seconds = (int)(waveManager.ElapsedTime % 60);
        timeText.text = $"Clear Time: {minutes:00}:{seconds:00}";
        timeText.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(delayBetween);
        buttonsPanel.SetActive(true); // 마지막에 버튼 활성화
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