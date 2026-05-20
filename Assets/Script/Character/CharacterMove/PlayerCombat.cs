using UnityEngine;

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
    private Animator anim;

    private int leftComboIndex = 0;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        anim = GetComponent<Animator>();
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

    /// <summary>
    /// 지정한 트리거 하나만 켜고 다른 액션 트리거는 모두 끈다.
    /// 트리거 누적으로 인한 의도치 않은 콤보 진행 방지.
    /// </summary>
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
        SetTriggerExclusive(AttackHash);
    }

    private void HandleHeavyAttack()
    {
        switch (leftComboIndex)
        {
            case 0: SetTriggerExclusive(HeavyAttack0Hash); break;
            case 1: SetTriggerExclusive(HeavyAttack1Hash);    break;
            case 2: SetTriggerExclusive(HeavyDashHash); break;
            case 3: SetTriggerExclusive(FinisherHash);     break;
        }
        // 향후 자리: 스테미나 소모 체크
    }

    private void HandleDodge()
    {
        SetTriggerExclusive(DodgeHash);
        // 향후 자리: 쿨타임 체크, i-frame 활성화
    }

    public void SetComboIndex(int value)
    {
        leftComboIndex = value;
        Debug.Log($"Left Combo Index: {leftComboIndex}");
    }

    public void ResetCombo()
    {
        leftComboIndex = 0;
        // 잔존 트리거 일괄 정리 — Idle 복귀 시 호출
        for (int i = 0; i < ActionTriggers.Length; i++)
            anim.ResetTrigger(ActionTriggers[i]);
        Debug.Log("Combo Reset");
    }
}
