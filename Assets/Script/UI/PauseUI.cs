using UnityEngine;
using UnityEngine.InputSystem;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    private InputSystem_Actions _actions;

    [SerializeField] private StartPanel startPanel;
    [SerializeField] private OptionPanel optionPanel;

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
    }

    public void Resume()
    {
        panel.SetActive(false);
        PauseManager.Instance.Resume();
    }

    private void HandlePause(InputAction.CallbackContext _)
    {
        if (!startPanel.IsGameStarted) return;
        if (optionPanel.IsOpen) return; // 추가
        if (panel.activeSelf) Resume();
        else Pause();
    }
}