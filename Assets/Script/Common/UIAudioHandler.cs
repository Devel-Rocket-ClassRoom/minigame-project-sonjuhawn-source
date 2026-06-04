using UnityEngine;

public class UIAudioHandler : MonoBehaviour
{
    [SerializeField] private AudioClip clickClip;

    public void OnClick()
    {
        AudioManager.Instance.PlaySFX(clickClip);
    }
}