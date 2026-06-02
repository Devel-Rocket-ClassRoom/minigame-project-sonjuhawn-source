using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private BossData data;

    private BossBlackboard bb;
    private BTNode root;
    private BossHealth health;


    private void Start()
    {
        // bb 초기화
        bb = new BossBlackboard
        {
            self = transform,
            target = GameObject.FindWithTag("Player").transform,
            anim = GetComponent<Animator>(),
            data = data
        };

        bb.onAttackFired = () => StartCoroutine(AttackCooldown());
        bb.onChargeFired = () => StartCoroutine(ChargeCooldown());
        bb.onRangedFired = () => StartCoroutine(RangedCooldown());

        // BT 트리 조립
        root = new BTSelector(new List<BTNode>
        {
            new ContinueChargeAction(),
            new ContinueTelegraphAction(),
            new BTSequence(new List<BTNode>
            {
                new CheckDistance(data.rangedRange),
                new DecidePatternAction(),
                new BTSelector(new List<BTNode>
                {
                    new MeleeAttackAction(),
                    new ChargeAction(),
                    new RangedAttackAction()
                })
            }),
            new ChaseAction()
        });

        health = GetComponent<BossHealth>();
        health.OnDeath += HandleDeath;
    }
    private void Update()
    {
        if (bb.isAppearing) return;
        bb.anim.SetFloat("Move", 0f);
        root.Execute(bb);
    }
    // 애니메이션 이벤트에서 호출
    public void OnAttackHit()
    {
        if (bb.target == null) return;

        float distance = Vector3.Distance(bb.self.position, bb.target.position);
        if (distance <= data.attackRange + 0.5f)
        {
            var playerHealth = bb.target.GetComponent<HealthSystem>();
            if (playerHealth != null)
                playerHealth.TakeDamage(data.attackPower);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!bb.isCharging) return;  // 돌진 중일 때만
        if (!other.CompareTag("Player")) return;

        var playerHealth = other.GetComponent<HealthSystem>();
        if (playerHealth != null)
            playerHealth.TakeDamage((int)(data.attackPower * 1.5f));
    }

    private void HandleDeath()
    {
        // BT 멈춤
        enabled = false;

        // 보상 지급
        if (bb.target.TryGetComponent<ExperienceSystem>(out var exp))
            exp.AddExp(data.expReward);
        if (bb.target.TryGetComponent<GoldSystem>(out var gold))
            gold.AddGold(data.goldReward);

        // 사망 애니
        bb.anim.SetTrigger("Die");
    }

    // 공격 쿨다운 코루틴
    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(data.attackCooldown);
        bb.isAttackCooldown = false;
    }
    private IEnumerator ChargeCooldown()
    {
        yield return new WaitForSeconds(data.chargeCooldown);
        bb.isChargeCooldown = false;
    }

    private IEnumerator RangedCooldown()
    {
        yield return new WaitForSeconds(data.rangedCooldown);
        bb.isRangedCooldown = false;
    }

    public void OnAppearFinished()
    {
        bb.isAppearing = false;
    }

    private void OnDestroy()
    {
        if (health != null)
            health.OnDeath -= HandleDeath;
    }
}