using Cysharp.Threading.Tasks;
using UnityEngine;

public class ClearTimeManager : MonoBehaviour
{
    public static ClearTimeManager instance;
    public static ClearTimeManager Instance => instance;

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
            Debug.LogError("[ClearTime] Firebase 초기화 실패");
            return;
        }

        await UniTask.WaitUntil(() => AuthManager.Instance.IsInitalized);
        await UniTask.WaitUntil(() => ProfileManager.Instance.IsInitialized);

        Debug.Log("[ClearTime] 초기화 완료");
    }

    public async UniTask SubmitClearTimeAsync(float clearTime, int kills, int goldSpent)
    {
        if (!AuthManager.Instance.IsLoggedIn) return;

        string uid = AuthManager.Instance.UserId;
        bool isAnonymous = AuthManager.Instance.CurrentUser.IsAnonymous;

        string displayName;
        if (isAnonymous)
        {
            displayName = "Anonymous";
        }
        else
        {
            if (ProfileManager.Instance.CachedProfile == null)
                await ProfileManager.Instance.LoadProfileAsync();

            displayName = ProfileManager.Instance.CachedProfile?.nickname
                ?? AuthManager.Instance.CurrentUser.Email;
        }

        await LeaderboardManager.Instance.SaveToLeaderboardAsync(clearTime, displayName);
    }
}
