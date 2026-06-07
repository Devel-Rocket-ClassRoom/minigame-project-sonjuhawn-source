using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private InputSystem_Actions _actions;

    // === 폴링용 (연속 입력) ===
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }   // 카메라는 Cinemachine이 직접 읽지만 일관성 위해 노출 가능

    // === 이벤트용 (단발 입력) ===
    public event Action OnAttack;
    public event Action OnHeavyAttack;
    public event Action OnDodge;
    public event Action OnSkill;       
    public event Action OnPotion;

    private bool _isPointerOverUI;

    private void Awake()
    {
        _actions = new InputSystem_Actions();
    }

    private void OnDestroy() => _actions.Dispose();

    private void OnEnable()
    {
        _actions.Player.Enable();

        _actions.Player.Attack.performed += HandleAttack;
        _actions.Player.HeavyAttack.performed += HandleHeavyAttack;
        _actions.Player.Dodge.performed += HandleDodge;
        _actions.Player.Skill.performed += HandleSkill;
        _actions.Player.Potion.performed += HandlePotion; 

    }

    private void OnDisable()
    {
        _actions.Player.Attack.performed -= HandleAttack;
        _actions.Player.HeavyAttack.performed -= HandleHeavyAttack;
        _actions.Player.Dodge.performed -= HandleDodge;
        _actions.Player.Skill.performed -= HandleSkill;
        _actions.Player.Potion.performed -= HandlePotion;

        _actions.Player.Disable();
    }

    private void Update()
    {
        _isPointerOverUI = EventSystem.current != null &&
                       EventSystem.current.IsPointerOverGameObject();
        MoveInput = _actions.Player.Move.ReadValue<Vector2>();
        LookInput = _actions.Player.Look.ReadValue<Vector2>();
    }

    // === 내부 핸들러: InputAction 콜백 → 이벤트 호출 ===
    private void HandleAttack(InputAction.CallbackContext _)
    {
        if (_isPointerOverUI) return;
        OnAttack?.Invoke();
    }
    private void HandleHeavyAttack(InputAction.CallbackContext _)
    {
        if (_isPointerOverUI) return;
        OnHeavyAttack?.Invoke();
    }
    private void HandleDodge(InputAction.CallbackContext _) => OnDodge?.Invoke();
    private void HandleSkill(InputAction.CallbackContext _) => OnSkill?.Invoke();
    private void HandlePotion(InputAction.CallbackContext _) => OnPotion?.Invoke();

}