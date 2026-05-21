using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartPanel : MonoBehaviour
{
    public void OnClickRestart()
    {
        Time.timeScale = 1f;  // 혹시 pause 상태면 해제
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickQuit() { Application.Quit(); }
}