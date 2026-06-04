using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MonsterProjectile : MonoBehaviour
{
    private int damage;
    private float speed;
    private float lifeTime = 5f;
    private float spawnTime;

    [SerializeField] private GameObject hitEffectPrefab;

    public void Init(int dmg, float spd, Vector3 direction)
    {
        damage = dmg;
        speed = spd;
        spawnTime = Time.time;
        if (direction.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(direction.normalized);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        if (Time.time - spawnTime >= lifeTime) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Proj] HIT: {other.name} (tag={other.tag})");
        if (other.CompareTag("Projectile")) return;
        if (other.CompareTag("Monster")) return;

        if (hitEffectPrefab != null)
        {
            var vfx = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(vfx, 2f);
        }

        var hp = other.GetComponent<HealthSystem>();
        if (hp != null) hp.TakeDamage(damage);
        Destroy(gameObject);                              
    }
}