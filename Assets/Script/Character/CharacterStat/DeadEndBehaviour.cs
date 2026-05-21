using System;
using UnityEngine;

public class DeadEndBehaviour : StateMachineBehaviour
{
    public static event Action OnDeadAnimFinished;

    public override void OnStateExit(Animator animator, AnimatorStateInfo info, int layerIndex)
    {
        OnDeadAnimFinished?.Invoke();
    }
}