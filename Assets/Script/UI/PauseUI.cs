using UnityEngine;
using UnityEngine.InputSystem;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    private InputSystem_Actions _actions;

    private void Awake()
    {
        _actions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _actions.UI.Enable();
        _actions.UI.Pause.performed += HandlePause;
    }

    private void OnDisable()
    {
        _actions.UI.Pause.performed -= HandlePause;
        _actions.UI.Disable();
    }

    private void Pause()
    {
        panel.SetActive(true);
        PauseManager.Instance.Pause();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        panel.SetActive(false);
        PauseManager.Instance.Resume();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void HandlePause(InputAction.CallbackContext _)
    {
        if (panel.activeSelf) Resume();
        else Pause();
    }
}