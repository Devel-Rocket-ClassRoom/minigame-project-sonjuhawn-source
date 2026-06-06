// PauseManager 싱글톤
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    private int pauseCount = 0;

    public int PauseCount => pauseCount;

    private void Awake() => Instance = this;

    public void Pause()
    {
        pauseCount++;
        Time.timeScale = 0f;
        ApplyCursor();
    }
    public void Resume()
    {
        pauseCount = Mathf.Max(0, pauseCount - 1);
        if (pauseCount == 0) Time.timeScale = 1f;
        ApplyCursor();
    }
    private void ApplyCursor()
    {
        bool uiOpen = pauseCount > 0;
        Cursor.visible = uiOpen;
        Cursor.lockState = uiOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }
}