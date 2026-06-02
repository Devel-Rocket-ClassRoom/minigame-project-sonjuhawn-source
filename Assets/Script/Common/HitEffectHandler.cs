using UnityEngine;

public class HitEffectHandler : MonoBehaviour
{
    [SerializeField] private GameObject hitEffectPrefab;
    private HealthSystem health;
    private MonsterHealth monsterHealth;
    private BossHealth bossHealth;

    [SerializeField] private AudioClip hitClip;


    private void Awake()
    {
        health = GetComponent<HealthSystem>();
        monsterHealth = GetComponent<MonsterHealth>();
        bossHealth = GetComponent<BossHealth>();
    }

    private void OnEnable()
    {
        if (health != null) health.OnDamaged += PlayEffect;
        if (monsterHealth != null) monsterHealth.OnDamaged += PlayEffect;
        if (bossHealth != null) bossHealth.OnDamaged += PlayEffect;
    }

    private void OnDisable()
    {
        if (health != null) health.OnDamaged -= PlayEffect;
        if (monsterHealth != null) monsterHealth.OnDamaged -= PlayEffect;
        if (bossHealth != null) bossHealth.OnDamaged -= PlayEffect;
    }

    private void PlayEffect()
    {
        var vfx = Instantiate(hitEffectPrefab, transform.position + Vector3.up * 0.2f, Quaternion.identity);
        Destroy(vfx, 0.5f);
        if (hitClip != null) AudioManager.Instance.PlaySFX(hitClip);
    }
}