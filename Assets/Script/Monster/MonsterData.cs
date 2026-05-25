using UnityEngine;

[CreateAssetMenu(fileName = "NewMonsterData", menuName = "Game/Monster Data")]
public class MonsterData : ScriptableObject
{
    [Header("Identity")]
    public string id = "turnipa";
    public string displayName = "순무";

    [Header("Combat")]
    public int maxHp = 30;
    public int attackPower = 10;

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("AI Ranges")]
    public float detectRange = 8f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;

    [Header("Rewards")]
    public int expReward = 10;
    public int goldReward = 5;

    [Header("Stagger")]
    public float staggerDuration = 0.4f;

    [Header("Telegraph Attack (Optional)")]
    public float telegraphTime = 0f;   
    public int heavyAttackPower = 0;
    public float heavyRecoveryTime = 1.5f;

    [Header("Ranged (Optional)")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float rangedAttackRange = 8f;
    public float rangedRecoveryTime = 1.2f;
    public Vector3 muzzleLocalOffset = new Vector3(0, 1f, 0.5f);
    public float kiteDistance = 4f;   // 이 거리보다 가까우면 도망

}