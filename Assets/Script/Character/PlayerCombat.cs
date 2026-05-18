using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int HeavyAttackHash = Animator.StringToHash("HeavyAttack");
    private static readonly int DodgeHash = Animator.StringToHash("Dodge");
    private static readonly int FinisherHash = Animator.StringToHash("Finisher");


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
        if(leftComboIndex == 3)
        {
            anim.SetTrigger(FinisherHash);
        }
        else
        {
            anim.SetTrigger(HeavyAttackHash);
        }
        // 향후 자리: 스테미나 소모 체크
    }

    private void HandleDodge()
    {
        anim.SetTrigger(DodgeHash);
        // 향후 자리: 쿨타임 체크, i-frame 활성화
    }

    
}