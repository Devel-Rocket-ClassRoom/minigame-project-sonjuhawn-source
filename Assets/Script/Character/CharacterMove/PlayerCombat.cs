using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CharacterStateMachine;

public enum AttackType
{
    Light1,
    Light2,
    Light3,
    Heavy1,
    Heavy2,
    HeavyDash,        // 좌클 2타 후 강공
    HeavyFinisher,    // 좌클 3타 후 강공
}

[System.Serializable]
public class AttackEntry
{
    public AttackType type;
    public float multiplier;
}

public class PlayerCombat : MonoBehaviour
{
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int HeavyAttack0Hash = Animator.StringToHash("HeavyAttack0");
    private static readonly int HeavyAttack1Hash = Animator.StringToHash("HeavyAttack1");
    private static readonly int HeavyDashHash = Animator.StringToHash("HeavyDash");
    private static readonly int FinisherHash = Animator.StringToHash("Finisher");
    private static readonly int DodgeHash = Animator.StringToHash("Dodge");
    private static readonly int DeadHash = Animator.StringToHash("Dead");

    // 모든 액션 트리거 한 곳에 모아둠 — 새 트리거 추가 시 여기에도 함께 추가
    private static readonly int[] ActionTriggers =
    {
        AttackHash, HeavyAttack0Hash, HeavyAttack1Hash,
        HeavyDashHash, FinisherHash, DodgeHash
    };

    private PlayerInputHandler input;
    private CharacterStateMachine state;
    private StaminaSystem stamina;
    private Animator anim;
    private Rigidbody rb;
    private HealthSystem health;

    private int leftComboIndex = 0;

    [SerializeField] private GameObject hitbox;   // 위의 Hitbox 자식 GameObject
    [SerializeField] private SwordHitbox sword;

    [SerializeField] private float baseAnimSpeed = 1f;
    [SerializeField] private float speedPerAgility = 0.02f;

    [SerializeField] private float dodgeDistance = 3f;
    [SerializeField] private float dodgeDuration = 0.3f;
    [SerializeField] private float dodgeCooldown = 0.4f;
    private float lastDodgeTime = -999f;

    [SerializeField] private int dodgeStaminaCost = 25;
    [SerializeField] private int heavyAttack0Cost = 25;
    [SerializeField] private int heavyAttack1Cost = 30;
    [SerializeField] private int heavyDashCost = 35;
    [SerializeField] private int finisherCost = 50;

    private IStatProvider stats;

    [SerializeField] private List<AttackEntry> attackTable;
    private Dictionary<AttackType, float> multiplierMap;

    [SerializeField] private AudioClip normalCilp;
    [SerializeField] private AudioClip hardAttackCilp;
    [SerializeField] private AudioClip finalAttackCilp;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        state = GetComponent<CharacterStateMachine>();
        health = GetComponent<HealthSystem>();
        stamina = GetComponent<StaminaSystem>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<IStatProvider>();

        multiplierMap = new Dictionary<AttackType, float>();
        foreach (var e in attackTable)
            multiplierMap[e.type] = e.multiplier;
    }

    private void OnEnable()
    {
        input.OnAttack += HandleAttack;
        input.OnHeavyAttack += HandleHeavyAttack;
        input.OnDodge += HandleDodge;
        stats.OnStatChanged += RecalculateAnimSpeed;
        health.OnDeath += HandleDeath;
        RecalculateAnimSpeed();
    }

    private void OnDisable()
    {
        input.OnAttack -= HandleAttack;
        input.OnHeavyAttack -= HandleHeavyAttack;
        input.OnDodge -= HandleDodge;
        health.OnDeath -= HandleDeath;
        stats.OnStatChanged -= RecalculateAnimSpeed;
    }

    private void SetTriggerExclusive(int hash)
    {
        for (int i = 0; i < ActionTriggers.Length; i++)
        {
            if (ActionTriggers[i] != hash)
                anim.ResetTrigger(ActionTriggers[i]);
        }
        anim.SetTrigger(hash);
    }

    private void HandleAttack()
    {
        if (state.CurrentState == PlayerState.Dodging ||
        state.CurrentState == PlayerState.Damaged ||
        state.CurrentState == PlayerState.Dead)
            return;

        SetTriggerExclusive(AttackHash);
    }

    public void OnAttackRecover()
    {
        if (stamina != null)
            stamina.RecoverByStat();
    }

    private void HandleHeavyAttack()
    {
        if (state.CurrentState == PlayerState.Dodging ||
        state.CurrentState == PlayerState.Damaged ||
        state.CurrentState == PlayerState.HeavyAttacking ||
        state.CurrentState == PlayerState.Dead)
            return;

        int cost;
        int triggerHash;

        switch (leftComboIndex)
        {
            case 0:
                cost = heavyAttack0Cost;
                triggerHash = HeavyAttack0Hash;
                break;
            case 1:
                cost = heavyAttack1Cost;
                triggerHash = HeavyAttack1Hash;
                break;
            case 2:
                cost = heavyDashCost;
                triggerHash = HeavyDashHash;
                break;
            case 3:
                cost = finisherCost;
                triggerHash = FinisherHash;
                break;
            default: return;
        }

        bool canUse = stamina.TryConsume(cost);

        if (!canUse)
            return;

        SetTriggerExclusive(triggerHash);
    }
    public void SetAttackType(AttackType type)
    {
        if (!multiplierMap.TryGetValue(type, out float mult)) mult = 1f;
        int dmg = Mathf.RoundToInt(stats.Strength * mult);
        sword.SetDamage(dmg);
    }

    private void HandleDodge()
    {
        if (state.CurrentState == PlayerState.Damaged ||
            state.CurrentState == PlayerState.HeavyAttacking ||
        state.CurrentState == PlayerState.Dead)
            return;

        if (Time.time - lastDodgeTime < dodgeCooldown)
            return;

        if (!stamina.TryConsume(dodgeStaminaCost))
            return;

        lastDodgeTime = Time.time;

        Vector3 direction = transform.forward;

        SetTriggerExclusive(DodgeHash);
        StartCoroutine(DodgeMove(direction));
    }

    private IEnumerator DodgeMove(Vector3 direction)
    {
        float elapsed = 0f;
        Vector3 velocity = direction * (dodgeDistance / dodgeDuration);

        while (elapsed < dodgeDuration)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            elapsed += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    private void HandleDeath()
    {
        state.ChangeState(PlayerState.Dead);
        SetTriggerExclusive(DeadHash);
    }

    public void SetComboIndex(int value)
    {
        leftComboIndex = value;
    }

    public void ResetCombo()
    {
        leftComboIndex = 0;

        for (int i = 0; i < ActionTriggers.Length; i++)
            anim.ResetTrigger(ActionTriggers[i]);
    }
    public void EnableHitbox() => hitbox.SetActive(true);
    public void DisableHitbox() => hitbox.SetActive(false);

    private void RecalculateAnimSpeed()
    {
        int bonus = Mathf.Max(0, stats.Agility - 10);
        anim.speed = baseAnimSpeed + bonus * speedPerAgility; ;
    }

    public void OnNormalAttackSound() => AudioManager.Instance.PlaySFX(normalCilp);
    public void OnHardAttackSound() => AudioManager.Instance.PlaySFX(hardAttackCilp);
    public void OnFinalAttackSound() => AudioManager.Instance.PlaySFX(finalAttackCilp);

}
