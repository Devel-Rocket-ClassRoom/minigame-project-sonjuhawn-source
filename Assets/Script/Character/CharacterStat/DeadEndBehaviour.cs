using System;
using UnityEngine;

public class DeadEndBehaviour : StateMachineBehaviour
{
    public static event Action OnDeadAnimFinished;
    private bool fired = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo info, int layerIndex)
    {
        fired = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo info, int layerIndex)
    {
        if (!fired && info.normalizedTime >= 0.95f)
        {
            fired = true;
            OnDeadAnimFinished?.Invoke();
        }
    }
}