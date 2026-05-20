using UnityEngine;

public class InvincibilityBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var sm = animator.GetComponent<CharacterStateMachine>();
        if (sm != null)
            sm.IsInvincible = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var sm = animator.GetComponent<CharacterStateMachine>();
        if (sm != null)
            sm.IsInvincible = false;
    }
}