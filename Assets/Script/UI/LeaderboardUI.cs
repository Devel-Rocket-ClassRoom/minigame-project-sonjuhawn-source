using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private Button closeButton;
    [SerializeField] private Transform entryContainer;  
    [SerializeField] private GameObject entryPrefab;   

    private async UniTaskVoid Start()
    {
        closeButton.onClick.AddListener(Close);
        leaderboardPanel.SetActive(false);

        await UniTask.WaitUntil(() => LeaderboardManager.Instance.IsReady);
        await LoadAndDisplayAsync(); 
    }

    private async UniTaskVoid OpenAsync()
    {
        await UniTask.WaitUntil(() => LeaderboardManager.Instance.IsReady);
        await LoadAndDisplayAsync();  // 최신 데이터로 갱신
        leaderboardPanel.SetActive(true);
    }

    private async UniTask LoadAndDisplayAsync()
    {
        for (int i = entryContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(entryContainer.GetChild(i).gameObject);
        }

        List<LeaderboardEntry> entries = await LeaderboardManager.Instance.LoadLeaderboardAsync();

        for (int i = 0; i < entries.Count; i++)
        {
            GameObject entry = Instantiate(entryPrefab, entryContainer);
            TMP_Text[] texts = entry.GetComponentsInChildren<TMP_Text>();

            int minutes = (int)(entries[i].clearTime / 60);
            int seconds = (int)(entries[i].clearTime % 60);

            texts[0].text = $"{i + 1}.";
            texts[1].text = entries[i].displayName;
            texts[2].text = $"{minutes:00}:{seconds:00}";
        }
    }

    public void Open()
    {
        OpenAsync().Forget();
    }
    private void Close()
    {
        leaderboardPanel.SetActive(false);
    }
}