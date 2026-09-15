using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NicknamePanelUI : MonoBehaviour
{
    [SerializeField] private GameObject nicknamePanel;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private TMP_InputField nicknameInput;
    [SerializeField] private Button confirmButton;
    [SerializeField] private TextMeshProUGUI errorText;

    private void Start()
    {
        confirmButton.onClick.AddListener(() => OnConfirmClicked().Forget());
    }

    private async UniTaskVoid OnConfirmClicked()
    {
        string nickname = nicknameInput.text.Trim();

        if (string.IsNullOrEmpty(nickname))
        {
            errorText.text = "닉네임을 입력하세요";
            return;
        }

        confirmButton.interactable = false;

        var (success, error) = await ProfileManager.Instance.SaveProfileAsync(nickname);
        if (success)
        {
            nicknamePanel.SetActive(false);
            startPanel.SetActive(true);
        }
        else
        {
            errorText.text = error;
        }

        confirmButton.interactable = true;
    }
}
