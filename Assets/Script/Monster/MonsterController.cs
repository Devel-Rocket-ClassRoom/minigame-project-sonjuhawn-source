using UnityEngine;

[RequireComponent(typeof(MonsterHealth))]
[RequireComponent(typeof(Animator))]
public class MonsterController : MonoBehaviour
{
    [SerializeField] private MonsterData data;

    public static readonly int MoveHash = Animator.StringToHash("Move");
    public static readonly int AttackHash = Animator.StringToHash("Attack");
    public static readonly int DamageHash = Animator.StringToHash("Damage");
    public static readonly int ChargeHash = Animator.StringToHash("Charge");
    public static readonly int HeavyHash = Animator.StringToHash("HeavyAttack");
    public static readonly int CancelHash = Animator.StringToHash("CancelCharge");
    public static readonly int RangedHash = Animator.StringToHash("RangedAttack");
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

    public void SetData(MonsterData d)
    {
        data = d;
        Health.Initialize(data);
    }

    public void ChangeState(IMonsterState next)
    {
        Current?.Exit(this);
        Current = next;
        Current?.Enter(this);
    }
    public void FacePlayer()
    {
        if (Target == null) return;
        Vector3 direction = (Target.position - transform.position);
        direction.y = 0;
        if (direction.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(direction.normalized);
    }

    public void ChasePlayer()
    {
        if (Target == null) return;
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

    public void OnHeavyAttackHit()
    {
        if (Health.IsDead || Target == null) return;
        float distance = Vector3.Distance(transform.position, Target.position);
        if (distance <= data.attackRange + 0.5f)
        {
            int dmg = data.heavyAttackPower > 0 ? data.heavyAttackPower : data.attackPower;
            Target.GetComponent<HealthSystem>()?.TakeDamage(dmg);
        }
    }
    public void OnRangedAttackFire()
    {
        if (Health.IsDead || Target == null || Data.projectilePrefab == null) return;

        Vector3 spawnPos = transform.TransformPoint(Data.muzzleLocalOffset);
        Vector3 dir = (Target.position + Vector3.up * 1f - spawnPos).normalized;  // 살짝 위로 조준 (플레이어 가슴쯤)

        var proj = Instantiate(Data.projectilePrefab, spawnPos, Quaternion.identity);
        var mp = proj.GetComponent<MonsterProjectile>();
        if (mp != null)
            mp.Init(Data.attackPower, Data.projectileSpeed, dir);
    }
    public void Retreat()
    {
        if (Target == null) return;
        Vector3 direction = (transform.position - Target.position);
        direction.y = 0;
        direction = direction.normalized;

        transform.position += direction * Data.moveSpeed * Time.deltaTime;
        Anim.SetFloat(MoveHash, 1);   // 또는 별도 BackStep 트리거
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
        if (Target != null && Target.TryGetComponent<ExperienceSystem>(out var exp))
        {
            exp.AddExp(data.expReward);   // ← 이 줄이 빠짐
            Debug.Log($"[Exp] +{data.expReward} → Lv{exp.CurrentLevel}, {exp.CurrentExp}/{exp.ExpToNext}, points={exp.PendingPoints}");
        }
        ChangeState(new MonsterDeadState());
    }

    public IMonsterState CreateAttackState()
    {
        if (Data.projectilePrefab != null)
            return new MonsterRangedAttackState();
        if (Data.telegraphTime > 0f)
            return new MonsterTelegraphAttackState();
        return new MonsterAttackState();
    }
}