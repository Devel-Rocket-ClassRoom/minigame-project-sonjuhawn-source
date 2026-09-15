using System;
using UnityEngine;

[Serializable]
public class LeaderboardEntry
{
    public string userId;
    public string displayName;  // 닉네임 or "Anonymous"
    public float clearTime;
    public int kills;
    public int goldSpent;
    public long timestamp;

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public static LeaderboardEntry FromJson(string json)
    {
        return JsonUtility.FromJson<LeaderboardEntry>(json);
    }
}
