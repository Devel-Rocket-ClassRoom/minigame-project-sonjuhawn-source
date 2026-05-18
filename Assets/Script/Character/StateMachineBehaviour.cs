using UnityEngine;

public class ComboIndexBehaviour : StateMachineBehaviour
{
    [SerializeField] private int comboIndex = 0;  // 이 상태가 콤보 몇 번째인지

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var combat = animator.GetComponent<PlayerCombat>();
        if (combat != null)
            combat.SetComboIndex(comboIndex);
    }
}