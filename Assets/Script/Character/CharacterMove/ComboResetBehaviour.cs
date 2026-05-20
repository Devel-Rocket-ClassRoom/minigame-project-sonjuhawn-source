using UnityEngine;

public class ComboResetBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var combat = animator.GetComponent<PlayerCombat>();
        if (combat != null)
            combat.ResetCombo();
    }
}