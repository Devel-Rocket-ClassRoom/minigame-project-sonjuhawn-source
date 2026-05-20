using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    [SerializeField] private MonsterData data;

    private static readonly int MoveHash = Animator.StringToHash("Move");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int DamageHash = Animator.StringToHash("Damage");
    private static readonly int DieHash = Animator.StringToHash("Die");

    private Transform player;
    private MonsterHealth health;
    private Animator anim;
    private float lastAttackTime = -999f;

    private void Awake()
    {
        health = GetComponent<MonsterHealth>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        var playerGo = GameObject.FindWithTag("Player");
        if (playerGo != null) player = playerGo.transform;
    }

    private void OnEnable()
    {
        health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        health.OnDeath -= HandleDeath;
    }

    private void Update()
    {
        if (health.IsDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > data.detectRange)
        {
            Debug.Log(distance);
            anim.SetFloat(MoveHash, 0);
        }
        else if (distance > data.attackRange)
        {
            Debug.Log(distance);
            Debug.Log("chase");
            ChasePlayer();
        }
        else
        {
            Debug.Log(distance);
            Debug.Log("attack");
            // 공격 범위 안 — 정지 + 회전 + 공격 쿨타임 체크
            anim.SetFloat(MoveHash, 0);
            FacePlayer();

            if (Time.time - lastAttackTime >= data.attackCooldown)
            {
                anim.SetTrigger(AttackHash);
                lastAttackTime = Time.time;
            }
        }
    }

    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position);
        direction.y = 0;
        direction = direction.normalized;

        transform.position += direction * data.moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);
        anim.SetFloat(MoveHash, 1);
    }

    private void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position);
        direction.y = 0;
        if (direction.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(direction.normalized);
    }

    // === Animation Event: 공격 모션 타격 프레임에 호출 ===
    public void OnAttackHit()
    {
        if (health.IsDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= data.attackRange + 0.5f)
        {
            var playerDamage = player.GetComponent<HealthSystem>();
            if (playerDamage != null)
                playerDamage.TakeDamage(data.attackPower);
        }
    }

    // === 외부에서 호출 (PlayerCombat이 데미지 줄 때) — 피격 모션 ===
    public void PlayHitReaction()
    {
        if (health.IsDead) return;
        anim.SetTrigger(DamageHash);
    }

    private void HandleDeath()
    {
        anim.SetTrigger(DieHash);
        enabled = false;   // AI 중단 (Update 안 돔)
    }
}