using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private Button closeButton;
    [SerializeField] private Transform entryContainer;  // 리스트 부모 오브젝트
    [SerializeField] private GameObject entryPrefab;    // 항목 프리팹 (닉네임+시간 텍스트)

    private void Start()
    {
        closeButton.onClick.AddListener(Close);
        leaderboardPanel.SetActive(false);
    }

    private async UniTaskVoid OpenAsync()
    {
        leaderboardPanel.SetActive(true);
        await LoadAndDisplayAsync();
    }

    private async UniTask LoadAndDisplayAsync()
    {
        foreach (Transform child in entryContainer)
            Destroy(child.gameObject);

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