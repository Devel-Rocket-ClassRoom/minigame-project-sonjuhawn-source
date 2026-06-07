using UnityEngine;
using UnityEngine.InputSystem;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    private InputSystem_Actions _actions;

    [SerializeField] private StartPanel startPanel;
    [SerializeField] private OptionPanel optionPanel;
    [SerializeField] private GameObject clearUIPanel;
    [SerializeField] private GameObject restartPanel;

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
        if (optionPanel.IsOpen) return;
        if (clearUIPanel != null && clearUIPanel.activeSelf) return;
        if (restartPanel != null && restartPanel.activeSelf) return;
        if (panel.activeSelf) Resume();
        else Pause();
    }
}