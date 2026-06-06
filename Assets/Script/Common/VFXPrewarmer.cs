using System.Collections;
using UnityEngine;

public class VFXPrewarmer : MonoBehaviour
{
    [SerializeField] private GameObject[] effectsToPrewarm;

    private void Start()
    {
        StartCoroutine(Prewarm());
    }

    private IEnumerator Prewarm()
    {
        Camera cam = Camera.main;
        if (cam == null) yield break;

        Vector3 spawnPos = cam.transform.position + cam.transform.forward * 2f;

        foreach (var prefab in effectsToPrewarm)
        {
            if (prefab == null) continue;

            var inst = Instantiate(prefab, spawnPos, Quaternion.identity);
            inst.transform.localScale = Vector3.one * 0.01f;

            foreach (var ps in inst.GetComponentsInChildren<ParticleSystem>())
                ps.Play();

            yield return null;
            yield return null; // 2프레임 렌더링

            Destroy(inst);
        }
    }
}