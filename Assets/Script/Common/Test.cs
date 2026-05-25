using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform target;     // Player 드래그
    public int damage = 5;
    public float speed = 10f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))   // T 키 누르면 발사
        {
            var p = Instantiate(projectilePrefab, transform.position + Vector3.up, Quaternion.identity);
            Vector3 dir = (target.position - transform.position).normalized;
            p.GetComponent<MonsterProjectile>().Init(damage, speed, dir);
        }
    }
}
