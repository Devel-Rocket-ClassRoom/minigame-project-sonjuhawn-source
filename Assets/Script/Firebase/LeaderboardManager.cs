using Cysharp.Threading.Tasks;
using Firebase.Database;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    private static LeaderboardManager instance;
    public static LeaderboardManager Instance => instance;

    private DatabaseReference leaderboardRef;
    private Query listenerQuery;
    private bool isListenerActive;
    public event Action<List<LeaderboardEntry>> OnLeaderboardUpdated;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private async UniTaskVoid Start()
    {
        if (!await FirebaseInitializer.Instance.WaitForInitializationAsync())
        {
            Debug.LogError("[Leaderboard] Firebase 초기화 실패");
            return;
        }

        leaderboardRef = FirebaseInitializer.Instance.Database.RootReference.Child("leaderboard");
    }

    private void OnDestroy()
    {
        StopRealtimeListener();
    }

    public async UniTask<(bool success, string error)> SaveToLeaderboardAsync(
    float clearTime, string displayName)
    {
        if (!AuthManager.Instance.IsLoggedIn)
        {
            return (false, "로그인이 필요합니다");
        }

        if (leaderboardRef == null)
        {
            return (false, "leaderboardRef null");
        }

        string userId = AuthManager.Instance.UserId;

        try
        {
            Debug.Log($"[Leaderboard] 시도");

            Dictionary<string, object> entryData = new Dictionary<string, object>
            {
                {"userId", userId},
                {"displayName", displayName},
                {"clearTime", clearTime},
                {"timestamp", ServerValue.Timestamp}
            };

            await leaderboardRef.Child(userId).UpdateChildrenAsync(entryData);
            Debug.Log($"[Leaderboard] 성공");
            return (true, null);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Leaderboard] 저장실패: {ex.Message}");
            return (false, ex.Message);
        }
    }

    public async UniTask<List<LeaderboardEntry>> LoadLeaderboardAsync(int limit = 5)
    {
        if (leaderboardRef == null)
        {
            return new List<LeaderboardEntry>();
        }

        try
        {
            Debug.Log($"[Leaderboard] 로드 시도");

            Query query = leaderboardRef.OrderByChild("clearTime").LimitToFirst(limit);
            DataSnapshot snapshot = await query.GetValueAsync();
            List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();

            Debug.Log($"[Leaderboard] 로드 성공");
            return leaderboard;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Leaderboard] 로드 실패: {ex.Message}");
            return new List<LeaderboardEntry>();
        }
    }

    public List<LeaderboardEntry> ParseEntries(DataSnapshot snapshot)
    {
        List<LeaderboardEntry> list = new List<LeaderboardEntry>();

        if (snapshot.Exists)
        {
            foreach (DataSnapshot child in snapshot.Children)
            {
                list.Add(LeaderboardEntry.FromJson(child.GetRawJsonValue()));
            }
        }
        list.Sort((a, b) => a.clearTime.CompareTo(b.clearTime));
        return list;
    }

    public void StartRealtimeListener(int limit = 10)
    {
        if (isListenerActive || leaderboardRef == null)
        {
            return;
        }
        Debug.Log("[Leaderboard] 실시간 리스너 시작");

        listenerQuery = leaderboardRef.OrderByChild("clearTime").LimitToFirst(limit);
        listenerQuery.ValueChanged += OnvalueChanged;
        isListenerActive = true;
    }

    public void StopRealtimeListener()
    {
        if (isListenerActive && listenerQuery != null)
        {
            Debug.Log("[Leaderboard] 실시간 리스너 중지");
            listenerQuery.ValueChanged -= OnvalueChanged;
            listenerQuery = null;
            isListenerActive = false;
        }
    }

    private void OnvalueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError($"[Leaderboard] 리스너 오류: {args.DatabaseError.Message}");
            return;
        }

        List<LeaderboardEntry> leaderboard = ParseEntries(args.Snapshot);
        dispatchUpdateAsync(leaderboard).Forget();
    }

    private async UniTaskVoid dispatchUpdateAsync(List<LeaderboardEntry> leaderboard)
    {
        await UniTask.SwitchToMainThread();
        OnLeaderboardUpdated?.Invoke(leaderboard);
    }
}
