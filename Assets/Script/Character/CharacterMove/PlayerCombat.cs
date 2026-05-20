using System.Collections;
using UnityEngine;
using static CharacterStateMachine;

public class PlayerCombat : MonoBehaviour
{
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int HeavyAttack0Hash = Animator.StringToHash("HeavyAttack0");
    private static readonly int HeavyAttack1Hash = Animator.StringToHash("HeavyAttack1");
    private static readonly int HeavyDashHash = Animator.StringToHash("HeavyDash");
    private static readonly int FinisherHash = Animator.StringToHash("Finisher");
    private static readonly int DodgeHash = Animator.StringToHash("Dodge");

    // 모든 액션 트리거 한 곳에 모아둠 — 새 트리거 추가 시 여기에도 함께 추가
    private static readonly int[] ActionTriggers =
    {
        AttackHash, HeavyAttack0Hash, HeavyAttack1Hash,
        HeavyDashHash, FinisherHash, DodgeHash
    };

    private PlayerInputHandler input;
    private CharacterStateMachine state;
    private CharacterMover mover;
    private StaminaSystem stamina;
    private Animator anim;
    private Rigidbody rb;

    private int leftComboIndex = 0;

    [SerializeField] private float dodgeDistance = 3f;
    [SerializeField] private float dodgeDuration = 0.3f;
    [SerializeField] private float dodgeCooldown = 0.4f;
    private float lastDodgeTime = -999f;

    [SerializeField] private int dodgeStaminaCost = 25;
    [SerializeField] private int heavyAttack0Cost = 25;
    [SerializeField] private int heavyAttack1Cost = 30;
    [SerializeField] private int heavyDashCost = 35;
    [SerializeField] private int finisherCost = 50;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        state = GetComponent<CharacterStateMachine>();
        mover = GetComponent<CharacterMover>();
        stamina = GetComponent<StaminaSystem>();    
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
    }

    private void OnEnable()
    {
        input.OnAttack += HandleAttack;
        input.OnHeavyAttack += HandleHeavyAttack;
        input.OnDodge += HandleDodge;
    }

    private void OnDisable()
    {
        input.OnAttack -= HandleAttack;
        input.OnHeavyAttack -= HandleHeavyAttack;
        input.OnDodge -= HandleDodge;
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
            stamina.Recover(15);
    }

    private void HandleHeavyAttack()
    {
        if (state.CurrentState == PlayerState.Dodging ||
        state.CurrentState == PlayerState.Damaged ||
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
            rb.MovePosition(rb.position + velocity * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void SetComboIndex(int value)
    {
        leftComboIndex = value;
    }

    public void ResetCombo()
    {
        leftComboIndex = 0;
        // 잔존 트리거 일괄 정리 — Idle 복귀 시 호출
        for (int i = 0; i < ActionTriggers.Length; i++)
            anim.ResetTrigger(ActionTriggers[i]);
    }
}
