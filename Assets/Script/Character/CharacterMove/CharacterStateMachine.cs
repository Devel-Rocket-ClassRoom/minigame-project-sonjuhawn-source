using System;
using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    public enum PlayerState
    {
        Idle, 
        Moving, 
        Attacking, 
        HeavyAttacking, 
        Dodging, 
        Damaged, 
        Dead
    }

    public PlayerState CurrentState {  get; private set; }
    public bool IsInvincible { get; set; }

    public event Action<PlayerState> OnStateChanged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeState(PlayerState.Idle);
    }

    public void ChangeState(PlayerState newState)
    {
        if (CurrentState == newState) return;  
        CurrentState = newState;
        OnStateChanged?.Invoke(newState);
        Debug.Log($"State: {newState}");
    }

    public bool IsState(PlayerState state)
    {
        return CurrentState == state;
    }
}
