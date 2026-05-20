using UnityEngine;

public class PlayerStateBehaviour : StateMachineBehaviour
{
    [SerializeField] private CharacterStateMachine.PlayerState targetState;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var sm = animator.GetComponent<CharacterStateMachine>();
        if (sm != null)
            sm.ChangeState(targetState);
    }
}