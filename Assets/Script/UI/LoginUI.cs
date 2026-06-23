using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject nicknamePanel;  
    [SerializeField] private GameObject startPanel;    

    [Header("Login Form")]
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button signupButton;
    [SerializeField] private Button anonymousButton;
    [SerializeField] private TextMeshProUGUI errorText;

    private async UniTaskVoid Start()
    {
        await UniTask.WaitUntil(() => AuthManager.Instance.IsInitalized);

        loginButton.onClick.AddListener(() => OnLoginClicked().Forget());
        signupButton.onClick.AddListener(() => OnSignupClicked().Forget());
        anonymousButton.onClick.AddListener(() => OnAnonymousClicked().Forget());

        if (AuthManager.Instance.IsLoggedIn)
        {
            ShowStartPanel();
        }
    }

    private async UniTaskVoid OnLoginClicked()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowError("이메일과 비밀번호를 입력하세요");
            return;
        }

        SetButtonsInteractable(false);
        var (success, error) = await AuthManager.Instance.SignInUserWithEmailAsync(email, password);
        if (success)
        {
            var (profile, _) = await ProfileManager.Instance.LoadProfileAsync();
            if (profile != null)
            {
                ShowStartPanel();
            }
            else
            {
                loginPanel.SetActive(false);
                nicknamePanel.SetActive(true);
            }
        }
        else
        {
            ShowError(error);
        }
        SetButtonsInteractable(true);
    }

    private async UniTaskVoid OnSignupClicked()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        var (success, error) = await AuthManager.Instance.CreateUserWithEmailAsync(email, password);
        if (success)
        {
            loginPanel.SetActive(false);
            nicknamePanel.SetActive(true);
        }
        else
        {
            ShowError(error);
        }
    }

    private async UniTaskVoid OnAnonymousClicked()
    {
        var (success, error) = await AuthManager.Instance.SignInAnonymouslyAsync();
        if (success)
        {
            ShowStartPanel();
        }
        else
        {
            ShowError(error);
        }
    }

    private void ShowStartPanel()
    {
        loginPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    private void ShowError(string message)
    {
        errorText.text = message;
        errorText.color = Color.red;
    }

    private void SetButtonsInteractable(bool interactable)
    {
        loginButton.interactable = interactable;
        signupButton.interactable = interactable;
        anonymousButton.interactable = interactable;
    }

    public void ShowLoginPanel()
    {
        startPanel.SetActive(false);
        loginPanel.SetActive(true);
    }
}
