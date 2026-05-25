using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MonsterProjectile : MonoBehaviour
{
    private int damage;
    private float speed;
    private float lifeTime = 5f;
    private float spawnTime;

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
        Debug.Log($"pos={transform.position}");
        transform.position += transform.forward * speed * Time.deltaTime;
        if (Time.time - spawnTime >= lifeTime) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Hit: {other.name}, tag={other.tag}");

        if (other.CompareTag("Monster")) return;          // 자기편 무시
        var hp = other.GetComponent<HealthSystem>();
        if (hp != null) hp.TakeDamage(damage);
        Destroy(gameObject);                               // 벽/플레이어 맞으면 소멸
    }
}