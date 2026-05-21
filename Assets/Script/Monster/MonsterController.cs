using UnityEngine;

[RequireComponent(typeof(MonsterHealth))]
[RequireComponent(typeof(Animator))]
public class MonsterController : MonoBehaviour
{
    [SerializeField] private MonsterData data;

    public static readonly int MoveHash = Animator.StringToHash("Move");
    public static readonly int AttackHash = Animator.StringToHash("Attack");
    public static readonly int DamageHash = Animator.StringToHash("Damage");
    public static readonly int DieHash = Animator.StringToHash("Die");

    public MonsterData Data => data;
    public Animator Anim { get; private set; }
    public MonsterHealth Health { get; private set; }
    public Transform Target { get; private set; }
    public IMonsterState Current { get; private set; }

    private void Awake()
    {
        Anim = GetComponent<Animator>();
        Health = GetComponent<MonsterHealth>();
    }

    private void Start()
    {
        var p = GameObject.FindWithTag("Player");
        if (p != null) Target = p.transform;

        ChangeState(new MonsterIdleState());
    }

    private void OnEnable() 
    { 
        Health.OnDeath += HandleDeath;
        Health.OnDamaged += HandleDamaged;
    }
    private void OnDisable() 
    { 
        Health.OnDeath -= HandleDeath;
        Health.OnDamaged -= HandleDamaged;
    }

    private void Update()
    {
        if (Current == null) return;
        Current.Tick(this);
    }

    public void ChangeState(IMonsterState next)
    {
        Current?.Exit(this);
        Current = next;
        Current?.Enter(this);
    }
    public void FacePlayer()
    {
        Vector3 direction = (Target.position - transform.position);
        direction.y = 0;
        if (direction.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(direction.normalized);
    }

    public void ChasePlayer()
    {
        Vector3 direction = (Target.position - transform.position);
        direction.y = 0;
        direction = direction.normalized;

        transform.position += direction * data.moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);
        Anim.SetFloat(MoveHash, 1);
    }

    public void OnAttackHit()
    {
        if (Health.IsDead || Target == null) return;

        float distance = Vector3.Distance(transform.position, Target.position);
        if (distance <= data.attackRange + 0.5f)
        {
            var playerDamage = Target.GetComponent<HealthSystem>();
            if (playerDamage != null)
                playerDamage.TakeDamage(data.attackPower);
        }
    }

    public void PlayHitReaction()
    {
        if (Health.IsDead) return;
        Anim.SetTrigger(DamageHash);
    }
    private void HandleDamaged()
{
    if (Health.IsDead) return;             // 안전장치
    ChangeState(new MonsterDamagedState());
}

    private void HandleDeath()
    {
        if (Target.TryGetComponent<ExperienceSystem>(out var exp))
        {
            exp.AddExp(data.expReward);   // ← 이 줄이 빠짐
            Debug.Log($"[Exp] +{data.expReward} → Lv{exp.CurrentLevel}, {exp.CurrentExp}/{exp.ExpToNext}, points={exp.PendingPoints}");
        }
        ChangeState(new MonsterDeadState());
    }
}