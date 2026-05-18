using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int HeavyAttack0Hash = Animator.StringToHash("HeavyAttack0");
    private static readonly int HeavyAttack1Hash = Animator.StringToHash("HeavyAttack1");
    private static readonly int HeavyDashHash = Animator.StringToHash("HeavyDash");
    private static readonly int FinisherHash = Animator.StringToHash("Finisher");
    private static readonly int DodgeHash = Animator.StringToHash("Dodge");



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

    private void HandleAttack()
    {
        anim.SetTrigger(AttackHash);
    }

    public void SetComboIndex(int value)
    {
        leftComboIndex = value;
        Debug.Log($"Left Combo Index: {leftComboIndex}");
    }

    public void ResetCombo()
    {
        leftComboIndex = 0;
        Debug.Log($"Combo Reset");
    }


    private void HandleHeavyAttack()
    {
        switch (leftComboIndex)
        {
            case 0:
                anim.SetTrigger(HeavyAttack0Hash);
                break;
            case 1:
                anim.SetTrigger(HeavyDashHash);
                break;
            case 2:
                anim.SetTrigger(HeavyAttack1Hash);
                break;
            case 3:
                anim.SetTrigger(FinisherHash);
                break;
        }
        // 향후 자리: 스테미나 소모 체크
    }

    private void HandleDodge()
    {
        anim.SetTrigger(DodgeHash);
        // 향후 자리: 쿨타임 체크, i-frame 활성화
    }
}