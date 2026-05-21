using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    private readonly HashSet<IDamageable> hitThisSwing = new();

    private void OnEnable()
    {
        hitThisSwing.Clear();   // 스윙 시작마다 히트 기록 초기화
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var target))
        {
            if (hitThisSwing.Add(target))   // 이번 스윙에서 처음이면 true
                target.TakeDamage(damage);
        }
    }

    public void SetDamage(int value) => damage = value;
}