using Cysharp.Threading.Tasks;
using UnityEngine;

public class VFXPrewarmer : MonoBehaviour
{
    [SerializeField] private GameObject[] effectsToPrewarm;

    private void Start()
    {
        PrewarmAsync().Forget();
    }

    private async UniTaskVoid PrewarmAsync()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Vector3 spawnPos = cam.transform.position + cam.transform.forward * 2f;

        foreach (var prefab in effectsToPrewarm)
        {
            if (prefab == null) continue;

            var inst = Instantiate(prefab, spawnPos, Quaternion.identity);
            inst.transform.localScale = Vector3.one * 0.01f;

            foreach (var ps in inst.GetComponentsInChildren<ParticleSystem>())
                ps.Play();

            await UniTask.Yield(); // 1프레임
            await UniTask.Yield(); // 2프레임
            Destroy(inst);
        }
    }
}