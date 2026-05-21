using UnityEngine;

public class AttackTypeBehaviour : StateMachineBehaviour
{
    [SerializeField] private AttackType type;
    [SerializeField] private int comboIndex = -1;  // -1이면 호출 안 함

    public override void OnStateEnter(Animator animator, AnimatorStateInfo info, int layer)
    {
        var combat = animator.GetComponent<PlayerCombat>();
        if (combat == null) return;

        combat.SetAttackType(type);
        if (comboIndex >= 0) combat.SetComboIndex(comboIndex);
    }
}