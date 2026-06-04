using System.Collections;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private float riseSpeed = 50f;

    private RectTransform rectTransform;
    private Vector3 worldPos;

    public void Init(int damage, Vector3 position)
    {
        rectTransform = GetComponent<RectTransform>();
        worldPos = position + Vector3.up * 1.5f;
        damageText.text = damage.ToString();
        StartCoroutine(AnimateAndDestroy());
    }

    private IEnumerator AnimateAndDestroy()
    {
        float elapsed = 0f;
        Color color = damageText.color;

        while (elapsed < lifetime)
        {
            elapsed += Time.deltaTime;

            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            rectTransform.position = screenPos + Vector3.up * (riseSpeed * elapsed);
            color.a = 1f - (elapsed / lifetime);
            damageText.color = color;
            yield return null;
        }
        Destroy(gameObject);
    }
}