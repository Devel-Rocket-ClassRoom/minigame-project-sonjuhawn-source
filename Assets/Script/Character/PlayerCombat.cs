using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int HeavyAttackHash = Animator.StringToHash("HeavyAttack");
    private static readonly int DodgeHash = Animator.StringToHash("Dodge");

    private PlayerInputHandler input;
    private Animator anim;

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
        // 향후 자리: 스테미나 체크, 콤보 인덱스 관리
    }

    private void HandleHeavyAttack()
    {
        anim.SetTrigger(HeavyAttackHash);
        // 향후 자리: 스테미나 소모 체크
    }

    private void HandleDodge()
    {
        anim.SetTrigger(DodgeHash);
        // 향후 자리: 쿨타임 체크, i-frame 활성화
    }
}