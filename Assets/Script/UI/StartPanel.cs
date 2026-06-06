using Unity.Cinemachine;
using UnityEngine;

public class StartPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private CameraTargetFollow cameraFollow;
    [SerializeField] private Transform menuCameraPoint;
    [SerializeField] private CinemachineCamera gameCamera;
    [SerializeField] private GameObject hudCanvas;
    public bool IsGameStarted { get; private set; } = false;

    private void Start()
    {
        panel.SetActive(true);
        hudCanvas.SetActive(false);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameCamera.enabled = false;
        Camera.main.transform.position = menuCameraPoint.position;
        Camera.main.transform.rotation = menuCameraPoint.rotation;
    }

    public void OnStartButton()
    {
        IsGameStarted = true;
        hudCanvas.SetActive(true);
        panel.SetActive(false);
        Time.timeScale = 1f;
        gameCamera.enabled = true;
    }
}